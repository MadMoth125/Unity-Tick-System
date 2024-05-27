using TickSystem.Core;
using UnityEngine;

namespace TickSystem
{
	[CreateAssetMenu(fileName = "NewTickGroup", menuName = "Tick System/Tick Group")]
	public class TickGroupAsset : ScriptableObject
	{
		[SerializeField]
		public bool active = true;

		[SerializeField]
		public bool useRealTime = false;

		[Range(1, 60)]
		[SerializeField]
		public int tickRate = 20;
		
		private TickGroup _tickGroup;
		
		public GroupParams GetGroupParams()
		{
			return new GroupParams(name, tickRate, active, useRealTime);
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
	}
}