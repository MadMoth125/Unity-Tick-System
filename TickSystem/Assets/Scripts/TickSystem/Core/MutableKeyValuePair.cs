using System;
using System.Collections.Generic;

namespace TickSystem.Core
{
	public class MutableKeyValuePair<TKey, TValue>
	{
		public TKey Key;
		public TValue Value;
		
		public MutableKeyValuePair(TKey key, TValue value)
		{
			Key = key;
			Value = value;
		}

		public MutableKeyValuePair(KeyValuePair<TKey, TValue> pair)
		{
			Key = pair.Key;
			Value = pair.Value;
		}
		
		public void CopyFrom(MutableKeyValuePair<TKey, TValue> other)
		{
			Key = other.Key;
			Value = other.Value;
		}
		
		public void CopyFrom(KeyValuePair<TKey, TValue> pair)
		{
			Key = pair.Key;
			Value = pair.Value;
		}

		public void Set(TKey key, TValue value)
		{
			Key = key;
			Value = value;
		}
		
		public void Deconstruct(out TKey key, out TValue value)
		{
			key = Key;
			value = Value;
		}

		public KeyValuePair<TKey, TValue> AsKeyValuePair()
		{
			return new KeyValuePair<TKey, TValue>(Key, Value);
		}
		
		public override string ToString()
		{
			return $"[{Key}, {Value}]";
		}

		public static implicit operator MutableKeyValuePair<TKey, TValue>(KeyValuePair<TKey, TValue> pair)
		{
			return new MutableKeyValuePair<TKey, TValue>(pair);
		}
		
		public static implicit operator KeyValuePair<TKey, TValue>(MutableKeyValuePair<TKey, TValue> pair)
		{
			return new KeyValuePair<TKey, TValue>(pair.Key, pair.Value);
		}
	}
}