using System;

namespace TickSystem
{
	[Serializable]
	public struct GroupParams
	{
		public string name;
		public float interval;
		public bool enabled;
		public bool realTime;

		/// <summary>
		/// Default parameters for a tick group.
		/// </summary>
		/// <remarks>
		/// name = "TickGroup", interval = 0.05f, enabled = true, realTime = false.
		/// </remarks>
		public static GroupParams Default => new GroupParams("TickGroup", 0.05f, true, false);

		#region Constructors

		public GroupParams(string name, float interval, bool enabled, bool realTime)
		{
			this.name = name;
			this.interval = interval;
			this.enabled = enabled;
			this.realTime = realTime;
		}

		public GroupParams(string name, float interval, bool enabled)
		{
			this.name = name;
			this.interval = interval;
			this.enabled = enabled;
			this.realTime = false;
		}

		public GroupParams(string name, float interval)
		{
			this.name = name;
			this.interval = interval;
			this.enabled = true;
			this.realTime = false;
		}

		#endregion

		public void Set(in GroupParams other)
		{
			this.name = other.name;
			this.interval = other.interval;
			this.enabled = other.enabled;
			this.realTime = other.realTime;
		}

		public void Set(string name = "", float? interval = null, bool? enabled = null, bool? realTime = null)
		{
			if (name != "") this.name = name;
			if (interval != null) this.interval = interval.Value;
			if (enabled != null) this.enabled = enabled.Value;
			if (realTime != null) this.realTime = realTime.Value;
		}

		public readonly void Deconstruct(out string name, out float interval, out bool enabled, out bool realTime)
		{
			name = this.name;
			interval = this.interval;
			enabled = this.enabled;
			realTime = this.realTime;
		}

		#region Overrides

		public override string ToString() => $"[name: {name}, interval: {interval}, enabled: {enabled}, realTime: {realTime}]";

		public override bool Equals(object obj)
		{
			if (obj is GroupParams other)
			{
				return this.name == other.name &&
				       Math.Abs(this.interval - other.interval) < 0.0001f &&
				       this.enabled == other.enabled &&
				       this.realTime == other.realTime;
			}
			return false;
		}

		public override int GetHashCode() => HashCode.Combine(name, interval, enabled, realTime);

		public static bool operator ==(GroupParams a, GroupParams b) => a.Equals(b);

		public static bool operator !=(GroupParams a, GroupParams b) => !(a == b);

		#endregion
	}
}