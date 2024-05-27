using System.Collections.Generic;

namespace TickSystem.Core
{
	public class Ticker
	{
		public bool active = true;

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
		
		public void Add(TickGroup tickGroup)
		{
			if (tickGroup == null) return;
			if (_groupsAndTimers.Exists(pair => pair.Key == tickGroup)) return;
			_groupsAndTimers.Add(new MutableKeyValuePair<TickGroup, float>(tickGroup, 0f));
		}
		
		public void Remove(TickGroup tickGroup)
		{
			if (tickGroup == null) return;
			if (!_groupsAndTimers.Exists(pair => pair.Key == tickGroup)) return;
			_groupsAndTimers.RemoveAll(pair => pair.Key == tickGroup);
		}
		
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
		
		public void ResetTimers()
		{
			if (_groupsAndTimers.Count == 0) return;
			foreach (var mutableKeyValuePair in _groupsAndTimers)
			{
				mutableKeyValuePair.Value = 0f;
			}
		}
		
		// dt = Time.deltaTime, udt = Time.unscaledDeltaTime
		// not using Time.deltaTime or Time.unscaledDeltaTime directly
		// as to keep the class independent from any Unity APIs
		public void Update(float dt, float udt)
		{
			// Skip if the ticker is inactive
			if (!active) return;
			
			for (int i = 0; i < _groupsAndTimers.Count; i++)
			{
				// Skip inactive tick groups
				if (!_groupsAndTimers[i].Key.Parameters.active) continue;
				
				// Skip tick groups with a tick rate of 0
				if (_groupsAndTimers[i].Key.Parameters.tickRate <= 0) continue;
				
				// Branching based on real-time or unscaled time
				if (_groupsAndTimers[i].Key.Parameters.useRealTime)
				{
					if ((_groupsAndTimers[i].Value += udt) < (1f / _groupsAndTimers[i].Key.Parameters.tickRate)) continue;
				}
				else
				{
					if ((_groupsAndTimers[i].Value += dt) < (1f / _groupsAndTimers[i].Key.Parameters.tickRate)) continue;
				}
				
				// Reset the timer and invoke the tick group's callbacks
				_groupsAndTimers[i].Value = 0f;
				_groupsAndTimers[i].Key.Invoke();
			}
		}

	}
}