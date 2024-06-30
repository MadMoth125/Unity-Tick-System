using System;

namespace TickSystem.Core
{
	[Serializable]
	public struct GroupParams
	{
		public string name;
		public float interval;
		public bool enabled;
		public bool useRealTime;

		/// <summary>
		/// Default parameters for a tick group.
		/// </summary>
		/// <remarks>
		/// name = "TickGroup", interval = 0.05f, enabled = true, useRealTime = false.
		/// </remarks>
		public static GroupParams Default => new GroupParams("TickGroup", 0.05f, true, false);

		#region Constructors

		public GroupParams(string name, float interval, bool enabled, bool useRealTime)
		{
			this.name = name;
			this.interval = interval;
			this.enabled = enabled;
			this.useRealTime = useRealTime;
		}

		public GroupParams(string name, float interval, bool enabled)
		{
			this.name = name;
			this.interval = interval;
			this.enabled = enabled;
			this.useRealTime = false;
		}

		public GroupParams(string name, float interval)
		{
			this.name = name;
			this.interval = interval;
			this.enabled = true;
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
			this.enabled = other.enabled;
			this.useRealTime = other.useRealTime;
		}

		/// <summary>
		/// Sets the parameters of the TickGroup.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="interval"></param>
		/// <param name="enabled"></param>
		/// <param name="useRealTime"></param>
		public void Set(string name = "", float? interval = null, bool? enabled = null, bool? useRealTime = null)
		{
			if (name != "") this.name = name;
			if (interval != null) this.interval = interval.Value;
			if (enabled != null) this.enabled = enabled.Value;
			if (useRealTime != null) this.useRealTime = useRealTime.Value;
		}

		#region Overrides

		public override string ToString() => $"[name: {name}, interval: {interval}, enabled: {enabled}, useRealTime: {useRealTime}]";

		public override bool Equals(object obj)
		{
			if (obj is GroupParams other)
			{
				return this.name == other.name &&
				       Math.Abs(this.interval - other.interval) < 0.0001f &&
				       this.enabled == other.enabled &&
				       this.useRealTime == other.useRealTime;
			}
			return false;
		}

		public override int GetHashCode() => HashCode.Combine(name, interval, enabled, useRealTime);

		public static bool operator ==(GroupParams a, GroupParams b) => a.Equals(b);

		public static bool operator !=(GroupParams a, GroupParams b) => !(a == b);

		#endregion
	}
}