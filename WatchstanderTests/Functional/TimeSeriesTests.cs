using NUnit;
using NUnit.Framework;

using System;

using System.Collections.Generic;

using Watchstander;
using Watchstander.Common;
using Watchstander.Plumbing;
using Watchstander.Porcelain;
using Watchstander.Utilities;

using WatchstanderTests.Common;

namespace WatchstanderTests.Functional
{
	[TestFixture]
	public class TimeSeriesTests
	{
		private ICollectorMetric<TData> getMetric<TData>()
		{
			return getMetric<TData> (new NullPipelineElement());
		}

		private ICollectorMetric<TData> getMetric<TData>(IPipelineElement consumer)
		{
			return new RootCollector (consumer, new MockFlusher())
				.WithTag("host", "foobar")
				.GetMetric<TData>("foo.bar.baz");
		}

		[Test]
		public void TestRecord ()
		{
			var consumer = new AccumulatingPipelineElement ();

			var longTimeSeries = getMetric<long> (consumer)
				.GetTimeSeries();
			var floatTimeSeries = getMetric<float> (consumer)
				.GetTimeSeries();

			longTimeSeries.Record (42);
			floatTimeSeries.Record (1.0f);
			longTimeSeries.Record (43);
			floatTimeSeries.Record (2.0f);

			Assert.AreEqual (2, consumer.longConsumer.Data.Count);
			Assert.AreEqual (2, consumer.floatConsumer.Data.Count);

			Assert.AreEqual (42, consumer.longConsumer.Data [0].Value);
			Assert.AreEqual (43, consumer.longConsumer.Data [1].Value);

			Assert.AreEqual (1.0f, consumer.floatConsumer.Data [0].Value);
			Assert.AreEqual (2.0f, consumer.floatConsumer.Data [1].Value);
		}

		[Test]
		public void TestDisabling()
		{
			var consumer = new AccumulatingPipelineElement ();

			var timeSeries = getMetric<long> (consumer)
				.GetTimeSeries();

			timeSeries.Record (42);

			var disabled = timeSeries.Disabled ();
			disabled.Record (0);

			var reenabled = disabled.Reenabled ();
			reenabled.Record (43);

			Assert.AreEqual (2, consumer.longConsumer.Data.Count);

			Assert.AreEqual (42, consumer.longConsumer.Data [0].Value);
			Assert.AreEqual (43, consumer.longConsumer.Data [1].Value);
		}
	}
}

