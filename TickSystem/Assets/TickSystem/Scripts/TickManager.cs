#define REVERSE_CLEAR

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TickSystem
{
	/// <summary>
	/// This class should not be manually instantiated,
	/// as it auto-instantiates an instance when the game starts.
	/// </summary>
	[DisallowMultipleComponent]
	public class TickManager : MonoBehaviour
	{
		/// <summary>
		/// Invoked when a TickGroup has been added to the manager
		/// </summary>
		public static event Action<TickGroup> OnTickGroupAdded = delegate {  };

		/// <summary>
		/// Invoked when a TickGroup has been removed from the manager
		/// </summary>
		public static event Action<TickGroup> OnTickGroupRemoved = delegate {  };

		// Singleton instance
		private static TickManager _instance;

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

		private static readonly List<MutableKeyValuePair<TickGroup, float>> GroupsAndTimers = new();

		private static readonly Queue<TickGroup> AddedTickGroups = new();

		// Initializes an instance of the TickManager.
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Init()
		{
			if (_instance != null) return;
			var inst = new GameObject("TickManager").AddComponent<TickManager>();
			DontDestroyOnLoad(inst.gameObject);
		}

		/// <summary>
		/// Add a TickGroup to the manager.
		/// </summary>
		/// <param name="tickGroup"></param>
		public static void Add(TickGroup tickGroup)
		{
			if (tickGroup == null)
			{
				if (EnableLogging())
				{
					Debug.LogWarning($"Failed to add '{nameof(TickGroup)}': {nameof(NullReferenceException)}", _instance);
				}
				return;
			}

			if (Contains(tickGroup))
			{
				if (EnableLogging())
				{
					Debug.LogWarning($"Failed to add '{nameof(TickGroup)}.{tickGroup.Name}': Instance already exists in {nameof(TickManager)}.", _instance);
				}
				return;
			}

			GroupsAndTimers.Add(new MutableKeyValuePair<TickGroup, float>(tickGroup, 0f));

			// If the singleton instance is not initialized when adding TickGroups,
			// other scripts likely can't respond to the event. Queue TickGroups for
			// later if the instance is null, else invoke the event immediately.
			if (_instance != null)
			{
				OnTickGroupAdded.Invoke(tickGroup);
			}
			else
			{
				AddedTickGroups.Enqueue(tickGroup);
			}
		}

		/// <summary>
		/// Remove a TickGroup from the manager.
		/// </summary>
		/// <param name="tickGroup"></param>
		public static void Remove(TickGroup tickGroup)
		{
			if (tickGroup == null)
			{
				if (EnableLogging())
				{
					Debug.LogWarning($"Failed to remove '{nameof(TickGroup)}': {nameof(NullReferenceException)}", _instance);
				}
				return;
			}

			if (!Contains(tickGroup))
			{
				if (EnableLogging())
				{
					Debug.LogWarning($"Failed to remove '{nameof(TickGroup)}.{tickGroup.Name}': Instance doesn't exist in {nameof(TickManager)}.", _instance);
				}
				return;
			}

			GroupsAndTimers.RemoveAt(IndexOf(tickGroup));
			OnTickGroupRemoved.Invoke(tickGroup);
		}

		/// <summary>
		/// Find the first TickGroup with a matching name.
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
		/// Find the first TickGroup with a matching name.
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
		/// Check if a TickGroup with a matching name is already referenced by the manager.
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
		/// Check if the TickGroup is already referenced by the manager.
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
				if (EnableLogging())
				{
					Debug.Log($"Multiple instances of '{nameof(TickManager)}' detected. Destroying duplicate.");
				}
				Destroy(this);
				return;
			}

			_instance = this;
		}

		private void Start()
		{
			// If we have any queued events, invoke them on Start
			// so other scripts can receive the added instances.
			foreach (TickGroup tickGroup in AddedTickGroups)
			{
				OnTickGroupAdded.Invoke(tickGroup);
			}

			AddedTickGroups.Clear();
		}

		private void Update()
		{
			// Early return if we have no TickGroups
			if (GroupsAndTimers.Count == 0) return;

			// Using a for-loop to avoid the garbage allocation of a foreach-loop
			for (int i = 0; i < GroupsAndTimers.Count; i++)
			{
				// Skipping groups that are null, disabled,
				// have 0 callbacks, or have invalid intervals.
				if (GroupsAndTimers[i] == null ||
				    GroupsAndTimers[i].Key == null ||
				    GroupsAndTimers[i].Key.Count == 0 ||
				    GroupsAndTimers[i].Key.Enabled == false ||
				    GroupsAndTimers[i].Key.Interval <= 0) continue;

				if (GroupsAndTimers[i].Key.RealTime)
				{
					float halfDelta = Time.unscaledDeltaTime * 0.5f;
					if ((GroupsAndTimers[i].Value += halfDelta) <= GroupsAndTimers[i].Key.Interval)
					{
						GroupsAndTimers[i].Value += halfDelta;
						continue;
					}
				}
				else
				{
					// using unscaledDeltaTime and timeScale to fix sync issues with deltaTime
					float halfDelta = (Time.unscaledDeltaTime * Time.timeScale) * 0.5f;
					if ((GroupsAndTimers[i].Value += halfDelta) <= GroupsAndTimers[i].Key.Interval)
					{
						GroupsAndTimers[i].Value += halfDelta;
						continue;
					}
				}

				GroupsAndTimers[i].Value = 0f;
				GroupsAndTimers[i].Key.Invoke();
			}
		}

		private void OnDestroy()
		{
			// Call events upon instance being destroyed
			while (GroupsAndTimers.Count > 0)
			{
				#if !REVERSE_CLEAR
				// Remove TickGroups last-to-first
				Remove(GroupsAndTimers[^1].Key);
				#else
				// Remove TickGroups first-to-last
				Remove(GroupsAndTimers[0].Key);
				#endif
			}
		}

		#endregion

		// Finds the index of the MutableKeyValuePair that contains the TickGroup
		private static int IndexOf(TickGroup tickGroup)
		{
			List<TickGroup> groups = GroupsAndTimers.Select(x => x.Key).ToList();
			return groups.IndexOf(tickGroup);
		}

		// Controls whether Debug.Log() methods are called in class methods.
		private static bool EnableLogging()
		{
			#if UNITY_EDITOR
			return false;
			#else
			return false
			#endif
		}
	}
}