using System;
using System.Collections;
using NUnit.Framework;
using TickSystem;
using TickSystem.Core;
using UnityEngine;
using UnityEngine.TestTools;

internal class TickSystemTests
{
	[Test]
	public void TickGroup_Instantiated_AddedToManager()
	{
		var tickGroup = new TickGroup();

		bool registered = TickManager_New.Contains(tickGroup);

		tickGroup.Dispose();

		Assert.IsTrue(registered, $"{nameof(TickGroup)} couldn't be found in the {nameof(TickManager_New)}'s registered instances.");
	}

	[Test]
	public void TickGroup_Disposed_RemovedFromManager()
	{
		var tickGroup = new TickGroup();

		tickGroup.Dispose();

		bool registered = TickManager_New.Contains(tickGroup);

		Assert.IsFalse(registered, $"{nameof(TickGroup)} was found in the {nameof(TickManager_New)}'s registered instances after being disposed.");
	}

	[Test]
	public void TickGroup_InitializedWithParams_HasSameParams()
	{
		var paramsA = new GroupParams("Test_TickGroup", 1f, true, true);

		var tickGroup = new TickGroup(paramsA);

		var paramsB = tickGroup.Parameters;

		bool hasParams = paramsA == paramsB;

		tickGroup.Dispose();

		Assert.IsTrue(hasParams, $"{nameof(TickGroup)} was not initialized with the provided parameters:\n" +
		                         DisplayGroupParamStrings(paramsA, paramsB));
	}

	[Test]
	public void TickGroup_InitializedWithNoParams_HasDefaultParams()
	{
		var tickGroup = new TickGroup();

		var paramsA = GroupParams.Default;

		var paramsB = tickGroup.Parameters;

		bool hasParams = paramsA == paramsB;

		tickGroup.Dispose();

		Assert.IsTrue(hasParams, $"{nameof(TickGroup)} was not initialized with the default parameters:\n" +
		                         DisplayGroupParamStrings(paramsA, paramsB));
	}

	[UnityTest]
	public IEnumerator TickGroup_AddListenerMethod_CallsMethod()
	{
		// save the previous state of the TickManager_New
		bool prevManagerState = TickManager_New.Enabled;

		// declare a variable to count the number of times the listener method is called
		int tickCount = 0; // should be 1 at end of test

		// enable the TickManager_New
		TickManager_New.Enabled = true;

		// create a new TickGroup
		var group = new TickGroup(GroupParams.Default);

		// add a listener method to the group
		group.Add(TestListenerMethod);

		// wait for a short time (enough time for the group to tick)
		yield return new WaitForSeconds(GetForgivingInterval(group.Parameters));

		// dispose of the group
		group.Dispose();
		TickManager_New.Enabled = prevManagerState;

		// check if the listener method has been called
		Assert.IsTrue(tickCount > 0, $"{nameof(TickGroup)} did not call the listener method.");
		Assert.IsFalse(tickCount > 1, $"{nameof(TickGroup)} called the listener method more than once.");

		yield break;

		void TestListenerMethod() => tickCount++;
	}

	[UnityTest]
	public IEnumerator TickManager_New_Disabled_PreventTick()
	{
		// save the previous state of the TickManager_New
		bool prevManagerState = TickManager_New.Enabled;

		int tickCount = 0; // should remain 0

		// disable the TickManager_New
		TickManager_New.Enabled = false;

		// create a new TickGroup
		var group = new TickGroup(GroupParams.Default);

		// add a listener method to the group
		group.Add(TestListenerMethod);

		// wait for a short time (enough time for the group to tick)
		yield return new WaitForSeconds(GetForgivingInterval(group.Parameters));

		// dispose of the group
		group.Dispose();

		// reset the TickManager_New to its previous state
		TickManager_New.Enabled = prevManagerState;

		// check if the listener method has been called
		Assert.IsTrue(tickCount < 1, $"'{nameof(TickManager_New)}.{nameof(TickManager_New.Enabled)} = false' did not prevent {nameof(TickGroup)} from ticking.");

		yield break;

		void TestListenerMethod() => tickCount++;
	}

	[Test]
	public void TickManager_New_FindTickGroup_ReturnsTickGroup()
	{
		GroupParams groupParams = new GroupParams("TestGroup", 0.1f, true, true);

		var tickGroup = new TickGroup(groupParams);

		var foundGroup = TickManager_New.Find("TestGroup");

		Assert.AreEqual(tickGroup, foundGroup, $"{nameof(TickManager_New)} did not return the correct {nameof(TickGroup)}.");

		tickGroup.Dispose();
		foundGroup.Dispose();
	}

	/// <summary>
	/// Returns a string displaying the parameters of two GroupParams instances, each parameter on a new line.
	/// </summary>
	private static string DisplayGroupParamStrings(GroupParams a, GroupParams b)
	{
		return $"A.name - {a.name}\n" +
		       $"B.name - {b.name}\n" +
		       $"A.interval - {a.interval}\n" +
		       $"B.interval - {b.interval}\n" +
		       $"A.active - {a.active}\n" +
		       $"B.active - {b.active}\n" +
		       $"A.useRealTime - {a.useRealTime}\n" +
		       $"B.useRealTime - {b.useRealTime}";
	}

	/// <summary>
	/// Returns a tick interval that has a small buffer,
	/// allowing yields to give enough time for systems to initialize and ticks to occur.
	/// </summary>
	private static float GetForgivingInterval(GroupParams groupParams)
	{
		// return the interval plus a small buffer
		// (has minimum delay of 0.01f to prevent useless delay values)
		return groupParams.interval + Math.Max(groupParams.interval * 0.1f, 0.01f);
	}
}