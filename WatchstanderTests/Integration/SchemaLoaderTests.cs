using NUnit;
using NUnit.Framework;

using System;

using Watchstander.Common;
using Watchstander.Plumbing;

namespace WatchstanderTests
{
	[TestFixture]
	public class SchemaLoaderTests
	{
		private Api GetApi()
		{
			var url = new Uri("http://localhost:8070");
			var api = new Api(new ApiOptions(url, new SerializerOptions()));

			GZip.Use(api);

			return api;
		}

		[Test]
		public void TestsLoadSchema ()
		{
			var options = new SchemaLoaderOptions();

			var loader = new SchemaLoader (options);

			var api = GetApi ();

			var schema = loader.LoadSchema (api);

			Assert.That (schema.Entries.ContainsKey ("foo.bar.baz"));

			var entry = schema.Entries ["foo.bar.baz"];
			Assert.NotNull (entry);

			var metric = entry.Metric;
			Assert.NotNull (metric);

			Assert.AreEqual ("\"baz\"", metric.Unit);
			Assert.AreEqual (Rate.Gauge, metric.Rate);

			var timeSeries = entry.TimeSeries;
			Assert.NotNull (timeSeries);

			Assert.Greater (timeSeries.Count, 0);

			Assert.NotNull (timeSeries [0].Description);
			Assert.NotNull (metric.Description);
			Assert.AreNotEqual (metric.Description, timeSeries [0].Description);

			Assert.Greater (timeSeries [0].TagValues.Count, 0);
		}
	}
}

