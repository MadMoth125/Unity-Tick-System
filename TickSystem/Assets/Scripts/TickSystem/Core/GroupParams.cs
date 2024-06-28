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

		/// <summary>
		/// Default parameters for a tick group.
		/// Values: name = "TickGroup", interval = 0.05f, active = true, useRealTime = false.
		/// </summary>
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
		/// Sets the parameters of the TickGroup.
		/// </summary>
		/// <param name="other"></param>
		public void Set(in GroupParams other)
		{
			this.name = other.name;
			this.interval = other.interval;
			this.active = other.active;
			this.useRealTime = other.useRealTime;
		}

		/// <summary>
		/// Sets the parameters of the TickGroup.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="interval"></param>
		/// <param name="active"></param>
		/// <param name="useRealTime"></param>
		public void Set(string name = "", float? interval = null, bool? active = null, bool? useRealTime = null)
		{
			if (name != "") this.name = name;
			if (interval != null) this.interval = interval.Value;
			if (active != null) this.active = active.Value;
			if (useRealTime != null) this.useRealTime = useRealTime.Value;
		}

		#region Overrides

		public override string ToString() => $"[name: {name}, interval: {interval}, active: {active}, useRealTime: {useRealTime}]";

		public override bool Equals(object obj)
		{
			if (obj is GroupParams other)
			{
				return this.name == other.name &&
				       Math.Abs(this.interval - other.interval) < 0.0001f &&
				       this.active == other.active &&
				       this.useRealTime == other.useRealTime;
			}
			return false;
		}

		public override int GetHashCode() => HashCode.Combine(name, interval, active, useRealTime);

		public static bool operator ==(GroupParams a, GroupParams b) => a.Equals(b);

		public static bool operator !=(GroupParams a, GroupParams b) => !(a == b);

		#endregion
	}
}