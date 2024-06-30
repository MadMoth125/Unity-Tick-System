using System;
using System.Collections.Generic;
using System.Linq;

namespace TickSystem.Core
{
	/// <summary>
	/// Encapsulates a collection of callbacks (actions) that should be
	/// invoked at specified intervals, defined by a set of parameters.
	/// </summary>
	public class TickGroup : IDisposable
	{
		/// <summary>
		/// The actions to be invoked when the group ticks.
		/// </summary>
		public IReadOnlyList<Action> Callbacks => _callbacks;

		/// <summary>
		/// The number of callbacks in the group.
		/// </summary>
		public int Count => _callbacks.Count;

		/// <summary>
		/// The parameters for the TickGroup.
		/// </summary>
		public GroupParams Parameters;

		private readonly List<Action> _callbacks;

		#region Constructors

		public TickGroup(string name, float interval, bool? active = null, bool? useRealTime = null)
		{
			Parameters = new GroupParams(name, interval, active ?? true, useRealTime ?? false);
			TickManager_New.Add(this);
		}

		public TickGroup(GroupParams parameters)
		{
			Parameters = parameters;
			_callbacks = new List<Action>();
			TickManager_New.Add(this);
		}

		public TickGroup(GroupParams parameters, IEnumerable<Action> callbacks)
		{
			Parameters = parameters;
			_callbacks = callbacks.ToList();
			TickManager_New.Add(this);
		}

		public TickGroup(GroupParams parameters, params Action[] callbacks)
		{
			Parameters = parameters;
			_callbacks = callbacks.ToList();
			TickManager_New.Add(this);
		}

		public TickGroup(IEnumerable<Action> callbacks)
		{
			Parameters = GroupParams.Default;
			_callbacks = callbacks.ToList();
			TickManager_New.Add(this);
		}

		public TickGroup(params Action[] callbacks)
		{
			Parameters = GroupParams.Default;
			_callbacks = callbacks.ToList();
			TickManager_New.Add(this);
		}

		public TickGroup()
		{
			Parameters = GroupParams.Default;
			_callbacks = new List<Action>();
			TickManager_New.Add(this);
		}

		#endregion

		/// <summary>
		/// Checks if the TickGroup name matches the provided name.
		/// </summary>
		/// <param name="group"></param>
		/// <param name="name"></param>
		/// <returns>Whether the names are equal.</returns>
		/// <remarks>
		/// Strings are compared with all whitespace being removed.
		/// </remarks>
		public static bool CompareName(TickGroup group, in string name)
		{
			return group != null &&
			       group.Parameters.name.Trim().Replace(" ", "") == name.Trim().Replace(" ", "");
		}

		/// <summary>
		/// Adds a callback to the group.
		/// </summary>
		/// <param name="callback">The callback to add.</param>
		public void Add(Action callback)
		{
			if (callback == null) return;
			if (!_callbacks.Contains(callback)) _callbacks.Add(callback);
		}

		/// <summary>
		/// Removes a callback from the group.
		/// </summary>
		/// <param name="callback">The callback to remove.</param>
		public void Remove(Action callback)
		{
			if (callback == null) return;
			if (_callbacks.Contains(callback)) _callbacks.Remove(callback);
		}

		/// <summary>
		/// Clears all callbacks from the group.
		/// </summary>
		public void Clear()
		{
			_callbacks.Clear();
		}

		/// <summary>
		/// Invokes all callbacks in the group.
		/// </summary>
		public void Invoke()
		{
			if (!Parameters.enabled) return;
			// using a for-loop to avoid garbage allocation
			for (int i = _callbacks.Count - 1; i >= 0; i--) _callbacks?[i]?.Invoke();
		}

		/// <summary>
		/// Clears all callbacks and unregisters the group, preventing further updates to it.
		/// </summary>
		public void Dispose()
		{
			TickManager_New.Remove(this);
			Clear();
		}
	}
}