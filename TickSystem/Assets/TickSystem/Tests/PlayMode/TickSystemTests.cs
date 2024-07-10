using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace TickSystem
{
	internal class TickSystemTests
	{
		[Test]
		public void TickGroup_Instantiated_AddedToManager()
		{
			// Arrange
			TickGroup tickGroup = new TickGroup();

			// Act
			bool registered = TickManager.Contains(tickGroup);
			tickGroup.Dispose();

			// Assert
			Assert.IsTrue(registered, $"{nameof(TickGroup)} couldn't be found in the {nameof(TickManager)}'s registered instances.");
		}

		[Test]
		public void TickGroup_Disposed_RemovedFromManager()
		{
			// Arrange
			TickGroup tickGroup = new TickGroup();
			tickGroup.Dispose();

			// Act
			bool registered = TickManager.Contains(tickGroup);

			// Assert
			Assert.IsFalse(registered, $"{nameof(TickGroup)} was found in the {nameof(TickManager)}'s registered instances after being disposed.");
		}

		[Test]
		public void TickGroup_InitializedWithParams_HasSameParams()
		{
			// Arrange
			GroupParams paramsA = new GroupParams("Test_TickGroup", 1f, true, true);
			TickGroup tickGroup = new TickGroup(paramsA);

			// Act
			GroupParams paramsB = tickGroup.GetParameters();
			bool hasParams = paramsA == paramsB;
			tickGroup.Dispose();

			// Assert
			Assert.IsTrue(hasParams, $"{nameof(TickGroup)} was not initialized with the provided parameters:\n" +
			                         DisplayGroupParamStrings(paramsA, paramsB));
		}

		[Test]
		public void TickGroup_InitializedWithNoParams_HasDefaultParams()
		{
			// Arrange
			TickGroup tickGroup = new TickGroup();
			GroupParams paramsA = GroupParams.Default;
			GroupParams paramsB = tickGroup.GetParameters();

			// Act
			bool hasParams = paramsA == paramsB;
			tickGroup.Dispose();

			// Assert
			Assert.IsTrue(hasParams, $"{nameof(TickGroup)} was not initialized with the default parameters:\n" +
			                         DisplayGroupParamStrings(paramsA, paramsB));
		}

		[UnityTest]
		public IEnumerator TickGroup_AddCallback_InvokeCallback()
		{
			// Arrange
			bool prevManagerState = TickManager.Enabled;
			TickManager.Enabled = true;
			int tickCount = 0;
			TickGroup group = new TickGroup(GroupParams.Default);
			group.Add(TestListenerMethod);

			// Act
			yield return new WaitForSeconds(GetForgivingInterval(group.GetParameters()));
			group.Dispose();
			TickManager.Enabled = prevManagerState;

			// Assert
			Assert.IsTrue(tickCount > 0, $"{nameof(TickGroup)} did not call the listener method.");
			Assert.IsFalse(tickCount > 1, $"{nameof(TickGroup)} called the listener method more than once.");

			yield break;

			void TestListenerMethod() => tickCount++;
		}

		[UnityTest]
		public IEnumerator TickManager_Disabled_PreventTick()
		{
			// Arrange
			bool prevManagerState = TickManager.Enabled;
			TickManager.Enabled = false;
			int tickCount = 0;
			TickGroup group = new TickGroup(GroupParams.Default);
			group.Add(TestListenerMethod);

			// Act
			yield return new WaitForSeconds(GetForgivingInterval(group.GetParameters()));
			group.Dispose();
			TickManager.Enabled = prevManagerState;

			// Assert
			Assert.IsTrue(tickCount < 1, $"'{nameof(TickManager)}.{nameof(TickManager.Enabled)} = false' did not prevent {nameof(TickGroup)} from ticking.");

			yield break;

			void TestListenerMethod() => tickCount++;
		}

		[Test]
		public void TickManager_FindTickGroup_ReturnsTickGroup()
		{
			// Arrange
			GroupParams groupParams = GroupParams.Default;
			TickGroup tickGroup = new TickGroup(groupParams);

			// Act
			TickGroup foundGroup = TickManager.Find(GroupParams.Default.name);
			tickGroup.Dispose();

			// Assert
			Assert.AreEqual(tickGroup, foundGroup, $"{nameof(TickManager)} did not return the correct {nameof(TickGroup)}.");
		}

		[Test]
		public void TickManager_FindTickGroup_ReturnsTrue()
		{
			// Arrange
			GroupParams groupParams = GroupParams.Default;
			TickGroup tickGroup = new TickGroup(groupParams);

			// Act
			bool validGroup = TickManager.Find(GroupParams.Default.name, out TickGroup foundGroup);
			tickGroup.Dispose();

			// Assert
			Assert.IsTrue(validGroup, $"{nameof(TickManager)} did not return 'true' when finding {nameof(TickGroup)}.");
			Assert.AreEqual(tickGroup, foundGroup, $"{nameof(TickManager)} did not output the correct {nameof(TickGroup)}.");
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
			       $"A.active - {a.enabled}\n" +
			       $"B.active - {b.enabled}\n" +
			       $"A.useRealTime - {a.realTime}\n" +
			       $"B.useRealTime - {b.realTime}";
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
}