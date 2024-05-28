using System.Collections.Generic;

namespace TickSystem.Core
{
	public class Ticker
	{
		/// <summary>
		/// Whether the ticker is active.
		/// </summary>
		public bool active = true;

		/// <summary>
		/// The tick groups managed by this ticker.
		/// </summary>
		public IReadOnlyList<TickGroup> TickGroups => _groupsAndTimers.ConvertAll(pair => pair.Key);

		private readonly List<MutableKeyValuePair<TickGroup, float>> _groupsAndTimers;

		#region Constructors

		public Ticker(bool active, IEnumerable<TickGroup> tickGroups)
		{
			this.active = active;
			_groupsAndTimers = new List<MutableKeyValuePair<TickGroup, float>>();
			foreach (var tickGroup in tickGroups)
			{
				_groupsAndTimers.Add(new MutableKeyValuePair<TickGroup, float>(tickGroup, 0f));
			}
		}
		
		public Ticker(bool active, params TickGroup[] tickGroups)
		{
			this.active = active;
			_groupsAndTimers = new List<MutableKeyValuePair<TickGroup, float>>();
			foreach (var tickGroup in tickGroups)
			{
				_groupsAndTimers.Add(new MutableKeyValuePair<TickGroup, float>(tickGroup, 0f));
			}
		}

		public Ticker(bool active)
		{
			this.active = active;
			_groupsAndTimers = new List<MutableKeyValuePair<TickGroup, float>>();
		}

		public Ticker(IEnumerable<TickGroup> tickGroups)
		{
			_groupsAndTimers = new List<MutableKeyValuePair<TickGroup, float>>();
			foreach (var tickGroup in tickGroups)
			{
				_groupsAndTimers.Add(new MutableKeyValuePair<TickGroup, float>(tickGroup, 0f));
			}
		}
		
		public Ticker(params TickGroup[] tickGroups)
		{
			_groupsAndTimers = new List<MutableKeyValuePair<TickGroup, float>>();
			foreach (var tickGroup in tickGroups)
			{
				_groupsAndTimers.Add(new MutableKeyValuePair<TickGroup, float>(tickGroup, 0f));
			}
		}
		
		public Ticker()
		{
			_groupsAndTimers = new List<MutableKeyValuePair<TickGroup, float>>();
		}

		#endregion

		/// <summary>
		/// Adds a tick group to the ticker.
		/// </summary>
		/// <param name="tickGroup">The tick group to add.</param>
		public void Add(TickGroup tickGroup)
		{
			if (tickGroup == null) return;
			if (_groupsAndTimers.Exists(pair => pair.Key == tickGroup)) return;
			_groupsAndTimers.Add(new MutableKeyValuePair<TickGroup, float>(tickGroup, 0f));
		}

		/// <summary>
		/// Removes a tick group from the ticker.
		/// </summary>
		/// <param name="tickGroup">The tick group to remove.</param>
		public void Remove(TickGroup tickGroup)
		{
			if (tickGroup == null) return;
			if (!_groupsAndTimers.Exists(pair => pair.Key == tickGroup)) return;
			_groupsAndTimers.RemoveAll(pair => pair.Key == tickGroup);
		}

		/// <summary>
		/// Clears all tick groups from the ticker.
		/// </summary>
		public void Clear()
		{
			if (_groupsAndTimers.Count == 0) return;
			foreach (var mutableKeyValuePair in _groupsAndTimers)
			{
				// Clear the tick group's callbacks
				mutableKeyValuePair.Key.Clear();
			}
			_groupsAndTimers.Clear();
		}

		/// <summary>
		/// Sets the tick groups for the ticker.
		/// </summary>
		/// <param name="tickGroups">The tick groups to set.</param>
		public void SetTickGroups(IEnumerable<TickGroup> tickGroups)
		{
			Clear();
			foreach (var tickGroup in tickGroups)
			{
				_groupsAndTimers.Add(new MutableKeyValuePair<TickGroup, float>(tickGroup, 0f));
			}
		}

		/// <summary>
		/// Resets the timers for all tick groups.
		/// </summary>
		public void ResetTimers()
		{
			if (_groupsAndTimers.Count == 0) return;
			foreach (var mutableKeyValuePair in _groupsAndTimers)
			{
				mutableKeyValuePair.Value = 0f;
			}
		}
		
		/// <summary>
		/// Updates the ticker and invokes callbacks based on delta time.
		/// </summary>
		/// <param name="dt">The delta time.</param>
		/// <param name="udt">The unscaled delta time.</param>
		public void Update(float dt, float udt)
		{
			// Skip if the ticker is inactive
			if (!active) return;
			
			for (int i = 0; i < _groupsAndTimers.Count; i++)
			{
				// Skip null tick groups
				if (_groupsAndTimers[i] == null) continue;
				
				// Skip inactive tick groups
				if (!_groupsAndTimers[i].Key.parameters.active) continue;
				
				// Skip tick groups with a tick rate of 0
				if (_groupsAndTimers[i].Key.parameters.interval <= 0) continue;
				
				// Branching based on real-time or unscaled time
				if (_groupsAndTimers[i].Key.parameters.useRealTime)
				{
					if ((_groupsAndTimers[i].Value += udt) < _groupsAndTimers[i].Key.parameters.interval) continue;
				}
				else
				{
					if ((_groupsAndTimers[i].Value += dt) < _groupsAndTimers[i].Key.parameters.interval) continue;
				}
				
				// Reset the timer and invoke the tick group's callbacks
				_groupsAndTimers[i].Value = 0f;
				_groupsAndTimers[i].Key.Invoke();
			}
		}

	}
}