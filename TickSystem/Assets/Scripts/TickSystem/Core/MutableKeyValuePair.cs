using System.Collections.Generic;

namespace TickSystem.Core
{
	public class MutableKeyValuePair<TKey, TValue>
	{
		public TKey Key;
		public TValue Value;

		#region Constructors

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

		#endregion
		
		/// <summary>
		/// Copies values from another MutableKeyValuePair instance.
		/// </summary>
		/// <param name="other">The MutableKeyValuePair to copy values from.</param>
		public void CopyFrom(MutableKeyValuePair<TKey, TValue> other)
		{
			Key = other.Key;
			Value = other.Value;
		}
		
		/// <summary>
		/// Copies values from a KeyValuePair instance.
		/// </summary>
		/// <param name="pair">The KeyValuePair to copy values from.</param>
		public void CopyFrom(KeyValuePair<TKey, TValue> pair)
		{
			Key = pair.Key;
			Value = pair.Value;
		}

		/// <summary>
		/// Sets the key and value of the pair.
		/// </summary>
		/// <param name="key">The new key.</param>
		/// <param name="value">The new value.</param>
		public void Set(TKey key, TValue value)
		{
			Key = key;
			Value = value;
		}
		
		/// <summary>
		/// Deconstructs the pair into key and value.
		/// </summary>
		/// <param name="key">The key of the pair.</param>
		/// <param name="value">The value of the pair.</param>
		public void Deconstruct(out TKey key, out TValue value)
		{
			key = Key;
			value = Value;
		}

		/// <summary>
		/// Converts the instance to a KeyValuePair.
		/// </summary>
		/// <returns>A KeyValuePair representing the instance.</returns>
		public KeyValuePair<TKey, TValue> AsKeyValuePair()
		{
			return new KeyValuePair<TKey, TValue>(Key, Value);
		}
		
		public override string ToString()
		{
			return $"[{Key}, {Value}]";
		}

		#region Operators

		public static implicit operator MutableKeyValuePair<TKey, TValue>(KeyValuePair<TKey, TValue> pair)
		{
			return new MutableKeyValuePair<TKey, TValue>(pair);
		}
		
		public static implicit operator KeyValuePair<TKey, TValue>(MutableKeyValuePair<TKey, TValue> pair)
		{
			return new KeyValuePair<TKey, TValue>(pair.Key, pair.Value);
		}

		#endregion
	}
}