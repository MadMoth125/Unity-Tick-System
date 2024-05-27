using System;
using System.Collections.Generic;
using System.Linq;

namespace TickSystem.Core
{
	public class TickGroup
	{
		/// <summary>
		/// The parameters for the group.
		/// </summary>
		public GroupParams Parameters { get; private set; }
		
		/// <summary>
		/// The actions to be invoked when the group ticks.
		/// </summary>
		public IReadOnlyList<Action> Callbacks => _callbacks;
		
		/// <summary>
		/// The number of callbacks in the group.
		/// </summary>
		public int CallbackCount => _callbacks.Count;
		
		private readonly List<Action> _callbacks;

		#region Constructors

		public TickGroup(GroupParams parameters, IEnumerable<Action> callbacks)
		{
			Parameters = parameters;
			_callbacks = callbacks.ToList();
		}

		public TickGroup(GroupParams parameters)
		{
			Parameters = parameters;
			_callbacks = new List<Action>();
		}

		public TickGroup(IEnumerable<Action> callbacks)
		{
			Parameters = GroupParams.Default;
			_callbacks = callbacks.ToList();
		}

		public TickGroup()
		{
			Parameters = GroupParams.Default;
			_callbacks = new List<Action>();
		}

		#endregion
		
		/// <summary>
		/// Adds a callback to the group.
		/// </summary>
		/// <param name="callback">The callback to add.</param>
		public void Add(Action callback)
		{
			if (callback == null) return;
			if (_callbacks.Contains(callback)) return;
			_callbacks.Add(callback);
		}
		
		/// <summary>
		/// Removes a callback from the group.
		/// </summary>
		/// <param name="callback">The callback to remove.</param>
		public void Remove(Action callback)
		{
			if (callback == null) return;
			if (!_callbacks.Contains(callback)) return;
			_callbacks.Remove(callback);
		}
		
		/// <summary>
		/// Clears all callbacks from the group.
		/// </summary>
		public void Clear()
		{
			if (_callbacks.Count == 0) return;
			_callbacks.Clear();
		}

		/// <summary>
		/// Invokes all callbacks in the group.
		/// </summary>
		public void Invoke()
		{
			if (_callbacks.Count == 0) return;
			// using a for-loop to avoid garbage allocation
			for (int i = 0; i < _callbacks.Count; i++)
			{
				_callbacks[i]?.Invoke();
			}
		}
	}
}