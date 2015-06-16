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

		public static IReadOnlyDictionary<string, TValue> CombineDictionaries<TValue>(
			this IReadOnlyDictionary<string, TValue> first,
			IReadOnlyDictionary<string, TValue> second,
			IDictionary<string, TValue> target = null
		){
			if (target == null)
			{
				target = new Dictionary<string, TValue> ();
			}

			if (first != null)
			{
				foreach (var key in first.Keys)
				{
					if (target.ContainsKey (key))
					{
						// probably should do something here
						throw new Exception ("can't add tag twice");
					}

					target [key] = first [key];
				}
			}

			if (second != null)
			{
				foreach (var key in second.Keys)
				{
					if (target.ContainsKey (key))
					{
						// probably should do something here
						throw new Exception ("can't add tag twice");
					}

					target [key] = second [key];
				}
			}

			return target.AsReadOnly ();
		}
	}
}

