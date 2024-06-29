using System;
using System.Collections.Generic;

namespace TickSystem.Core
{
	/// <summary>
	/// Serves as a centralized system to manage and reference any existing TickGroups.
	/// </summary>
	[Obsolete]
	public static class TickManager
	{
		public static event Action<TickGroup> OnTickGroupAdded = delegate {  };
		public static event Action<TickGroup> OnTickGroupRemoved = delegate {  };
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
		/// Adds a TickGroup to the manager.
		/// </summary>
		/// <param name="tickGroup"></param>
		public static void Add(TickGroup tickGroup)
		{
			if (tickGroup == null || tickGroups.Contains(tickGroup)) return;
			tickGroups.Add(tickGroup);
			OnTickGroupAdded.Invoke(tickGroup);
		}

		/// <summary>
		/// Removes a TickGroup from the manager.
		/// </summary>
		/// <param name="tickGroup"></param>
		public static void Remove(TickGroup tickGroup)
		{
			if (tickGroup == null || !tickGroups.Contains(tickGroup)) return;
			OnTickGroupRemoved.Invoke(tickGroup);
			tickGroups.Remove(tickGroup);
		}

		public static TickGroup Find(string name)
		{
			return tickGroups.Find(tg => TickGroup.CompareName(tg, name));
		}

		public static bool Find(string name, out TickGroup tickGroup)
		{
			tickGroup = tickGroups.Find(tg => TickGroup.CompareName(tg, name));
			return tickGroup != null;
		}

		public static bool Contains(string name)
		{
			return tickGroups.Find(tg => TickGroup.CompareName(tg, name)) != null;
		}
	}
}