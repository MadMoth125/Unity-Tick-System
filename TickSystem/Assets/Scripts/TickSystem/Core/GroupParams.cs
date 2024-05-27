using System;

namespace TickSystem.Core
{
	[Serializable]
	public struct GroupParams
	{
		public string name;
		public int tickRate;
		public bool active;
		public bool useRealTime;
		
		public static GroupParams Default => new GroupParams("TickGroup", 20, true, false);

		#region Constructors

		public GroupParams(string name, int tickRate, bool active, bool useRealTime)
		{
			this.name = name;
			this.tickRate = tickRate;
			this.active = active;
			this.useRealTime = useRealTime;
		}
		
		public GroupParams(string name, int tickRate, bool active)
		{
			this.name = name;
			this.tickRate = tickRate;
			this.active = active;
			this.useRealTime = false;
		}
		
		public GroupParams(string name, int tickRate)
		{
			this.name = name;
			this.tickRate = tickRate;
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
			tickRate = other.tickRate;
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
		public void Set(string name, int tickRate, bool active, bool useRealTime)
		{
			this.name = name;
			this.tickRate = tickRate;
			this.active = active;
			this.useRealTime = useRealTime;
		}

		#region Overrides

		public override string ToString()
		{
			return $"[{name}, {tickRate}, {active}, {useRealTime}]";
		}
		
		public override bool Equals(object obj)
		{
			if (obj is GroupParams other)
			{
				return name == other.name && tickRate == other.tickRate && active == other.active && useRealTime == other.useRealTime;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(name, tickRate, active, useRealTime);
		}

		#endregion

		/// <summary>
		/// Checks equality with another GroupParams instance.
		/// </summary>
		/// <param name="other">The GroupParams instance to compare with.</param>
		/// <returns>True if equal, otherwise false.</returns>
		public bool Equals(GroupParams other)
		{
			return name == other.name && tickRate == other.tickRate && active == other.active && useRealTime == other.useRealTime;
		}
	}
}