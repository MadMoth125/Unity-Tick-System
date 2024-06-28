using System.Collections.Generic;
using TickSystem.Core;
using UnityEngine;

namespace TickSystem
{
	/// <summary>
	/// Manages the update loop for all <see cref="TickGroup"/> instances found in the <see cref="TickManager"/> class.
	/// </summary>
	[DisallowMultipleComponent]
	public class TickUpdater : MonoBehaviour
	{
		/// <summary>
		/// Singleton instance of the TickUpdater.
		/// </summary>
		private static TickUpdater instance;

		private readonly List<MutableKeyValuePair<TickGroup, float>> _groupsAndTimers = new();

		/// <summary>
		/// Initializes instance of the TickUpdater.
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void InitializeInstance()
		{
			if (instance != null) return;
			var tempInst = new GameObject("Tick System Updater").AddComponent<TickUpdater>();
			DontDestroyOnLoad(tempInst.gameObject);
		}

		#region Unity Methods

		private void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
			else
			{
				Debug.LogWarning($"Duplicate instance of '{nameof(TickUpdater)}' detected. Destroying...");
				Destroy(this);
				return;
			}

			TickManager.OnTickGroupAdded += TickGroupAdded;
			TickManager.OnTickGroupRemoved += TickGroupRemoved;
		}

		private void OnDestroy()
		{
			TickManager.OnTickGroupAdded -= TickGroupAdded;
			TickManager.OnTickGroupRemoved -= TickGroupRemoved;
		}

		private void Start()
		{
			/* The TickGroupAssets and TickManager execute before the TickUpdater is initialized,
			 * so we need to manually register the tick groups that were already registered (if any).
			 * However, any tick groups registered after the TickUpdater is initialized will be automatically registered.
			 */
			if (TickManager.TickGroupInstances.Count == 0) return;
			foreach (var tickGroup in TickManager.TickGroupInstances)
			{
				TickGroupAdded(tickGroup);
			}
		}

		private void Update()
		{
			// Only update if the TickManager is active
			if (!TickManager.Active) return;

			// Using a for-loop to avoid the garbage allocation of a foreach-loop
			for (int i = 0; i < _groupsAndTimers.Count; i++)
			{
				// Skip null tick groups
				if (_groupsAndTimers[i] == null) continue;

				// Skip inactive tick groups
				if (!_groupsAndTimers[i].Key.Active()) continue;

				// Skip tick groups with no callbacks
				if (_groupsAndTimers[i].Key.CallbackCount == 0) continue;

				// Skip tick groups with a tick rate of 0
				if (_groupsAndTimers[i].Key.Interval() <= 0) continue;

				/* We split the deltaTime values in half to increment the timer in two steps rather than one.
				 * This is meant to alleviate the issue of the timer overshooting the specified interval value.
				 *
				 * This method seems to work, but it's not perfect.
				 * The timer will still never reach the exact interval value and will overshoot it,
				 * albeit by a smaller margin using the split method.
				 */
				if (_groupsAndTimers[i].Key.UseRealTime())
				{
					float halfDelta = Time.unscaledDeltaTime * 0.5f;
					if ((_groupsAndTimers[i].Value += halfDelta) <= _groupsAndTimers[i].Key.Interval())
					{
						_groupsAndTimers[i].Value += halfDelta;
						continue;
					}
				}
				else
				{
					// using unscaledDeltaTime and timeScale to fix sync issues with deltaTime
					float halfDelta = (Time.unscaledDeltaTime * Time.timeScale) * 0.5f;
					if ((_groupsAndTimers[i].Value += halfDelta) <= _groupsAndTimers[i].Key.Interval())
					{
						_groupsAndTimers[i].Value += halfDelta;
						continue;
					}
				}

				_groupsAndTimers[i].Value = 0f;
				_groupsAndTimers[i].Key.Invoke();
			}
		}

		#endregion

		private void TickGroupAdded(TickGroup group)
		{
			// Check if the tick group is already registered
			if (_groupsAndTimers.Exists(pair => pair.Key == group)) return;

			// Add the tick group to the list
			_groupsAndTimers.Add(new MutableKeyValuePair<TickGroup, float>(group, 0f));
		}

		private void TickGroupRemoved(TickGroup group)
		{
			// Find the MutableKeyValuePair with the tick group so we can modify it before removing it
			var groupValuePair = _groupsAndTimers.Find(pair => pair.Key == group);
			if (groupValuePair == null) return;

			// Remove the tick group from the list
			_groupsAndTimers.Remove(groupValuePair);
		}
	}
}