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
		/// Event when TickGroup should invoke callbacks.
		/// </summary>
		/// <remarks>
		/// Wraps <see cref="Add"/> and <see cref="Remove"/> for event syntax.
		/// </remarks>
		public event Action OnTick
		{
			add => Add(value);
			remove => Remove(value);
		}

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

		/// <summary>
		/// The number of callbacks in the TickGroup.
		/// </summary>
		public int Count => _callbacks.Count;

		/// <summary>
		/// Whether the TickGroup is still being referenced by the TickManager.
		/// </summary>
		/// <remarks>
		/// Typically false once <see cref="Dispose"/> has been called.
		/// </remarks>
		public bool Valid => !_disposed && TickManager.Contains(this);

		private readonly List<Action> _callbacks;
		private bool _disposed = false;

		#region Constructors

		public TickGroup(string name, float interval, bool? enabled = null, bool? realTime = null)
		{
			Name = name;
			Interval = interval;
			Enabled = enabled ?? GroupParams.Default.enabled;
			RealTime = realTime ?? GroupParams.Default.realTime;
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
			_disposed = true;
			TickManager.Remove(this);
			Clear();
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
			_callbacks.InvokeCallbacksReverse();
		}
	}
}