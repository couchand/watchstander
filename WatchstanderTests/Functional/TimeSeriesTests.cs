using NUnit;
using NUnit.Framework;

using System;

using System.Collections.Generic;

using Watchstander.Common;
using Watchstander.Porcelain;
using Watchstander.Utilities;

using WatchstanderTests.Common;

namespace WatchstanderTests.Functional
{
	[TestFixture]
	public class TimeSeriesTests
	{
		private ICollectorMetric getMetric()
		{
			return getMetric (new NullConsumer<long> (), new NullConsumer<float> ());
		}

		private ICollectorMetric getMetric(IDataPointConsumer<long> longConsumer, IDataPointConsumer<float> floatConsumer)
		{
			return new RootCollector (longConsumer, floatConsumer)
				.WithTag("host", "foobar")
				.GetMetric("foo.bar.baz");
		}

		[Test]
		public void TestRecord ()
		{
			var longConsumer = new AccumulatingConsumer<long> ();
			var floatConsumer = new AccumulatingConsumer<float> ();

			var longTimeSeries = getMetric (longConsumer, floatConsumer)
				.GetTimeSeries<long>();
			var floatTimeSeries = getMetric (longConsumer, floatConsumer)
				.GetTimeSeries<float>();

			longTimeSeries.Record (42);
			floatTimeSeries.Record (1.0f);
			longTimeSeries.Record (43);
			floatTimeSeries.Record (2.0f);

			Assert.AreEqual (2, longConsumer.Data.Count);
			Assert.AreEqual (2, floatConsumer.Data.Count);

			Assert.AreEqual (42, longConsumer.Data [0].Value);
			Assert.AreEqual (43, longConsumer.Data [1].Value);

			Assert.AreEqual (1.0f, floatConsumer.Data [0].Value);
			Assert.AreEqual (2.0f, floatConsumer.Data [1].Value);
		}

		[Test]
		public void TestDisabling()
		{
			var longConsumer = new AccumulatingConsumer<long> ();

			var timeSeries = getMetric (longConsumer, new NullConsumer<float>())
				.GetTimeSeries<long>();

			timeSeries.Record (42);

			var disabled = timeSeries.Disabled ();
			disabled.Record (0);

			var reenabled = disabled.Reenabled ();
			reenabled.Record (43);

			Assert.AreEqual (2, longConsumer.Data.Count);

			Assert.AreEqual (42, longConsumer.Data [0].Value);
			Assert.AreEqual (43, longConsumer.Data [1].Value);
		}
	}
}

