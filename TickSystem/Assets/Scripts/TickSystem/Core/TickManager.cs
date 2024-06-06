using System;
using System.Collections.Generic;

namespace TickSystem.Core
{
	public static class TickManager
	{
		public static event Action<TickGroup> OnTickGroupRegistered = delegate {  };
		public static event Action<TickGroup> OnTickGroupUnregistered = delegate {  };
		public static event Action<bool> OnActiveStateChanged = delegate {  };

		/// <summary>
		/// Read-only list of all registered tick groups.
		/// </summary>
		public static IReadOnlyList<TickGroup> TickGroupInstances => tickGroups;

		/// <summary>
		/// Whether the tick manager is active.
		/// Enabling will allow tick groups to update, and disabling will prevent it.
		/// </summary>
		public static bool Active
		{
			get => active;
			set
			{
				if (active == value) return;
				active = value;
				OnActiveStateChanged.Invoke(value);
			}
		}

		private static bool active = true;
		private static readonly List<TickGroup> tickGroups = new();

		/// <summary>
		/// Registers a new tick group to the manager.
		/// </summary>
		/// <param name="tickGroup">The tick group to register.</param>
		public static void RegisterTickGroup(TickGroup tickGroup)
		{
			if (tickGroup == null || tickGroups.Contains(tickGroup)) return;
			tickGroups.Add(tickGroup);
			OnTickGroupRegistered.Invoke(tickGroup);
		}

		/// <summary>
		/// Unregisters an existing tick group from the manager.
		/// </summary>
		/// <param name="tickGroup">The tick group to unregister.</param>
		public static void UnregisterTickGroup(TickGroup tickGroup)
		{
			if (tickGroup == null || !tickGroups.Contains(tickGroup)) return;
			OnTickGroupUnregistered.Invoke(tickGroup);
			tickGroups.Remove(tickGroup);
		}

		public static TickGroup FindTickGroup(string name)
		{
			return tickGroups.Find(tg => TickGroup.CompareName(tg, name));
		}

		public static bool FindTickGroup(string name, out TickGroup tickGroup)
		{
			tickGroup = tickGroups.Find(tg => TickGroup.CompareName(tg, name));
			return tickGroup != null;
		}
	}
}