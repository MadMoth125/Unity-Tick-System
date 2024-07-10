using UnityEngine;

namespace TickSystem.Example
{
	[DisallowMultipleComponent]
	public class TimeScaleController : MonoBehaviour
	{
		[Header("Time Controls")]
		[Tooltip("The key to incrementally slow down time.")]
		public KeyCode slowDownKey = KeyCode.LeftArrow;

		[Tooltip("The key to incrementally speed up time.")]
		public KeyCode speedUpKey = KeyCode.RightArrow;

		[Space(10)]
		[Tooltip("The key to pause/unpause time.")]
		public KeyCode pauseKey = KeyCode.Space;

		[Header("Debug")]
		public bool logTimeScale = true;

		private float _timeScale = 1f;
		private bool _timePaused = false;
		private bool _madeInput = false;

		#region Unity Methods

		private void Update()
		{
			// Pause/unpause time.
			if (Input.GetKeyDown(pauseKey))
			{
				_madeInput = true;
				_timePaused = !_timePaused;
				Time.timeScale = _timePaused ? 0f : _timeScale;
			}

			if (!_timePaused)
			{
				// Slow down time.
				if (Input.GetKeyDown(slowDownKey))
				{
					_madeInput = true;
					_timeScale = Mathf.Max(0f, _timeScale - 0.1f);
					Time.timeScale = _timeScale;
				}

				// Speed up time.
				if (Input.GetKeyDown(speedUpKey))
				{
					_madeInput = true;
					_timeScale = Mathf.Min(1f, _timeScale + 0.1f);
					Time.timeScale = _timeScale;
				}
			}

			if (logTimeScale && _madeInput) Debug.Log("Time.timeScale: " + Time.timeScale);
			_madeInput = false;
		}

		#endregion
	}
}