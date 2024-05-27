using TickSystem.Core;
using UnityEngine;

namespace TickSystem
{
	[CreateAssetMenu(fileName = "NewTickGroup", menuName = "Tick System/Tick Group")]
	public class TickGroupAsset : ScriptableObject
	{
		[Tooltip("Whether the tick group is active.")]
		public bool active = true;

		[Tooltip("Whether the tick group uses real time.\n" +
		         "If false, it uses game time.")]
		public bool useRealTime = false;

		[Tooltip("The number of ticks per second.")]
		[Range(1, 60)]
		public int tickRate = 20;
		
		private TickGroup _tickGroup;
		
		public GroupParams GetGroupParams()
		{
			return new GroupParams(name, 1f / tickRate, active, useRealTime);
		}
		
		public TickGroup GetTickGroup()
		{
			if (_tickGroup == null)
			{
				_tickGroup = new TickGroup(GetGroupParams());
				return _tickGroup;
			}
			
			_tickGroup.Parameters.CopyFrom(GetGroupParams());
			return _tickGroup;
		}

		private void OnDisable()
		{
			_tickGroup?.Clear();
		}
	}
}