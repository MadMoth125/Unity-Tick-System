using System;
using System.Collections.Generic;
using System.Linq;

namespace TickSystem
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
		internal IReadOnlyList<Action> Callbacks => _callbacks;

		/// <summary>
		/// The number of callbacks in the group.
		/// </summary>
		public int Count => _callbacks.Count;

		/// <summary>
		/// The name of the TickGroup.
		/// </summary>
		public string Name;

		/// <summary>
		/// The interval between each TickGroup tick.
		/// </summary>
		public float Interval;

		/// <summary>
		/// Whether the TickGroup is enabled.
		/// </summary>
		public bool Enabled;

		/// <summary>
		/// Whether the TickGroup uses real time.
		/// If false, it uses game time.
		/// </summary>
		public bool RealTime;

		private readonly List<Action> _callbacks;

		#region Constructors

		public TickGroup(string name, float interval, bool? enabled = null, bool? realTime = null)
		{
			Name = name;
			Interval = interval;
			Enabled = enabled ?? true;
			RealTime = realTime ?? false;
			TickManager.Add(this);
		}

		public TickGroup(GroupParams parameters)
		{
			parameters.Deconstruct(out Name, out Interval, out Enabled, out RealTime);
			_callbacks = new List<Action>();
			TickManager.Add(this);
		}

		public TickGroup(GroupParams parameters, IEnumerable<Action> callbacks)
		{
			parameters.Deconstruct(out Name, out Interval, out Enabled, out RealTime);
			_callbacks = callbacks.ToList();
			TickManager.Add(this);
		}

		public TickGroup(GroupParams parameters, params Action[] callbacks)
		{
			parameters.Deconstruct(out Name, out Interval, out Enabled, out RealTime);
			_callbacks = callbacks.ToList();
			TickManager.Add(this);
		}

		public TickGroup(IEnumerable<Action> callbacks)
		{
			GroupParams.Default.Deconstruct(out Name, out Interval, out Enabled, out RealTime);
			_callbacks = callbacks.ToList();
			TickManager.Add(this);
		}

		public TickGroup(params Action[] callbacks)
		{
			GroupParams.Default.Deconstruct(out Name, out Interval, out Enabled, out RealTime);
			_callbacks = callbacks.ToList();
			TickManager.Add(this);
		}

		public TickGroup()
		{
			GroupParams.Default.Deconstruct(out Name, out Interval, out Enabled, out RealTime);
			_callbacks = new List<Action>();
			TickManager.Add(this);
		}

		#endregion

		/// <summary>
		/// Add a callback to the TickGroup.
		/// </summary>
		/// <param name="callback"></param>
		public void Add(Action callback)
		{
			if (callback == null) return;
			if (!_callbacks.Contains(callback)) _callbacks.Add(callback);
		}

		/// <summary>
		/// Remove a callback from the TickGroup.
		/// </summary>
		/// <param name="callback"></param>
		public void Remove(Action callback)
		{
			if (callback == null) return;
			if (_callbacks.Contains(callback)) _callbacks.Remove(callback);
		}

		/// <summary>
		/// Clear all callbacks from the TickGroup.
		/// </summary>
		public void Clear()
		{
			_callbacks.Clear();
		}

		/// <summary>
		/// Clear all callbacks and remove the TickGroup from the TickManager, preventing further updates to it.
		/// </summary>
		public void Dispose()
		{
			TickManager.Remove(this);
			Clear();
		}

		/// <summary>
		/// Checks if the TickGroup name matches the provided name.
		/// </summary>
		/// <param name="group"></param>
		/// <param name="name"></param>
		/// <returns>Whether the names are equal.</returns>
		public static bool CompareName(TickGroup group, in string name)
		{
			return group != null &&
			       group.Name.Trim().Replace(" ", "") == name.Trim().Replace(" ", "");
		}

		/// <summary>
		/// Invoke all callbacks in the TickGroup.
		/// </summary>
		/// <remarks>
		/// Accessor is 'internal' as to prevent scripts that are not
		/// included in the assembly from invoking the registered callbacks.
		/// </remarks>
		internal void Invoke()
		{
			if (!Enabled) return;

			for (int i = _callbacks.Count - 1; i >= 0; i--)
			{
				_callbacks?[i]?.Invoke();
			}
		}
	}
}