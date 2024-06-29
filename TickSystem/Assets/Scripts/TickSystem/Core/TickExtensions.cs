namespace TickSystem.Core
{
	public static class TickExtensions
	{
		/// <summary>
		/// Gets the name of the TickGroup.
		/// </summary>
		/// <param name="group"></param>
		public static string Name(this TickGroup group) => group.Parameters.name;

		/// <summary>
		/// Sets the name of the TickGroup.
		/// </summary>
		/// <param name="group"></param>
		/// <param name="name"></param>
		public static void Name(this TickGroup group, string name) => group.Parameters.name = name;

		/// <summary>
		/// Gets the tick interval of the TickGroup
		/// </summary>
		/// <param name="group"></param>
		public static float Interval(this TickGroup group) => group.Parameters.interval;

		/// <summary>
		/// Sets the tick interval of the TickGroup
		/// </summary>
		/// <param name="group"></param>
		/// <param name="interval"></param>
		public static void Interval(this TickGroup group, float interval) => group.Parameters.interval = interval;

		/// <summary>
		/// Gets whether the TickGroup is active and can tick
		/// </summary>
		/// <param name="group"></param>
		public static bool Active(this TickGroup group) => group.Parameters.active;

		/// <summary>
		/// Sets whether the TickGroup is active and can tick
		/// </summary>
		/// <param name="group"></param>
		/// <param name="active"></param>
		public static void Active(this TickGroup group, bool active) => group.Parameters.active = active;

		/// <summary>
		/// Gets whether the TickGroup ticks in real time
		/// </summary>
		/// <param name="group"></param>
		public static bool UseRealTime(this TickGroup group) => group.Parameters.useRealTime;

		/// <summary>
		/// Sets whether the TickGroup ticks in real time
		/// </summary>
		/// <param name="group"></param>
		/// <param name="useRealTime"></param>
		public static void UseRealTime(this TickGroup group, bool useRealTime) => group.Parameters.useRealTime = useRealTime;

		/// <summary>
		/// Sets the GroupParams of the TickGroup to new values
		/// </summary>
		/// <param name="group"></param>
		/// <param name="parameters"></param>
		public static void SetParameters(this TickGroup group, in GroupParams parameters) => group.Parameters.Set(parameters);
	}
}