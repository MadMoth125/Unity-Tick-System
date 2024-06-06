namespace TickSystem.Core
{
	public static class TickExtensions
	{
		public static string Name(this TickGroup group) => group.parameters.name;

		public static void Name(this TickGroup group, string name) => group.parameters.name = name;

		public static float Interval(this TickGroup group) => group.parameters.interval;

		public static void Interval(this TickGroup group, float interval) => group.parameters.interval = interval;

		public static bool Active(this TickGroup group) => group.parameters.active;

		public static void Active(this TickGroup group, bool active) => group.parameters.active = active;

		public static bool UseRealTime(this TickGroup group) => group.parameters.useRealTime;

		public static void UseRealTime(this TickGroup group, bool useRealTime) => group.parameters.useRealTime = useRealTime;

		public static void SetParameters(this TickGroup group, GroupParams parameters) => group.parameters.Set(parameters);
	}
}