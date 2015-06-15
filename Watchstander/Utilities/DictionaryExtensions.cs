using System;
using System.Collections.Generic;

namespace Watchstander.Utilities
{
	public static class DictionaryExtensions
	{
		public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> source)
		{
			if (source == null)
				return null;

			return (IReadOnlyDictionary<TKey, TValue>)new ReadOnlyDictionary<TKey, TValue> (source);
		}

		public static bool IsIdentical<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> self, IReadOnlyDictionary<TKey, TValue> other)
		{
			if (self == null || other == null)
				return false;

			if (self.Count != other.Count)
				return false;

			foreach (var key in self.Keys)
			{
				if (!other.ContainsKey (key))
					return false;

				if (!self[key].Equals(other[key]))
					return false;
			}

			return true;
		}
	}
}

