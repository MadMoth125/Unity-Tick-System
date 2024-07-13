using System;
using System.Collections.Generic;

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
			return new GroupParams(tickGroup.Name, tickGroup.Interval, tickGroup.Enabled, tickGroup.RealTime);
		}

		/// <summary>
		/// Gets the values from the TickGroupAsset and converts them to GroupParams.
		/// </summary>
		/// <param name="tickGroup"></param>
		public static GroupParams GetParameters(this TickGroupAsset tickGroup)
		{
			return new GroupParams(tickGroup.Name, tickGroup.Interval, tickGroup.Enabled, tickGroup.RealTime);
		}

		/// <summary>
		/// Sets the TickGroup parameters to the provided ones.
		/// </summary>
		/// <param name="tickGroup"></param>
		/// <param name="name"></param>
		/// <param name="interval"></param>
		/// <param name="enabled"></param>
		/// <param name="realTime"></param>
		public static void SetParameters(this TickGroup tickGroup, string name = null, float? interval = null,
		                                 bool? enabled = null, bool? realTime = null)
		{
			if (name != null) tickGroup.Name = name;
			if (interval != null) tickGroup.Interval = interval.Value;
			if (enabled != null) tickGroup.Enabled = enabled.Value;
			if (realTime != null) tickGroup.RealTime = realTime.Value;
		}

		/// <summary>
		/// Sets the TickGroup parameters to the provided GroupParams.
		/// </summary>
		/// <param name="tickGroup"></param>
		/// <param name="parameters"></param>
		public static void SetParameters(this TickGroup tickGroup, in GroupParams parameters)
		{
			parameters.Deconstruct(out tickGroup.Name, out tickGroup.Interval, out tickGroup.Enabled, out tickGroup.RealTime);
		}

		/// <summary>
		/// Sets the TickGroupAsset parameters to the provided ones.
		/// </summary>
		/// <param name="tickGroup"></param>
		/// <param name="tickRate"></param>
		/// <param name="enabled"></param>
		/// <param name="realTime"></param>
		public static void SetParameters(this TickGroupAsset tickGroup, int? tickRate = null,
		                                 bool? enabled = null, bool? realTime = null)
		{
			if (tickRate != null) tickGroup.TickRate = tickRate.Value;
			if (enabled != null) tickGroup.Enabled = enabled.Value;
			if (realTime != null) tickGroup.RealTime = realTime.Value;
		}

		/// <summary>
		/// Checks if the TickGroup name matches the provided name.
		/// </summary>
		/// <param name="group"></param>
		/// <param name="name"></param>
		/// <returns>Whether the names are equal. (Excluding whitespace)</returns>
		public static bool CompareName(this TickGroup group, string name)
		{
			return group != null && group.Name.Trim() == name.Trim();
		}

		/// <summary>
		/// Checks if the TickGroupAsset name matches the provided name.
		/// </summary>
		/// <param name="group"></param>
		/// <param name="name"></param>
		/// <returns>Whether the names are equal. (Excluding whitespace)</returns>
		public static bool CompareName(this TickGroupAsset group, string name)
		{
			return group != null && group.Name.Trim() == name.Trim();
		}

		/// <summary>
		/// Invoke a set of callbacks in order.
		/// </summary>
		/// <param name="callbacks"></param>
		public static void InvokeCallbacks(this IList<Action> callbacks)
		{
			if (callbacks.Count == 0) return;
			for (int i = 0; i < callbacks.Count; i++)
			{
				callbacks[i]?.Invoke();
			}
		}

		/// <summary>
		/// Invoke a set of callbacks in reverse order
		/// </summary>
		/// <param name="callbacks"></param>
		public static void InvokeCallbacksReverse(this IList<Action> callbacks)
		{
			if (callbacks.Count == 0) return;
			for (int i = callbacks.Count - 1; i >= 0; i--)
			{
				callbacks[i]?.Invoke();
			}
		}
	}
}