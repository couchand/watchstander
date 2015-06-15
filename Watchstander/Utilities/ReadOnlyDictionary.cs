using System;
using System.Collections;
using System.Collections.Generic;

// from <http://stackoverflow.com/a/1269311/2029668>
// thanks Thomas Levesque

namespace Watchstander.Utilities
{
	public class ReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
	{
		private readonly IDictionary<TKey, TValue> _dictionary;

		public ReadOnlyDictionary()
		{
			_dictionary = new Dictionary<TKey, TValue>();
		}

		public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
		{
			// TODO: copy
			// http://stackoverflow.com/questions/678379/is-there-a-read-only-generic-dictionary-available-in-net#comment8995247_1269311
			_dictionary = dictionary;
		}

		#region IDictionary<TKey,TValue> Members

		public bool ContainsKey(TKey key)
		{
			return _dictionary.ContainsKey(key);
		}

		public IEnumerable<TKey> Keys
		{
			get { return _dictionary.Keys; }
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return _dictionary.TryGetValue(key, out value);
		}

		public IEnumerable<TValue> Values
		{
			get { return _dictionary.Values; }
		}

		public TValue this[TKey key]
		{
			get
			{
				return _dictionary[key];
			}
		}

		TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key]
		{
			get
			{
				return this[key];
			}
		}

		#endregion

		#region ICollection<KeyValuePair<TKey,TValue>> Members

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return _dictionary.Contains(item);
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			_dictionary.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _dictionary.Count; }
		}

		public bool IsReadOnly
		{
			get { return true; }
		}

		#endregion

		#region IEnumerable<KeyValuePair<TKey,TValue>> Members

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return _dictionary.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		private static Exception ReadOnlyException()
		{
			return new NotSupportedException("This dictionary is read-only");
		}
	}
}

