using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TickSystem
{
	[DisallowMultipleComponent]
	public class TickManager_New : MonoBehaviour
	{
		/// <summary>
		/// Invoked when a unique TickGroup has been added to the manager
		/// </summary>
		public static event Action<TickGroup> OnTickGroupAdded = delegate {  };

		/// <summary>
		/// Invoked when a TickGroup has been removed from the manager
		/// </summary>
		public static event Action<TickGroup> OnTickGroupRemoved = delegate {  };

		/// <summary>
		/// The enabled state of the TickManager.
		/// </summary>
		/// <remarks>
		/// Automatic TickGroup updates are only active when the TickManager is enabled.
		/// </remarks>
		public static bool Enabled
		{
			get => _instance.enabled;
			set => _instance.enabled = value;
		}

		// private singleton instance
		private static TickManager_New _instance;

		// private static TickGroup reference collection
		private static readonly List<MutableKeyValuePair<TickGroup, float>> GroupsAndTimers = new();

		// Initializes an instance of the TickManager.
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Init()
		{
			if (_instance != null) return;
			var tempInst = new GameObject("TickManager").AddComponent<TickManager_New>();
			DontDestroyOnLoad(tempInst.gameObject);
		}

		/// <summary>
		/// Adds a TickGroup to the manager.
		/// </summary>
		/// <param name="tickGroup"></param>
		public static void Add(TickGroup tickGroup)
		{
			if (_instance == null) return;

			if (tickGroup == null)
			{
				Debug.LogWarning($"Failed to add '{nameof(TickGroup)}': {nameof(NullReferenceException)}", _instance);
				return;
			}

			if (Contains(tickGroup))
			{
				Debug.LogWarning($"Failed to add '{nameof(TickGroup)}.{tickGroup.GetName()}': Instance already exists.", _instance);
				return;
			}

			GroupsAndTimers.Add(new(tickGroup, 0f));
			OnTickGroupAdded.Invoke(tickGroup);
		}

		/// <summary>
		/// Removes a TickGroup from the manager.
		/// </summary>
		/// <param name="tickGroup"></param>
		public static void Remove(TickGroup tickGroup)
		{
			if (_instance == null) return;

			if (tickGroup == null)
			{
				Debug.LogWarning($"Failed to remove '{nameof(TickGroup)}': {nameof(NullReferenceException)}", _instance);
				return;
			}

			if (!Contains(tickGroup))
			{
				Debug.LogWarning($"Failed to remove '{nameof(TickGroup)}.{tickGroup.GetName()}': Instance doesn't exists.", _instance);
				return;
			}

			GroupsAndTimers.RemoveAt(IndexOf(tickGroup));
			OnTickGroupRemoved.Invoke(tickGroup);
		}

		/// <summary>
		/// Finds the first TickGroup with a matching name.
		/// </summary>
		/// <param name="name"></param>
		/// <remarks>
		/// See <see cref="TickGroup.CompareName"/> for comparision details.
		/// </remarks>
		public static TickGroup Find(string name)
		{
			var result = GroupsAndTimers.Find(x => TickGroup.CompareName(x.Key, name));
			return result?.Key;
		}

		/// <summary>
		/// Finds the first TickGroup with a matching name.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="tickGroup"></param>
		/// <returns>Whether a matching TickGroup was found.</returns>
		/// <remarks>
		/// See <see cref="TickGroup.CompareName"/> for comparision details.
		/// </remarks>
		public static bool Find(string name, out TickGroup tickGroup)
		{
			tickGroup = GroupsAndTimers.Find(x => TickGroup.CompareName(x.Key, name))?.Key;
			return tickGroup != null;
		}

		/// <summary>
		/// Checks if a TickGroup with a matching name is already referenced by the manager.
		/// </summary>
		/// <param name="name"></param>
		/// <returns>Whether a matching TickGroup was found.</returns>
		/// <remarks>
		/// See <see cref="TickGroup.CompareName"/> for comparision details.
		/// </remarks>
		public static bool Contains(string name)
		{
			return GroupsAndTimers.Any(x => TickGroup.CompareName(x.Key, name));
		}

		/// <summary>
		/// Checks if the TickGroup is already referenced by the manager.
		/// </summary>
		/// <param name="tickGroup"></param>
		/// <returns>Whether a matching TickGroup was found.</returns>
		public static bool Contains(TickGroup tickGroup)
		{
			return tickGroup != null && GroupsAndTimers.Any(x => x.Key == tickGroup);
		}

		#region Unity Methods

		private void Awake()
		{
			if (_instance != null)
			{
				Debug.Log($"Multiple instances of '{nameof(TickManager_New)}' detected. Destroying...");
				Destroy(this); // Remove the script from the Object
				return;
			}

			_instance = this;
		}

		private void Update()
		{
			// Redundant check, but just making sure
			// it doesn't update groups when disabled.
			if (!enabled) return;

			// Early return if we have no TickGroups
			if (GroupsAndTimers.Count == 0) return;

			// Using a for-loop to avoid the garbage allocation of a foreach-loop
			for (int i = 0; i < GroupsAndTimers.Count; i++)
			{
				// Filtering groups that won't be ticking
				if (GroupsAndTimers[i] == null) continue;
				if (!GroupsAndTimers[i].Key.IsEnabled()) continue;
				if (GroupsAndTimers[i].Key.Count == 0) continue;
				if (GroupsAndTimers[i].Key.GetInterval() <= 0) continue;

				if (GroupsAndTimers[i].Key.IsRealTime())
				{
					float halfDelta = Time.unscaledDeltaTime * 0.5f;
					if ((GroupsAndTimers[i].Value += halfDelta) <= GroupsAndTimers[i].Key.GetInterval())
					{
						GroupsAndTimers[i].Value += halfDelta;
						continue;
					}
				}
				else
				{
					// using unscaledDeltaTime and timeScale to fix sync issues with deltaTime
					float halfDelta = (Time.unscaledDeltaTime * Time.timeScale) * 0.5f;
					if ((GroupsAndTimers[i].Value += halfDelta) <= GroupsAndTimers[i].Key.GetInterval())
					{
						GroupsAndTimers[i].Value += halfDelta;
						continue;
					}
				}

				GroupsAndTimers[i].Value = 0f;
				GroupsAndTimers[i].Key.Invoke();
			}
		}

		#endregion

		// Finds the index of the MutableKeyValuePair that contains the TickGroup
		private static int IndexOf(TickGroup tickGroup)
		{
			var groups = GroupsAndTimers.Select(x => x.Key).ToList();
			return groups.IndexOf(tickGroup);
		}
	}
}