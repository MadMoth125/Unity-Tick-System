using UnityEngine;

namespace TickSystem.Example
{
	[DisallowMultipleComponent]
	public class TickRotator : MonoBehaviour
	{
		[Header("Required")]
		public TickGroupAsset tickGroup;

		[Header("Parameters")]
		public Vector3 rotationIncrement = Vector3.up;

		#region Unity Methods

		private void OnEnable()
		{
			// Add the Rotate method to the tick group.
			tickGroup.OnTick += Rotate;
		}

		private void OnDisable()
		{
			// Remove the Rotate method from the tick group.
			tickGroup.OnTick -= Rotate;
		}

		#endregion

		/// <summary>
		/// Rotates the transform by the rotation increment.
		/// </summary>
		private void Rotate() => transform.Rotate(rotationIncrement);
	}
}