using NUnit;
using NUnit.Framework;

using System;
using System.Collections.Generic;

using Watchstander.Expressions;

namespace WatchstanderTests.Unit.Expressions
{
	[TestFixture]
	public class QueryBuilderTests
	{
		[Test]
		[TestCase("os.cpu")]
		[TestCase("os.mem.free")]
		[TestCase("foo.bar.baz")]
		public void TestDefaults (string metric)
		{
			var query = new QueryBuilder (metric);
			var expected = String.Format ("sum:{0}", metric);

			Assert.AreEqual (expected, query.GetQuery ());
		}

		[Test]
		[TestCase("os.cpu")]
		[TestCase("os.mem.free")]
		[TestCase("foo.bar.baz")]
		public void TestSetAggregator (string metric)
		{
			var query = new QueryBuilder (metric, Aggregators.Avg);
			var expected = String.Format ("avg:{0}", metric);

			Assert.AreEqual (expected, query.GetQuery ());

			query = new QueryBuilder (metric, Aggregators.ZimSum);
			expected = String.Format ("zimsum:{0}", metric);

			Assert.AreEqual (expected, query.GetQuery ());
		}

		[Test]
		[TestCase("os.cpu")]
		[TestCase("os.mem.free")]
		[TestCase("foo.bar.baz")]
		public void TestSetMetric (string metricName)
		{
			var metric = new MetricQuery (metricName, new Dictionary<string, string>{ { "host","*" } });
			var query = new QueryBuilder (metric);
			var expected = String.Format ("sum:{0}{{host=*}}", metricName);

			Assert.AreEqual (expected, query.GetQuery ());
		}

		[Test]
		[TestCase("os.cpu")]
		[TestCase("os.mem.free")]
		[TestCase("foo.bar.baz")]
		public void TestSetMetricAndAggregator (string metricName)
		{
			var metric = new MetricQuery (metricName, new Dictionary<string, string>{ { "host","*" } });

			var query = new QueryBuilder (metric, Aggregators.Avg);
			var expected = String.Format ("avg:{0}{{host=*}}", metricName);

			Assert.AreEqual (expected, query.GetQuery ());

			query = new QueryBuilder (metric, Aggregators.ZimSum);
			expected = String.Format ("zimsum:{0}{{host=*}}", metricName);

			Assert.AreEqual (expected, query.GetQuery ());
		}

		[Test]
		[TestCase("os.cpu")]
		[TestCase("os.mem.free")]
		[TestCase("foo.bar.baz")]
		public void TestWithAggregator(string metricName)
		{
			var metric = new MetricQuery (metricName, new Dictionary<string, string>{ { "host","*" } });

			var query = new QueryBuilder (metric, Aggregators.Avg);

			var withAggregator = query.WithAggregator (Aggregators.ZimSum);

			var expected = String.Format ("zimsum:{0}{{host=*}}", metricName);

			Assert.AreEqual (expected, withAggregator.GetQuery ());
		}

		[Test]
		[TestCase("os.cpu", 1)]
		[TestCase("os.mem.free", 1)]
		[TestCase("foo.bar.baz", 1)]
		[TestCase("os.cpu", 10)]
		[TestCase("os.mem.free", 10)]
		[TestCase("foo.bar.baz", 10)]
		public void TestWithDownsample(string metricName, int hours)
		{
			var metric = new MetricQuery (metricName, new Dictionary<string, string>{ { "host","*" } });

			var query = new QueryBuilder (metric, Aggregators.Avg);

			var time = TimeSpan.FromHours (hours);

			var downsampled = query.WithDownsample (time, Aggregators.Sum);

			var expected = String.Format ("avg:{1}h-sum:{0}{{host=*}}", metricName, hours);

			Assert.AreEqual (expected, downsampled.GetQuery ());
		}

		[Test]
		[TestCase("os.cpu", "1h")]
		[TestCase("os.mem.free", "1h")]
		[TestCase("foo.bar.baz", "1h")]
		[TestCase("os.cpu", "10y")]
		[TestCase("os.mem.free", "10y")]
		[TestCase("foo.bar.baz", "10y")]
		public void TestWithDownsampleText(string metricName, string duration)
		{
			var metric = new MetricQuery (metricName, new Dictionary<string, string>{ { "host","*" } });

			var query = new QueryBuilder (metric, Aggregators.Avg);

			var downsampled = query.WithDownsample (duration, Aggregators.Sum);

			var expected = String.Format ("avg:{1}-sum:{0}{{host=*}}", metricName, duration);

			Assert.AreEqual (expected, downsampled.GetQuery ());
		}

		[Test]
		[TestCase("os.cpu")]
		[TestCase("os.mem.free")]
		[TestCase("foo.bar.baz")]
		public void TestWithCounter (string metric)
		{
			var query = new QueryBuilder (metric);
			var expected = String.Format ("sum:rate{{counter,,}}:{0}", metric);

			var withCounter = query.WithCounter ();

			Assert.AreEqual (expected, withCounter.GetQuery ());
		}

		[Test]
		[TestCase("os.cpu", 1000, 900)]
		[TestCase("os.mem.free", 1000, 900)]
		[TestCase("foo.bar.baz", 700, 10)]
		public void TestWithCounterValues (string metric, int counterMax, int resetValue)
		{
			var query = new QueryBuilder (metric);
			var expected = String.Format ("sum:rate{{counter,{1},{2}}}:{0}", metric, counterMax, resetValue);

			var withCounter = query.WithCounter (counterMax, resetValue);

			Assert.AreEqual (expected, withCounter.GetQuery ());
		}

		[Test]
		[TestCase("os.cpu", 1000)]
		[TestCase("os.mem.free", 1000)]
		[TestCase("foo.bar.baz", 700)]
		public void TestWithCounterMax (string metric, int counterMax)
		{
			var query = new QueryBuilder (metric);
			var expected = String.Format ("sum:rate{{counter,{1},}}:{0}", metric, counterMax);

			var withCounter = query.WithCounterMax (counterMax);

			Assert.AreEqual (expected, withCounter.GetQuery ());
		}

		[Test]
		[TestCase("os.cpu", 1000)]
		[TestCase("os.mem.free", 1000)]
		[TestCase("foo.bar.baz", 700)]
		public void TestWithCounterReset (string metric, int resetValue)
		{
			var query = new QueryBuilder (metric);
			var expected = String.Format ("sum:rate{{counter,,{1}}}:{0}", metric, resetValue);

			var withCounter = query.WithCounterReset (resetValue);

			Assert.AreEqual (expected, withCounter.GetQuery ());
		}

		[Test]
		[TestCase("os.cpu", 1000)]
		[TestCase("os.mem.free", 1000)]
		[TestCase("foo.bar.baz", 700)]
		public void TestWithCounterMaxUpdated (string metric, int counterMax)
		{
			var query = new QueryBuilder (metric)
				.WithCounter(42, 42);
			var expected = String.Format ("sum:rate{{counter,{1},42}}:{0}", metric, counterMax);

			var withCounter = query.WithCounterMax (counterMax);

			Assert.AreEqual (expected, withCounter.GetQuery ());
		}

		[Test]
		[TestCase("os.cpu", 1000)]
		[TestCase("os.mem.free", 1000)]
		[TestCase("foo.bar.baz", 700)]
		public void TestWithCounterResetUpdated (string metric, int resetValue)
		{
			var query = new QueryBuilder (metric)
				.WithCounter(42, 42);
			var expected = String.Format ("sum:rate{{counter,42,{1}}}:{0}", metric, resetValue);

			var withCounter = query.WithCounterReset (resetValue);

			Assert.AreEqual (expected, withCounter.GetQuery ());
		}
	}
}

