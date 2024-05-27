using System;
using System.Collections.Generic;

namespace TickSystem.Core
{
	public class TickGroup
	{
		// The parameters for the group.
		public GroupParams Parameters { get; private set; }
		
		// The actions to be invoked when the group ticks.
		public IReadOnlyList<Action> Callbacks => _callbacks;
		
		// The number of callbacks in the group.
		public int CallbackCount => _callbacks.Count;
		
		private readonly List<Action> _callbacks;

		#region Constructors

		public TickGroup(GroupParams parameters, List<Action> callbacks)
		{
			Parameters = parameters;
			_callbacks = callbacks;
		}

		public TickGroup(GroupParams parameters)
		{
			Parameters = parameters;
			_callbacks = new List<Action>();
		}

		public TickGroup(List<Action> callbacks)
		{
			Parameters = GroupParams.Default;
			_callbacks = callbacks;
		}

		public TickGroup()
		{
			Parameters = GroupParams.Default;
			_callbacks = new List<Action>();
		}

		#endregion
		
		public void Add(Action callback)
		{
			if (callback == null) return;
			if (_callbacks.Contains(callback)) return;
			_callbacks.Add(callback);
		}
		
		public void Remove(Action callback)
		{
			if (callback == null) return;
			if (!_callbacks.Contains(callback)) return;
			_callbacks.Remove(callback);
		}
		
		public void Clear()
		{
			if (_callbacks.Count == 0) return;
			_callbacks.Clear();
		}

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