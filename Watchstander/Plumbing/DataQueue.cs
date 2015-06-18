using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Watchstander.Common;

namespace Watchstander.Plumbing
{
	public class DataQueue : IPipelineElement
	{
		private readonly ConcurrentBag<IDataPoint<long>> longBag;
		private readonly ConcurrentBag<IDataPoint<float>> floatBag;

		public DataQueue ()
		{
			this.longBag = new ConcurrentBag<IDataPoint<long>> ();
			this.floatBag = new ConcurrentBag<IDataPoint<float>> ();
		}

		public void Consume(IEnumerable<IDataPoint<long>> dataPoints)
		{
			foreach (var dataPoint in dataPoints)
				longBag.Add (dataPoint);
		}

		public void Consume(IEnumerable<IDataPoint<float>> dataPoints)
		{
			foreach (var dataPoint in dataPoints)
				floatBag.Add (dataPoint);
		}

		public IEnumerable<IDataPoint<long>> TakeLongs(int count)
		{
			return Take<long> (count, longBag);
		}

		public IEnumerable<IDataPoint<float>> TakeFloats(int count)
		{
			return Take<float> (count, floatBag);
		}

		private IEnumerable<IDataPoint<TData>> Take<TData>(int count, ConcurrentBag<IDataPoint<TData>> bag)
		{
			IDataPoint<TData> item;

			bool gotItem = bag.TryTake (out item);

			List<IDataPoint<TData>> items = new List<IDataPoint<TData>> ();

			if (!gotItem)
			{
				return items;
			}

			items.Add (item);
			count -= 1;

			while (count > 0 && bag.TryTake (out item))
			{
				items.Add (item);
				count -= 1;
			}

			return items;
		}
	}
}

