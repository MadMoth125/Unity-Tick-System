using System;

namespace TickSystem.Core
{
	[Serializable]
	public struct GroupParams
	{
		public string name;
		public float interval;
		public bool active;
		public bool useRealTime;
		
		public static GroupParams Default => new GroupParams("TickGroup", 0.05f, true, false);

		#region Constructors

		public GroupParams(string name, float interval, bool active, bool useRealTime)
		{
			this.name = name;
			this.interval = interval;
			this.active = active;
			this.useRealTime = useRealTime;
		}
		
		public GroupParams(string name, float interval, bool active)
		{
			this.name = name;
			this.interval = interval;
			this.active = active;
			this.useRealTime = false;
		}
		
		public GroupParams(string name, float interval)
		{
			this.name = name;
			this.interval = interval;
			this.active = true;
			this.useRealTime = false;
		}

		#endregion
		
		/// <summary>
		/// Copies values from another GroupParams instance.
		/// </summary>
		/// <param name="other">The GroupParams instance to copy values from.</param>
		public void CopyFrom(GroupParams other)
		{
			name = other.name;
			interval = other.interval;
			active = other.active;
			useRealTime = other.useRealTime;
		}

		/// <summary>
		/// Sets the parameters of the group.
		/// </summary>
		/// <param name="name">The new name.</param>
		/// <param name="tickRate">The new tick rate.</param>
		/// <param name="active">The new active state.</param>
		/// <param name="useRealTime">Whether to use real time.</param>
		public GroupParams Set(string name, float tickRate, bool active, bool useRealTime)
		{
			this.name = name;
			this.interval = tickRate;
			this.active = active;
			this.useRealTime = useRealTime;
			return this;
		}

		#region Overrides

		public override string ToString()
		{
			return $"[{name}, {interval}, {active}, {useRealTime}]";
		}
		
		public override bool Equals(object obj)
		{
			if (obj is GroupParams other)
			{
				return name == other.name && Math.Abs(interval - other.interval) < 0.0001f && active == other.active && useRealTime == other.useRealTime;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(name, interval, active, useRealTime);
		}

		#endregion

		/// <summary>
		/// Checks equality with another GroupParams instance.
		/// </summary>
		/// <param name="other">The GroupParams instance to compare with.</param>
		/// <returns>True if equal, otherwise false.</returns>
		public bool Equals(GroupParams other)
		{
			return name == other.name && Math.Abs(interval - other.interval) < 0.0001f && active == other.active && useRealTime == other.useRealTime;
		}
	}
}