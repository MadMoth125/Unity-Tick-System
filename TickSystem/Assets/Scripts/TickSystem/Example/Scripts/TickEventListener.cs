using UnityEngine;

namespace TickSystem.Example
{
	public class TickEventListener : MonoBehaviour
	{
		#region Unity Methods

		private void OnEnable()
		{
			TickManager.OnTickGroupAdded += TickGroupAdded;
			TickManager.OnTickGroupRemoved += TickGroupRemoved;
		}

		private void OnDisable()
		{
			TickManager.OnTickGroupAdded -= TickGroupAdded;
			TickManager.OnTickGroupRemoved -= TickGroupRemoved;
		}

		#endregion

		private void TickGroupAdded(TickGroup tickGroup)
		{
			Debug.Log($"{nameof(TickManager.OnTickGroupAdded)}: Group '{tickGroup.Name}' Added.");
		}

		private void TickGroupRemoved(TickGroup tickGroup)
		{
			Debug.Log($"{nameof(TickManager.OnTickGroupRemoved)}: Group '{tickGroup.Name}' Removed.");
		}
	}
}