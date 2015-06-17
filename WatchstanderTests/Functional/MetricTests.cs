using NUnit;
using NUnit.Framework;

using System;

using System.Collections.Generic;

using Watchstander.Common;
using Watchstander.Porcelain;
using Watchstander.Utilities;


namespace WatchstanderTests.Functional
{
	[TestFixture]
	public class MetricTests
	{
		private ICollectorMetric getMetric()
		{
			return new RootCollector ()
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
	}
}

