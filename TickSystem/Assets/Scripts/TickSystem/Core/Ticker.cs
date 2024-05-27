using System.Collections.Generic;

namespace TickSystem.Core
{
	public class Ticker
	{
		public bool active = true;

		public IReadOnlyList<TickGroup> TickGroups => _tickGroups;
		
		// TODO: merge into List<KeyValuePair<TickGroup, float>>
		private readonly List<TickGroup> _tickGroups;
		private readonly List<float> _timers;

		#region Constructors

		public Ticker(bool active, IEnumerable<TickGroup> tickGroups)
		{
			this.active = active;
			_tickGroups = new List<TickGroup>(tickGroups);
			_timers = MakeDefaultTimers(_tickGroups.Count);
		}
		
		public Ticker(bool active, params TickGroup[] tickGroups)
		{
			this.active = active;
			_tickGroups = new List<TickGroup>(tickGroups);
			_timers = MakeDefaultTimers(_tickGroups.Count);
		}

		public Ticker(bool active)
		{
			this.active = active;
			_tickGroups = new List<TickGroup>();
			_timers = new List<float>();
		}

		public Ticker(IEnumerable<TickGroup> tickGroups)
		{
			_tickGroups = new List<TickGroup>(tickGroups);
			_timers = MakeDefaultTimers(_tickGroups.Count);
		}
		
		public Ticker(params TickGroup[] tickGroups)
		{
			_tickGroups = new List<TickGroup>(tickGroups);
			_timers = MakeDefaultTimers(_tickGroups.Count);
		}
		
		public Ticker()
		{
			_tickGroups = new List<TickGroup>();
			_timers = new List<float>();
		}

		#endregion
		
		public void Add(TickGroup tickGroup)
		{
			if (_tickGroups.Contains(tickGroup)) return;
			_tickGroups.Add(tickGroup);
			_timers.Add(0f);
		}
		
		public void Remove(TickGroup tickGroup)
		{
			// Essentially a !Contains() check,
			// but we now have the index for both lists
			int index = _tickGroups.IndexOf(tickGroup);
			if (index < 0) return; // -1 means not found
			
			_tickGroups.RemoveAt(index);
			_timers.RemoveAt(index);
		}
		
		public void Clear()
		{
			if (_tickGroups.Count == 0) return;
			_tickGroups.Clear();
			_timers.Clear();
		}
		
		public void ResetTimers()
		{
			for (int i = 0; i < _timers.Count; i++)
			{
				_timers[i] = 0f;
			}
		}
		
		// dt = Time.deltaTime, udt = Time.unscaledDeltaTime
		// not using Time.deltaTime or Time.unscaledDeltaTime directly
		// as to keep the class independent from any Unity APIs
		public void Update(float dt, float udt)
		{
			if (!active) return;
			
			int count = MatchListLengths();
			if (count == 0) return;
			
			for (int i = 0; i < count; i++)
			{
				if (!_tickGroups[i].Parameters.active) continue;
				if (_tickGroups[i].Parameters.tickRate <= 0) continue;
				
				if (_tickGroups[i].Parameters.useRealTime)
				{
					if (_timers[i] + udt < 1f / _tickGroups[i].Parameters.tickRate) continue;
				}
				else
				{
					if (_timers[i] + dt < 1f / _tickGroups[i].Parameters.tickRate) continue;
				}
				
				_tickGroups[i].Invoke();
				_timers[i] = 0f;
			}
		}
		
		private int MatchListLengths()
		{
			if (_tickGroups.Count == _timers.Count) return _timers.Count;
			
			if (_tickGroups.Count > _timers.Count)
			{
				int difference = _tickGroups.Count - _timers.Count;
				for (int i = 0; i < difference; i++)
				{
					_timers.Add(0f);
				}
			}
			else if (_timers.Count > _tickGroups.Count)
			{
				int difference = _timers.Count - _tickGroups.Count;
				for (int i = 0; i < difference; i++)
				{
					_timers.RemoveAt(_timers.Count - 1);
				}
			}
			
			return _timers.Count;
		}
		
		private List<float> MakeDefaultTimers(int count)
		{
			List<float> timers = new List<float>();
			for (int i = 0; i < count; i++)
			{
				timers.Add(0f);
			}
			return timers;
		}
	}
}