using System;
using System.Collections.Generic;

namespace TickSystem.Core
{
	public struct MutableKeyValuePair<TKey, TValue>
	{
		public TKey Key { get; private set; }
		public TValue Value { get; private set; }
		
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

		public void SetKey(TKey key)
		{
			Key = key;
		}

		public void SetValue(TValue value)
		{
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
		
		public override bool Equals(object obj)
		{
			if (obj is MutableKeyValuePair<TKey, TValue> other)
			{
				return Key.Equals(other.Key) && Value.Equals(other.Value);
			}
			return false;
		}

		public bool Equals(MutableKeyValuePair<TKey, TValue> other)
		{
			return EqualityComparer<TKey>.Default.Equals(Key, other.Key) && EqualityComparer<TValue>.Default.Equals(Value, other.Value);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Key, Value);
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