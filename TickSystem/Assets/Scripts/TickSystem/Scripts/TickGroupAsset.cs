using System;
using TickSystem.Core;
using UnityEngine;

namespace TickSystem
{
	[CreateAssetMenu(fileName = "NewTickGroup", menuName = "Tick System/Tick Group")]
	public class TickGroupAsset : ScriptableObject
	{
		#region Properties

		/// <summary>
		/// Whether the tick group is active.
		/// </summary>
		public bool Active
		{
			get => active;
			set
			{
				if (active == value) return;
				active = value;
				GetTickGroup().Active(value);
			}
		}

		/// <summary>
		/// Whether the tick group uses real time.
		/// If false, it uses game time.
		/// </summary>
		public bool UseRealTime
		{
			get => useRealTime;
			set
			{
				if (useRealTime == value) return;
				useRealTime = value;
				GetTickGroup().UseRealTime(value);
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
				GetTickGroup().Interval(1f / value);
			}
		}

		/// <summary>
		/// The interval between ticks. (read-only)
		/// </summary>
		public float Interval => 1f / tickRate;

		#endregion

		[Tooltip("Whether the tick group is active.")]
		[SerializeField]
		private bool active = true;

		[Tooltip("Whether the tick group uses real time.\n" +
		         "If false, it uses game time.")]
		[SerializeField]
		private bool useRealTime = false;

		[Tooltip("The number of ticks per second.")]
		[Range(1, 60)]
		[SerializeField]
		private int tickRate = 20;

		private TickGroup _tickGroup;

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
		public GroupParams GetGroupParams() => new(name, 1f / tickRate, active, useRealTime);

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