using NUnit;
using NUnit.Framework;

using System;

using Watchstander.Common;
using Watchstander.Plumbing;

namespace WatchstanderTests.Unit
{
	[TestFixture]
	public class AccumulatingMetricTests
	{
		[Test]
		public void TestSetDescription ()
		{
			var metric = new AccumulatingMetric ("foo.bar.baz");

			Assert.IsNull (metric.Description);

			var wittyDescription = "The number of baz used by bar on foo.";

			metric.SetDescription (wittyDescription);

			Assert.AreEqual (wittyDescription, metric.Description);

			metric.SetDescription ("something else");

			Assert.AreEqual ("something else", metric.Description);
		}

		[Test]
		public void TestSetRate ()
		{
			var metric = new AccumulatingMetric ("foo.bar.baz");

			Assert.AreEqual (Rate.Unknown, metric.Rate);

			metric.SetRate (Rate.Gauge);

			Assert.AreEqual (Rate.Gauge, metric.Rate);

			metric.SetRate (Rate.Counter);

			Assert.AreEqual (Rate.Counter, metric.Rate);
		}

		[Test]
		public void TestSetRateRequiresKnownRate ()
		{
			var metric = new AccumulatingMetric ("foo.bar.baz");

			Assert.Throws<Exception> (() => metric.SetRate (Rate.Unknown));
		}

		[Test]
		public void TestSetUnit ()
		{
			var metric = new AccumulatingMetric ("foo.bar.baz");

			Assert.IsNull (metric.Unit);

			metric.SetUnit ("baz");

			Assert.AreEqual ("baz", metric.Unit);

			metric.SetUnit ("volts");

			Assert.AreEqual ("volts", metric.Unit);
		}

		[Test]
		public void TestAddTagKey ()
		{
			var metric = new AccumulatingMetric ("foo.bar.baz");

			Assert.AreEqual (0, metric.TagKeys.Count);

			metric.TaggerRepository.AddStatic ("host", "foobar");

			Assert.AreEqual (1, metric.TagKeys.Count);
			Assert.AreEqual ("host", metric.TagKeys [0]);
		}
	}

	[TestFixture]
	public class SetOnceMetricTests
	{
		[Test]
		public void TestSetDescription ()
		{
			var metric = new SetOnceMetric ("foo.bar.baz");

			Assert.IsNull (metric.Description);

			var wittyDescription = "The number of baz used by bar on foo.";

			metric.SetDescription (wittyDescription);

			Assert.AreEqual (wittyDescription, metric.Description);

			Assert.Throws<Exception> (() => metric.SetDescription ("something else"));
		}

		[Test]
		public void TestSetRate ()
		{
			var metric = new SetOnceMetric ("foo.bar.baz");

			Assert.AreEqual (Rate.Unknown, metric.Rate);

			metric.SetRate (Rate.Gauge);

			Assert.AreEqual (Rate.Gauge, metric.Rate);

			Assert.Throws<Exception> (() => metric.SetRate (Rate.Counter));
		}

		[Test]
		public void TestSetUnit ()
		{
			var metric = new SetOnceMetric ("foo.bar.baz");

			Assert.IsNull (metric.Unit);

			metric.SetUnit ("baz");

			Assert.AreEqual ("baz", metric.Unit);

			Assert.Throws<Exception> (() => metric.SetUnit ("volts"));
		}

		[Test]
		public void TestAddTagKey ()
		{
			var metric = new SetOnceMetric ("foo.bar.baz");

			Assert.AreEqual (0, metric.TagKeys.Count);

			metric.TaggerRepository.AddStatic ("host", "foobar");

			Assert.AreEqual (1, metric.TagKeys.Count);
			Assert.AreEqual ("host", metric.TagKeys [0]);
		}
	}
}

