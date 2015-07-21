using NUnit;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;

using Watchstander.Common;
using Watchstander.Plumbing;
using Watchstander.Utilities;

using WatchstanderTests.Common;

namespace WatchstanderTests.Integration
{
	[TestFixture]
	public class IntegrationTests
	{
		Random myRandom = new Random();

		private Api GetApi()
		{
			var url = new Uri("http://localhost:8070");
			var api = new Api(new ApiOptions(url, new SerializerOptions()));

			GZip.Use(api);

			return api;
		}

		[Test]
		public void LiveTest()
		{
			var api = GetApi ();

			var tags = new Dictionary<string, string>();

			tags["host"] = "foobar";

			var bazCount = (long)Math.Floor (500 * myRandom.NextDouble () + 100);

			var data = new DataTest<long>(
				"foo.bar.baz",
				DateTime.UtcNow,
				bazCount,
				tags.AsReadOnly()
			);

			api.Put(data);
		}

		[Test]
		public void LiveMetadataTest()
		{
			var api = GetApi ();

			var tags = new Dictionary<string, string>();

			tags["host"] = "foobar";

			var metricDescription = new MetadataTest(
				"foo.bar.baz",
				null,
				"desc",
				"A count of the number of baz it takes to bar on foo."
			);

			var seriesDescription = new MetadataTest(
				"foo.bar.baz",
				tags.AsReadOnly(),
				"desc",
				"A count of the number of baz it takes to bar on foo for foobar."
			);

			var rate = new MetadataTest (
				"foo.bar.baz",
				null,
				"rate",
				"gauge"
			);

			var unit = new MetadataTest (
				"foo.bar.baz",
				null,
				"unit",
				"baz"
			);

			api.PutMetadata (new List<MetadataTest> (){ metricDescription, seriesDescription, rate, unit });
		}

		[Test]
		public void ListMetadataTest()
		{
			var api = GetApi ();

			var metadata = api.ListMetadata ();

			Assert.Greater (metadata.Count, 0);
		}

		[Test]
		public void ListMetricsTest()
		{
			var api = GetApi ();

			var metrics = api.ListMetrics ();

			Assert.Greater (metrics.Count, 0);
		}

		[Test]
		public void ListMetricTags()
		{
			var api = GetApi ();

			var tagKeys = api.ListMetricTagKeys ("foo.bar.baz");

			Assert.Greater (tagKeys.Count, 0);
		}
	}
}

