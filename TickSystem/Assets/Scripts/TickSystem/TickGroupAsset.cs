using System;
using TickSystem.Core;
using UnityEngine;

namespace TickSystem
{
	[CreateAssetMenu(fileName = "NewTickGroup", menuName = "Tick System/Tick Group")]
	public class TickGroupAsset : ScriptableObject
	{
		public bool Active
		{
			get => active;
			set
			{
				if (active == value) return;
				active = value;
				UpdateParameters();
			}
		}
		
		public bool UseRealTime
		{
			get => useRealTime;
			set
			{
				if (useRealTime == value) return;
				useRealTime = value;
				UpdateParameters();
			}
		}
		
		public int TickRate
		{
			get => tickRate;
			set
			{
				if (tickRate == value) return;
				tickRate = value;
				UpdateParameters();
			}
		}

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
		
		private GroupParams _groupParams;
		private TickGroup _tickGroup;
		
		public void Add(Action callback) => GetTickGroup().Add(callback);

		public void Remove(Action callback) => GetTickGroup().Remove(callback);
		
		public GroupParams GetGroupParams()
		{
			return _groupParams.Set(name, 1f / tickRate, active, useRealTime);
		}
		
		public TickGroup GetTickGroup()
		{
			if (_tickGroup == null)
			{
				_tickGroup = new TickGroup(GetGroupParams());
				return _tickGroup;
			}
			
			_tickGroup.parameters = GetGroupParams();
			return _tickGroup;
		}

		private void UpdateParameters()
		{
			if (!Application.isPlaying) return;
			if (_tickGroup == null) return;
			_tickGroup.parameters = GetGroupParams();
		}

		#region Unity Methods

		private void OnValidate()
		{
			UpdateParameters();
		}

		private void OnDisable()
		{
			_tickGroup?.Clear();
		}

		#endregion
	}
}