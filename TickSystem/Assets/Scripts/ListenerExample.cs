using System.Diagnostics;
using TickSystem;
using UnityEngine;

public class ListenerExample : MonoBehaviour
{
	public TickGroupAsset tickGroup;

	public bool logTime = false;

	private float _lastTime;
	private Stopwatch _tickStopwatch;

	#region Unity Methods

	private void OnEnable()
	{
		_tickStopwatch = new Stopwatch();

		if (tickGroup == null) return;
		tickGroup.Add(TestMethod);
	}

	private void OnDisable()
	{
		_tickStopwatch.Stop();

		if (tickGroup == null) return;
		tickGroup.Remove(TestMethod);
	}

	#endregion

	private void TestMethod()
	{
		if (logTime) UnityEngine.Debug.Log($"Tick: <b>{_tickStopwatch.Elapsed.Milliseconds / 1000f}</b> seconds.");
		_tickStopwatch.Restart();
	}
}