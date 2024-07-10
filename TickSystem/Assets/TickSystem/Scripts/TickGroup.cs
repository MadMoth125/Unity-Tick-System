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
		public IReadOnlyList<Action> Callbacks => _callbacks;

		/// <summary>
		/// The number of callbacks in the group.
		/// </summary>
		public int Count => _callbacks.Count;

		public string Name;
		public float Interval;
		public bool Enabled;
		public bool IsRealTime;

		private readonly List<Action> _callbacks;

		#region Constructors

		public TickGroup(string name, float interval, bool? enabled = null, bool? realTime = null)
		{
			Name = name;
			Interval = interval;
			Enabled = enabled ?? true;
			IsRealTime = realTime ?? false;
			TickManager.Add(this);
		}

		public TickGroup(GroupParams parameters)
		{
			parameters.Deconstruct(out Name, out Interval, out Enabled, out IsRealTime);
			_callbacks = new List<Action>();
			TickManager.Add(this);
		}

		public TickGroup(GroupParams parameters, IEnumerable<Action> callbacks)
		{
			parameters.Deconstruct(out Name, out Interval, out Enabled, out IsRealTime);
			_callbacks = callbacks.ToList();
			TickManager.Add(this);
		}

		public TickGroup(GroupParams parameters, params Action[] callbacks)
		{
			parameters.Deconstruct(out Name, out Interval, out Enabled, out IsRealTime);
			_callbacks = callbacks.ToList();
			TickManager.Add(this);
		}

		public TickGroup(IEnumerable<Action> callbacks)
		{
			GroupParams.Default.Deconstruct(out Name, out Interval, out Enabled, out IsRealTime);
			_callbacks = callbacks.ToList();
			TickManager.Add(this);
		}

		public TickGroup(params Action[] callbacks)
		{
			GroupParams.Default.Deconstruct(out Name, out Interval, out Enabled, out IsRealTime);
			_callbacks = callbacks.ToList();
			TickManager.Add(this);
		}

		public TickGroup()
		{
			GroupParams.Default.Deconstruct(out Name, out Interval, out Enabled, out IsRealTime);
			_callbacks = new List<Action>();
			TickManager.Add(this);
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
			       group.Name.Trim().Replace(" ", "") == name.Trim().Replace(" ", "");
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
			if (!Enabled) return;

			// Using a for-loop to avoid garbage allocation
			for (int i = _callbacks.Count - 1; i >= 0; i--)
			{
				_callbacks?[i]?.Invoke();
			}
		}

		/// <summary>
		/// Clears all callbacks and unregisters the group, preventing further updates to it.
		/// </summary>
		public void Dispose()
		{
			TickManager.Remove(this);
			Clear();
		}
	}
}