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
	public class LimitedCollectorTests
	{
		private ICollector getRootCollector()
		{
			return getRootCollector (new NullConsumer<long> (), new NullConsumer<float> ());
		}

		private ICollector getRootCollector(IDataPointConsumer<long> longConsumer, IDataPointConsumer<float> floatConsumer)
		{
			return new RootCollector (longConsumer, floatConsumer);
		}

		[Test]
		public void TestLimitName()
		{
			var collector = getRootCollector ();

			var withHost = collector.WithTag ("host", "foobar");
			var withFoo = withHost.WithName ("foo");
			var withBar = withFoo.WithName ("bar");
			var baz = withBar.GetMetric ("baz");

			Assert.AreEqual ("foo.bar.baz", baz.Name);
		}

		[Test]
		public void TestLimitNamePrefix()
		{
			var collector = getRootCollector ();

			var withHost = collector.WithTag ("host", "foobar");
			var withFoo = withHost.WithNamePrefix ("foo");
			var withBar = withFoo.WithName ("bar");
			var baz = withBar.GetMetric ("baz");

			Assert.AreEqual ("foobar.baz", baz.Name);
		}

		[Test]
		public void TestLimitTagByValue()
		{
			var collector = getRootCollector ();

			var withHost = collector.WithTag ("host", "foobar");
			var baz = (CollectorMetric)withHost.GetMetric ("baz");

			Assert.AreEqual (1, baz.Tags.Count);
			Assert.That (baz.Tags.ContainsKey ("host"));
			Assert.AreEqual ("foobar", baz.Tags ["host"]);
		}

		[Test]
		public void TestLimitTagsByValueDictionary()
		{
			var collector = getRootCollector ();

			var tags = new Dictionary<string, string> ();
			tags ["host"] = "foobar";
			tags ["widget"] = "qux";

			var withTags = collector.WithTags (tags.AsReadOnly ());
			var baz = (CollectorMetric)withTags.GetMetric ("baz");

			Assert.AreEqual (2, baz.Tags.Count);

			Assert.That (baz.Tags.ContainsKey ("host"));
			Assert.AreEqual ("foobar", baz.Tags ["host"]);

			Assert.That (baz.Tags.ContainsKey ("widget"));
			Assert.AreEqual ("qux", baz.Tags ["widget"]);
		}

		[Test]
		public void TestLimitTagByTagger()
		{
			var collector = getRootCollector ();

			var hasHost = collector.WithTagger<bool> ("host", b => b ? "foobar" : "failed");
			var withHost = hasHost.WithTag<bool> ("host", true);
			var baz = (CollectorMetric)withHost.GetMetric ("baz");

			Assert.AreEqual (1, baz.Tags.Count);
			Assert.That (baz.Tags.ContainsKey ("host"));
			Assert.AreEqual ("foobar", baz.Tags ["host"]);
		}

		[Test]
		public void TestGetMetricNoTags()
		{
			var collector = getRootCollector ();

			// TODO: needz moar type
			Assert.Throws<Exception>(() => collector.GetTimeSeries<long> ("foo.bar.baz"));
		}

		[Test]
		public void TestGetMetricNoTagsWithName()
		{
			var collector = getRootCollector ()
				.WithName ("foo")
				.WithName ("bar");

			Assert.Throws<Exception>(() => collector.GetTimeSeries<long> ("baz"));
		}

		[Test]
		public void TestGetMetric()
		{
			var collector = getRootCollector ()
				.WithTag ("host", "foobar")
				.WithName ("foo")
				.WithName ("bar");

			var metric = (CollectorMetric)collector.GetMetric ("baz");

			Assert.AreEqual ("foo.bar.baz", metric.Name);
			Assert.AreEqual (1, metric.Tags.Count);
			Assert.That (metric.Tags.ContainsKey ("host"));
			Assert.AreEqual ("foobar", metric.Tags ["host"]);
		}

		[Test]
		public void TestGetTimeSeriesByValue()
		{
			var collector = getRootCollector ();

			var timeSeries = collector.GetTimeSeries<long> ("foo.bar.baz", "host", "foobar");

			Assert.AreEqual ("foo.bar.baz", timeSeries.Metric.Name);
			Assert.AreEqual (1, timeSeries.Tags.Count);
			Assert.That (timeSeries.Tags.ContainsKey ("host"));
			Assert.AreEqual ("foobar", timeSeries.Tags ["host"]);
		}

		[Test]
		public void TestGetTimeSeriesByDictionary()
		{
			var collector = getRootCollector ();

			var tags = new Dictionary<string, string> ();
			tags ["host"] = "foobar";
			tags ["fruit"] = "banana";

			var timeSeries = collector.GetTimeSeries<long> ("foo.bar.baz", tags.AsReadOnly());

			Assert.AreEqual ("foo.bar.baz", timeSeries.Metric.Name);

			Assert.AreEqual (2, timeSeries.Tags.Count);

			Assert.That (timeSeries.Tags.ContainsKey ("host"));
			Assert.AreEqual ("foobar", timeSeries.Tags ["host"]);

			Assert.That (timeSeries.Tags.ContainsKey ("fruit"));
			Assert.AreEqual ("banana", timeSeries.Tags ["fruit"]);
		}

		[Test]
		public void TestGetTimeSeriesByTagger()
		{
			var collector = getRootCollector ()
				.WithTagger<bool> ("host", b => b ? "foobar" : "failed");

			var timeSeries = collector.GetTimeSeries<long, bool> ("foo.bar.baz", "host", true);

			Assert.AreEqual ("foo.bar.baz", timeSeries.Metric.Name);
			Assert.AreEqual (1, timeSeries.Tags.Count);
			Assert.That (timeSeries.Tags.ContainsKey ("host"));
			Assert.AreEqual ("foobar", timeSeries.Tags ["host"]);
		}

		[Test]
		public void TestDisablingRoot()
		{
			var longConsumer = new AccumulatingConsumer<long> ();

			var collector = getRootCollector (longConsumer, new NullConsumer<float>());

			var disabled = collector
				.Disabled ()
				.WithTag ("host", "foobar");

			var metric = disabled.GetMetric ("foo.bar.baz");

			metric.Record<long> (42);

			var reenabled = metric.Reenabled ();
			reenabled.Record<long> (43);

			Assert.AreEqual (1, longConsumer.Data.Count);
			Assert.AreEqual (43, longConsumer.Data [0].Value);
		}

		[Test]
		public void TestDisablingChild()
		{
			var longConsumer = new AccumulatingConsumer<long> ();

			var collector = getRootCollector (longConsumer, new NullConsumer<float>());

			var disabled = collector
				.WithTag ("host", "foobar")
				.Disabled ();

			var metric = disabled.GetMetric ("foo.bar.baz");

			metric.Record<long> (42);

			var reenabled = disabled
				.Reenabled ()
				.GetMetric("foo.bar.baz");
			reenabled.Record<long> (43);

			Assert.AreEqual (1, longConsumer.Data.Count);
			Assert.AreEqual (43, longConsumer.Data [0].Value);
		}
	}
}
