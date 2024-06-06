using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TickSystem.Core;
using UnityEngine;
using UnityEngine.TestTools;

internal class TickSystemTests
{
	/*
	 * A test that checks if a TickGroup is properly added to TickManager.TickGroupInstances when instantiated.
	 *
	 * If this is failing, ensure that:
	 * - TickGroup is being registered with TickManager.RegisterTickGroup in its constructor.
	 * - TickManager is adding the TickGroup to its list of registered instances.
	 */
	[Test]
	public void TickGroup_Instantiated_AddedToManager()
	{
		var tickGroup = new TickGroup();

		bool registered = TickManager.TickGroupInstances.Contains(tickGroup);

		tickGroup.Dispose();

		Assert.IsTrue(registered, "TickGroup couldn't be found in the TickManager's registered instances.");
	}

	/*
	 * A test that checks if a TickGroup is properly removed from TickManager.TickGroupInstances when disposed.
	 *
	 * If this is failing, ensure that:
	 * - TickGroup is being unregistered with TickManager.UnregisterTickGroup in its Dispose method.
	 * - TickManager is removing the TickGroup from its list of registered instances.
	 */
	[Test]
	public void TickGroup_Disposed_RemovedFromManager()
	{
		var tickGroup = new TickGroup();

		tickGroup.Dispose();

		bool registered = TickManager.TickGroupInstances.Contains(tickGroup);

		Assert.IsFalse(registered, "TickGroup was found in the TickManager's registered instances after being disposed.");
	}

	/*
	 * A test that checks if a TickGroup is properly initialized with the provided parameters.
	 *
	 * If this is failing, ensure that:
	 * - GroupParams is being initialized with the provided parameters in its constructor.
	 * - TickGroup is being initialized with the provided parameters in its constructor.
	 * - The GroupParams equality operator is properly implemented.
	 */
	[Test]
	public void TickGroup_InitializedWithParams_HasSameParams()
	{
		var paramsA = new GroupParams("Test_TickGroup", 1f, true, true);

		var tickGroup = new TickGroup(paramsA);

		var paramsB = tickGroup.parameters;

		bool hasParams = paramsA == paramsB;

		tickGroup.Dispose();

		Assert.IsTrue(hasParams, "TickGroup was not initialized with the provided parameters:\n" +
		                         DisplayGroupParamStrings(paramsA, paramsB));
	}

	/*
	 * A test that checks if a TickGroup is properly initialized with the default parameters.
	 *
	 * If this is failing, ensure that:
	 * - TickGroup is being initialized with the default parameters in its parameterless constructor.
	 * - The GroupParams equality operator is properly implemented.
	 */
	[Test]
	public void TickGroup_InitializedWithNoParams_HasDefaultParams()
	{
		var tickGroup = new TickGroup();

		var paramsA = GroupParams.Default;

		var paramsB = tickGroup.parameters;

		bool hasParams = paramsA == paramsB;

		tickGroup.Dispose();

		Assert.IsTrue(hasParams, "TickGroup was not initialized with the default parameters:\n" +
		                         DisplayGroupParamStrings(paramsA, paramsB));
	}

	// A Test behaves as an ordinary method
	[UnityTest]
	public IEnumerator TickGroup_ManagerDisabled_PreventTick()
	{
		// save the previous state of the TickManager
		bool prevManagerState = TickManager.Active;

		int tickCount = 0; // should remain 0

		// disable the TickManager
		TickManager.Active = false;

		// create a new TickGroup
		var group = new TickGroup(GroupParams.Default);

		// add a listener method to the group
		group.Add(TestListenerMethod);

		// wait for a short time (enough time for the group to tick)
		yield return new WaitForSeconds(group.parameters.interval);

		// dispose of the group
		group.Dispose();

		// reset the TickManager to its previous state
		TickManager.Active = prevManagerState;

		// check if the listener method has been called
		Assert.IsTrue(tickCount == 0, "TickManager.Active = false did not prevent TickGroup from ticking.");

		yield break;

		void TestListenerMethod() => tickCount++;
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
}