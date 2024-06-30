namespace TickSystem
{
	/// <summary>
	/// Extension methods for <see cref="TickGroup"/> for
	/// easier access to <see cref="GroupParams"/> properties.
	/// </summary>
	public static class TickExtensions
	{
		/// <summary>
		/// Gets the name of the TickGroup.
		/// </summary>
		/// <param name="group"></param>
		public static string GetName(this TickGroup group) => group.Parameters.name;

		/// <summary>
		/// Sets the name of the TickGroup.
		/// </summary>
		/// <param name="group"></param>
		/// <param name="name"></param>
		public static void SetName(this TickGroup group, string name) => group.Parameters.name = name;

		/// <summary>
		/// Gets the tick interval of the TickGroup.
		/// </summary>
		/// <param name="group"></param>
		public static float GetInterval(this TickGroup group) => group.Parameters.interval;

		/// <summary>
		/// Sets the tick interval of the TickGroup.
		/// </summary>
		/// <param name="group"></param>
		/// <param name="interval"></param>
		public static void SetInterval(this TickGroup group, float interval) => group.Parameters.interval = interval;

		/// <summary>
		/// Gets whether the TickGroup is enabled.
		/// </summary>
		/// <param name="group"></param>
		public static bool IsEnabled(this TickGroup group) => group.Parameters.enabled;

		/// <summary>
		/// Sets whether the TickGroup is enabled.
		/// </summary>
		/// <param name="group"></param>
		/// <param name="active"></param>
		public static void SetEnabled(this TickGroup group, bool active) => group.Parameters.enabled = active;

		/// <summary>
		/// Gets whether the TickGroup ticks in real time.
		/// </summary>
		/// <param name="group"></param>
		public static bool IsRealTime(this TickGroup group) => group.Parameters.useRealTime;

		/// <summary>
		/// Sets whether the TickGroup ticks in real time.
		/// </summary>
		/// <param name="group"></param>
		/// <param name="useRealTime"></param>
		public static void SetRealTime(this TickGroup group, bool useRealTime) => group.Parameters.useRealTime = useRealTime;

		/// <summary>
		/// Sets the GroupParams of the TickGroup to new values.
		/// </summary>
		/// <param name="group"></param>
		/// <param name="parameters"></param>
		public static void SetParameters(this TickGroup group, in GroupParams parameters) => group.Parameters.Set(parameters);
	}
}