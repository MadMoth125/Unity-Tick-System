using System;
using System.Collections.Generic;
using System.Linq;

namespace TickSystem.Core
{
	/* Representation of a group of callbacks that are invoked at a fixed interval.
	 * Provides methods to add, remove, and invoke callbacks.
	 * Includes parameters for the group such as name, interval, and active state.
	 *
	 * Each TickGroup instance is registered with the TickManager upon creation and unregisters when disposed is called,
	 * this allows the TickManager to track all active groups and allows TickUpdater to access & update each group.
	 */
	public class TickGroup : IDisposable
	{
		/// <summary>
		/// The actions to be invoked when the group ticks.
		/// </summary>
		public IReadOnlyList<Action> Callbacks => _callbacks;

		/// <summary>
		/// The number of callbacks in the group.
		/// </summary>
		public int CallbackCount => _callbacks.Count;

		/// <summary>
		/// The parameters for the group.
		/// </summary>
		public GroupParams parameters;

		private readonly List<Action> _callbacks;

		#region Constructors

		public TickGroup(GroupParams parameters, IEnumerable<Action> callbacks)
		{
			this.parameters = parameters;
			_callbacks = callbacks.ToList();
			TickManager.RegisterTickGroup(this);
		}

		public TickGroup(GroupParams parameters, params Action[] callbacks)
		{
			this.parameters = parameters;
			_callbacks = callbacks.ToList();
			TickManager.RegisterTickGroup(this);
		}

		public TickGroup(GroupParams parameters)
		{
			this.parameters = parameters;
			_callbacks = new List<Action>();
			TickManager.RegisterTickGroup(this);
		}

		public TickGroup(IEnumerable<Action> callbacks)
		{
			parameters = GroupParams.Default;
			_callbacks = callbacks.ToList();
			TickManager.RegisterTickGroup(this);
		}

		public TickGroup(params Action[] callbacks)
		{
			parameters = GroupParams.Default;
			_callbacks = callbacks.ToList();
			TickManager.RegisterTickGroup(this);
		}

		public TickGroup()
		{
			parameters = GroupParams.Default;
			_callbacks = new List<Action>();
			TickManager.RegisterTickGroup(this);
		}

		#endregion

		public static bool CompareName(in TickGroup group, in string name)
		{
			return group.parameters.name.Trim().Replace(" ", "") == name.Trim().Replace(" ", "");
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
		public void Clear() => _callbacks.Clear();

		/// <summary>
		/// Invokes all callbacks in the group.
		/// </summary>
		public void Invoke()
		{
			if (!parameters.active) return;
			// using a for-loop to avoid garbage allocation
			for (int i = 0; i < _callbacks.Count; i++) _callbacks[i]?.Invoke();
		}

		/// <summary>
		/// Clears all callbacks and unregisters the group, preventing further updates to it.
		/// </summary>
		public void Dispose()
		{
			TickManager.UnregisterTickGroup(this);
			Clear();
		}
	}
}