using System;
using UnityEngine;

namespace TickSystem
{
	/// <summary>
	/// ScriptableObject container to integrate the
	/// creation/modification of TickGroups using the Unity inspector.
	/// </summary>
	[CreateAssetMenu(fileName = "NewTickGroup", menuName = "Tick System/TickGroup")]
	public class TickGroupAsset : ScriptableObject
	{
		#region Properties

		/// <summary>
		/// Whether the TickGroup is enabled.
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
		public bool RealTime
		{
			get => realTime;
			set
			{
				if (realTime == value) return;
				realTime = value;
				GetTickGroup().RealTime = value;
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

		[Tooltip("Whether the tick group is enabled.")]
		[SerializeField]
		private bool enabled = true;

		[Tooltip("Whether the tick group functions in real time.\n" +
		         "If false, it uses game time.")]
		[SerializeField]
		private bool realTime = false;

		[Tooltip("The number of ticks per second.")]
		[Range(1, 60)]
		[SerializeField]
		private int tickRate = 20;

		private TickGroup _tickGroup;
		private GroupParams _groupParams;

		/// <summary>
		/// Add a callback to the TickGroup.
		/// </summary>
		/// <param name="callback"></param>
		public void Add(Action callback) => GetTickGroup().Add(callback);

		/// <summary>
		/// Remove a callback from the TickGroup.
		/// </summary>
		/// <param name="callback"></param>
		public void Remove(Action callback) => GetTickGroup().Remove(callback);

		/// <summary>
		/// Returns the GroupParams for the TickGroupAsset.
		/// </summary>
		public GroupParams GetGroupParams()
		{
			_groupParams.Set(name, Interval, Enabled, RealTime);
			return _groupParams;
		}

		/// <summary>
		/// Returns the TickGroup for the TickGroupAsset.
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