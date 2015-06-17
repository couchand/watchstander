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

			Assert.LessOrEqual (2, withWidget.Tags.Count);
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

			Assert.LessOrEqual (2, withTags.Tags.Count);

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

			Assert.LessOrEqual (1, withFruit.Tags.Count);
			Assert.That (withFruit.Tags.ContainsKey ("host"));
			Assert.AreEqual ("foobar", withFruit.Tags ["host"]);
		}

		[Test]
		public void TestGetTimeSeriesNoTags()
		{
			var metric = getMetric ();

			var timeSeries = metric.GetTimeSeries ();

			Assert.AreSame (metric, timeSeries.Metric);
		}
	}
}

