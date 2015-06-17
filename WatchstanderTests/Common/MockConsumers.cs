using System;
using System.Collections.Generic;
using Watchstander.Common;

namespace WatchstanderTests.Common
{
	public class AccumulatingConsumer<TData> : IDataPointConsumer<TData>
	{
		public List<IDataPoint<TData>> Data { get; }

		public AccumulatingConsumer()
		{
			this.Data = new List<IDataPoint<TData>> ();
		}

		public void Consume(IEnumerable<IDataPoint<TData>> data)
		{
			Data.AddRange (data);
		}
	}

	public class NullConsumer<TData> : IDataPointConsumer<TData>
	{
		public NullConsumer() {}
		public void Consume(IEnumerable<IDataPoint<TData>> data) {}
	}

}

