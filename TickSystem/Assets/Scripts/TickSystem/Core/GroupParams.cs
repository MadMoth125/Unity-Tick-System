
namespace TickSystem.Core
{
	public struct GroupParams
	{
		public string name;
		public int tickRate;
		public bool active;
		public bool useRealTime;
		
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
		
		public void CopyFrom(GroupParams other)
		{
			name = other.name;
			tickRate = other.tickRate;
			active = other.active;
			useRealTime = other.useRealTime;
		}
	}
}