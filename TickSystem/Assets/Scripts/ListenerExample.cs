using TickSystem;
using UnityEngine;

public class ListenerExample : MonoBehaviour
{
	public TickGroupAsset tickGroup;
	
	private float _lastTime;
	
	#region Unity Methods

	private void OnEnable()
	{
		if (tickGroup == null) return;
		tickGroup.Add(TestMethod);
	}

	private void OnDisable()
	{
		if (tickGroup == null) return;
		tickGroup.Remove(TestMethod);
	}

	#endregion
	
	private void TestMethod()
	{
		Debug.Log($"<b>{(Time.time - _lastTime)}</b> seconds since last tick.");
		_lastTime = Time.time;
	}
}