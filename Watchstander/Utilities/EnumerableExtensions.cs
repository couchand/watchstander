using System;
using System.Collections;
using System.Collections.Generic;

namespace Watchstander.Utilities
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<IList<TValue>> BreakIntoBatches<TValue>(this IEnumerable<TValue> items, int batchSize)
		{
			var currentBatch = new List<TValue> ();

			foreach (var item in items)
			{
				if (currentBatch.Count == batchSize)
				{
					yield return currentBatch;

					currentBatch = new List<TValue> ();
				}
				currentBatch.Add (item);
			}
			yield return currentBatch;
			yield break;
		}
	}
}