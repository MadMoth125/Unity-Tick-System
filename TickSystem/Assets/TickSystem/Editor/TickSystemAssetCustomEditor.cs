using UnityEditor;
using UnityEngine;

namespace TickSystem.Editor
{
	internal static class TickSystemAssetCustomEditor
	{
		private const string CommandPath = "CONTEXT/TickGroupAsset/Set Tick Rate/";
		private const string TickRatePostFix = " Tick \u2215 s"; // using unicode char for '/'

		/// <summary>
		/// Sets the interval of the TickGroupAsset to match the given tick rate.
		/// </summary>
		/// <param name="tickGroup"></param>
		/// <param name="tickRate"></param>
		/// <remarks>
		/// The calculated interval will only support tick rates smaller than 60.
		/// Any tick rate values greater than that amount will be clamped to 60 to prevent miniscule float values.
		/// </remarks>
		private static void SetIntervalToTickRate(TickGroupAsset tickGroup, int tickRate)
		{
			tickGroup.Interval = tickRate == 0 ? 0 : 1f / Mathf.Min(tickRate, 60);
		}

		/// <summary>
		/// Casts a MenuCommand instance to a TickGroupAsset reference.
		/// </summary>
		/// <param name="menuCommand"></param>
		/// <returns>A TickGroupAsset reference.</returns>
		private static TickGroupAsset AsTickGroupAsset(this MenuCommand menuCommand)
		{
			return menuCommand.context as TickGroupAsset;
		}

		#region MenuItem Methods

		[MenuItem(CommandPath + "5" + TickRatePostFix, priority = 0)]
		private static void SetTickRateFive(MenuCommand command) =>
			SetIntervalToTickRate(command.AsTickGroupAsset(), 5);

		[MenuItem(CommandPath + "10" + TickRatePostFix, priority = 10)]
		private static void SetTickRateTen(MenuCommand command) =>
			SetIntervalToTickRate(command.AsTickGroupAsset(), 10);

		[MenuItem(CommandPath + "15" + TickRatePostFix, priority = 20)]
		private static void SetTickRateFifteen(MenuCommand command) =>
			SetIntervalToTickRate(command.AsTickGroupAsset(), 15);

		[MenuItem(CommandPath + "20" + TickRatePostFix, priority = 30)]
		private static void SetTickRateTwenty(MenuCommand command) =>
			SetIntervalToTickRate(command.AsTickGroupAsset(), 20);

		[MenuItem(CommandPath + "25" + TickRatePostFix, priority = 40)]
		private static void SetTickRateTwentyFive(MenuCommand command) =>
			SetIntervalToTickRate(command.AsTickGroupAsset(), 25);

		[MenuItem(CommandPath + "30" + TickRatePostFix, priority = 50)]
		private static void SetTickRateThirty(MenuCommand command) =>
			SetIntervalToTickRate(command.AsTickGroupAsset(), 30);

		[MenuItem(CommandPath + "35" + TickRatePostFix, priority = 60)]
		private static void SetTickRateThirtyFive(MenuCommand command) =>
			SetIntervalToTickRate(command.AsTickGroupAsset(), 35);

		[MenuItem(CommandPath + "40" + TickRatePostFix, priority = 70)]
		private static void SetTickRateForty(MenuCommand command) =>
			SetIntervalToTickRate(command.AsTickGroupAsset(), 40);

		[MenuItem(CommandPath + "45" + TickRatePostFix, priority = 80)]
		private static void SetTickRateFortyFive(MenuCommand command) =>
			SetIntervalToTickRate(command.AsTickGroupAsset(), 45);

		[MenuItem(CommandPath + "50" + TickRatePostFix, priority = 90)]
		private static void SetTickRateFifty(MenuCommand command) =>
			SetIntervalToTickRate(command.AsTickGroupAsset(), 50);

		[MenuItem(CommandPath + "55" + TickRatePostFix, priority = 100)]
		private static void SetTickRateFiftyFive(MenuCommand command) =>
			SetIntervalToTickRate(command.AsTickGroupAsset(), 55);

		[MenuItem(CommandPath + "60" + TickRatePostFix, priority = 110)]
		private static void SetTickRateSixty(MenuCommand command) =>
			SetIntervalToTickRate(command.AsTickGroupAsset(), 60);

		#endregion
	}
}