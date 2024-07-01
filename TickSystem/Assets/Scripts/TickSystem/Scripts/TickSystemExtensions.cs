namespace TickSystem
{
	public static class TickSystemExtensions
	{
		/// <summary>
		/// Gets the values from the TickGroup and converts them to GroupParams.
		/// </summary>
		/// <param name="tickGroup"></param>
		public static GroupParams GetParameters(this TickGroup tickGroup)
		{
			return new GroupParams(tickGroup.Name, tickGroup.Interval, tickGroup.Enabled, tickGroup.IsRealTime);
		}

		/// <summary>
		/// Gets the values from the TickGroupAsset and converts them to GroupParams.
		/// </summary>
		/// <param name="tickGroup"></param>
		public static GroupParams GetParameters(this TickGroupAsset tickGroup)
		{
			return new GroupParams(tickGroup.name, tickGroup.Interval, tickGroup.Active, tickGroup.IsRealTime);
		}

		/// <summary>
		/// Sets the TickGroup to copy the values found in the GroupParams.
		/// </summary>
		/// <param name="tickGroup"></param>
		/// <param name="parameters"></param>
		public static void SetParameters(this TickGroup tickGroup, in GroupParams parameters)
		{
			parameters.Deconstruct(out tickGroup.Name, out tickGroup.Interval, out tickGroup.Enabled, out tickGroup.IsRealTime);
		}
	}
}