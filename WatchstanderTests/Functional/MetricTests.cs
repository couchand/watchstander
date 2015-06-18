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
	public class MetricTests
	{
		private ICollectorMetric getMetric()
		{
			return getMetric (new NullPipelineElement());
		}

		private ICollectorMetric getMetric(IPipelineElement consumer)
		{
			return new RootCollector (consumer, new MockFlusher())
				.WithTag("host", "foobar")
				.GetMetric("foo.bar.baz");
		}

		[Test]
		public void TestLimitTagByValue()
		{
			var metric = getMetric ();

			var withWidget = (CollectorMetric)metric.WithTag ("widget", "qux");

			Assert.That (withWidget.Tags.ContainsKey ("widget"));
			Assert.AreEqual ("qux", withWidget.Tags ["widget"]);
		}

		[Test]
		public void TestLimitTagsByValueDictionary()
		{
			var metric = getMetric ();

			var tags = new Dictionary<string, string> ();
			tags ["widget"] = "qux";
			tags ["fruit"] = "banana";

			var withTags = (CollectorMetric)metric.WithTags (tags.AsReadOnly ());

			Assert.That (withTags.Tags.ContainsKey ("widget"));
			Assert.AreEqual ("qux", withTags.Tags ["widget"]);

			Assert.That (withTags.Tags.ContainsKey ("fruit"));
			Assert.AreEqual ("banana", withTags.Tags ["fruit"]);
		}

		[Test]
		public void TestLimitTagByTagger()
		{
			var metric = getMetric ();

			var hasFruit = metric.WithTagger<bool> ("fruit", b => b ? "banana" : "apple");
			var withFruit = (CollectorMetric) hasFruit.WithTag<bool> ("fruit", true);

			Assert.That (withFruit.Tags.ContainsKey ("host"));
			Assert.AreEqual ("foobar", withFruit.Tags ["host"]);
		}

		[Test]
		public void TestGetTimeSeriesNoTags()
		{
			var metric = getMetric ();

			var timeSeries = metric.GetTimeSeries<long> ();

			Assert.AreSame (metric, timeSeries.Metric);
		}

		[Test]
		public void TestGetTimeSeriesTagByValue()
		{
			var metric = getMetric ();

			var timeSeries = metric.GetTimeSeries<long> ("widget", "qux");

			Assert.AreSame (metric, timeSeries.Metric);

			Assert.That (timeSeries.Tags.ContainsKey ("widget"));
			Assert.AreEqual ("qux", timeSeries.Tags ["widget"]);
		}

		[Test]
		public void TestGetTimeSeriesTagsByDictionary()
		{
			var metric = getMetric ();

			var tags = new Dictionary<string, string> ();
			tags ["widget"] = "qux";
			tags ["fruit"] = "banana";

			var timeSeries = metric.GetTimeSeries<long> (tags.AsReadOnly());

			Assert.AreSame (metric, timeSeries.Metric);

			Assert.That (timeSeries.Tags.ContainsKey ("widget"));
			Assert.AreEqual ("qux", timeSeries.Tags ["widget"]);

			Assert.That (timeSeries.Tags.ContainsKey ("fruit"));
			Assert.AreEqual ("banana", timeSeries.Tags ["fruit"]);
		}

		[Test]
		public void TestGetTimeSeriesTagByTagger()
		{
			var metric = getMetric ();
			var hasFruit = metric.WithTagger<bool> ("fruit", b => b ? "banana" : "apple");

			var timeSeries = hasFruit.GetTimeSeries<long, bool> ("fruit", true);

			Assert.AreSame (hasFruit, timeSeries.Metric);

			Assert.That (timeSeries.Tags.ContainsKey ("fruit"));
			Assert.AreEqual ("banana", timeSeries.Tags ["fruit"]);
		}

		[Test]
		public void TestRecord ()
		{
			var consumer = new AccumulatingPipelineElement ();

			var metric = getMetric (consumer);

			metric.Record<long> (42);
			metric.Record<float> (1.0f);
			metric.Record<long> (43);
			metric.Record<float> (2.0f);

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

			var metric = getMetric (consumer);

			metric.Record<long> (42);

			var disabled = metric.Disabled ();
			disabled.Record<long> (0);

			var reenabled = disabled.Reenabled ();
			reenabled.Record<long> (43);

			Assert.AreEqual (2, consumer.longConsumer.Data.Count);

			Assert.AreEqual (42, consumer.longConsumer.Data [0].Value);
			Assert.AreEqual (43, consumer.longConsumer.Data [1].Value);
		}
	}
}

