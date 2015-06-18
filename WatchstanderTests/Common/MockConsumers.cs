using System;
using System.Collections.Generic;
using Watchstander.Common;
using Watchstander.Porcelain;

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

	public class AccumulatingPipelineElement : IPipelineElement
	{
		public AccumulatingConsumer<long> longConsumer = new AccumulatingConsumer<long>();
		public AccumulatingConsumer<float> floatConsumer = new AccumulatingConsumer<float>();

		public void Consume(IEnumerable<IDataPoint<long>> data)
		{
			longConsumer.Consume (data);
		}

		public void Consume(IEnumerable<IDataPoint<float>> data)
		{
			floatConsumer.Consume (data);
		}
	}

	public class NullConsumer<TData> : IDataPointConsumer<TData>
	{
		public NullConsumer() {}
		public void Consume(IEnumerable<IDataPoint<TData>> data) {}
	}

	public class NullPipelineElement : IPipelineElement
	{
		public NullConsumer<long> longConsumer = new NullConsumer<long>();
		public NullConsumer<float> floatConsumer = new NullConsumer<float>();

		public void Consume(IEnumerable<IDataPoint<long>> data)
		{
			longConsumer.Consume (data);
		}

		public void Consume(IEnumerable<IDataPoint<float>> data)
		{
			floatConsumer.Consume (data);
		}
	}
}

