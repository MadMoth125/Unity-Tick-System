using System;
using UnityEngine;

namespace TickSystem
{
	/// <summary>
	/// Wraps the base TickGroup to integrate the creation/modification of them using the Unity inspector.
	/// </summary>
	[CreateAssetMenu(fileName = "NewTickGroup", menuName = "Tick System/Tick Group")]
	public class TickGroupAsset : ScriptableObject
	{
		#region Properties

		/// <summary>
		/// Whether the TickGroup is active.
		/// </summary>
		public bool Enabled
		{
			get => enabled;
			set
			{
				if (enabled == value) return;
				enabled = value;
				GetTickGroup().Enabled = value;
			}
		}

		/// <summary>
		/// Whether the TickGroup uses real time.
		/// If false, it uses game time.
		/// </summary>
		public bool IsRealTime
		{
			get => isRealTime;
			set
			{
				if (isRealTime == value) return;
				isRealTime = value;
				GetTickGroup().IsRealTime = value;
			}
		}

		/// <summary>
		/// The number of ticks per second.
		/// </summary>
		public int TickRate
		{
			get => tickRate;
			set
			{
				if (tickRate == value) return;
				tickRate = value;
				GetTickGroup().Interval = 1f / value;
			}
		}

		/// <summary>
		/// The interval between ticks.
		/// </summary>
		public float Interval => 1f / tickRate;

		#endregion

		[Tooltip("Whether the tick group is active.")]
		[SerializeField]
		private bool enabled = true;

		[Tooltip("Whether the tick group functions in real time.\n" +
		         "If false, it uses game time.")]
		[SerializeField]
		private bool isRealTime = false;

		[Tooltip("The number of ticks per second.")]
		[Range(1, 60)]
		[SerializeField]
		private int tickRate = 20;

		private TickGroup _tickGroup;
		private GroupParams _groupParams;

		/// <summary>
		/// Adds a callback to the group.
		/// </summary>
		/// <param name="callback">The callback to add.</param>
		public void Add(Action callback) => GetTickGroup().Add(callback);

		/// <summary>
		/// Removes a callback from the group.
		/// </summary>
		/// <param name="callback">The callback to remove.</param>
		public void Remove(Action callback) => GetTickGroup().Remove(callback);

		/// <summary>
		/// Returns the parameters for the group asset.
		/// </summary>
		public GroupParams GetGroupParams()
		{
			_groupParams.Set(name, Interval, Enabled, IsRealTime);
			return _groupParams;
		}

		/// <summary>
		/// Returns the tick group associated with this asset.
		/// </summary>
		public TickGroup GetTickGroup() => _tickGroup ??= new TickGroup(GetGroupParams());

		#region Unity Methods

		#if UNITY_EDITOR
		private void OnValidate()
		{
			if (!Application.isPlaying || _tickGroup == null) return;
			GetTickGroup().SetParameters(GetGroupParams());
		}
		#endif

		private void OnEnable() => _tickGroup = new TickGroup(GetGroupParams());

		private void OnDisable() => _tickGroup?.Dispose();

		#endregion
	}
}