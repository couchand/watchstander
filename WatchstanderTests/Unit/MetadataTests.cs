using NUnit;
using NUnit.Framework;

using System;
using System.Collections.Generic;

using Watchstander.Common;
using Watchstander.Plumbing;
using Watchstander.Utilities;

namespace WatchstanderTests.Unit
{
	public class MetricTest : IMetric
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public Rate Rate { get; set; }
		public string Unit { get; set; }
		public IReadOnlyList<string> TagKeys { get; set; }
	}

	public class TimeSeriesTest : ITimeSeries
	{
		public string Description { get; set; }
		public IMetric Metric { get; set; }
		public IReadOnlyDictionary<string, string> TagValues { get; set; }
	}

	[TestFixture]
	public class MetadataTests
	{
		private readonly IReadOnlyList<string> tagKeys = (new List<string> (){ "host" }).AsReadOnly();
		private readonly string wittyDescription = "A count of the number of baz it takes to bar on foo.";

		[Test]
		public void TestMetricDescription()
		{
			var someMetric = new MetricTest ()
			{
				Name = "foo.bar.baz",
				Description = wittyDescription,
				Rate = Rate.Gauge,
				Unit = "baz",
				TagKeys = tagKeys
			};

			var metadata = MetadataFactory.GetDescription (someMetric);

			Assert.AreEqual ("foo.bar.baz", metadata.Metric);
			Assert.AreEqual (wittyDescription, metadata.Value);
			Assert.AreEqual ("desc", metadata.Name);

			// should be metric-global
			Assert.IsNull (metadata.Tags);
		}

		[Test]
		public void TestMetricRate()
		{
			var someMetric = new MetricTest ()
			{
				Name = "foo.bar.baz",
				Description = wittyDescription,
				Rate = Rate.Gauge,
				Unit = "baz",
				TagKeys = tagKeys
			};

			var metadata = MetadataFactory.GetRate (someMetric);

			Assert.AreEqual ("foo.bar.baz", metadata.Metric);
			Assert.AreEqual (Rate.Gauge.ToMetadataString(), metadata.Value);
			Assert.AreEqual ("rate", metadata.Name);

			// should be metric-global
			Assert.IsNull (metadata.Tags);
		}

		[Test]
		public void TestMetricUnit()
		{
			var someMetric = new MetricTest ()
			{
				Name = "foo.bar.baz",
				Description = wittyDescription,
				Rate = Rate.Gauge,
				Unit = "baz",
				TagKeys = tagKeys
			};

			var metadata = MetadataFactory.GetUnit (someMetric);

			Assert.AreEqual ("foo.bar.baz", metadata.Metric);
			Assert.AreEqual ("baz", metadata.Value);
			Assert.AreEqual ("unit", metadata.Name);

			// should be metric-global
			Assert.IsNull (metadata.Tags);
		}

		[Test]
		public void TestTimeSeriesDescription()
		{
			var someMetric = new MetricTest ()
			{
				Name = "foo.bar.baz",
				Description = wittyDescription,
				Rate = Rate.Gauge,
				Unit = "baz",
				TagKeys = tagKeys
			};

			var otherDescription = "The baz count on host 42.";
			var tags = new Dictionary<string, string> ();
			tags ["host"] = "42";

			var someTimeSeries = new TimeSeriesTest ()
			{
				Description = otherDescription,
				Metric = someMetric,
				TagValues = tags.AsReadOnly()
			};

			var metadata = MetadataFactory.GetDescription (someTimeSeries);

			Assert.AreEqual ("foo.bar.baz", metadata.Metric);
			Assert.AreEqual (otherDescription, metadata.Value);
			Assert.AreEqual ("desc", metadata.Name);

			Assert.That (metadata.Tags.IsIdentical (tags));
		}
	}
}

