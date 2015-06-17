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
	public class LimitedCollectorTests
	{
		private ICollector getRootCollector()
		{
			return new RootCollector ();
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

			var withHost = collector.WithTags (tags.AsReadOnly ());
			var baz = (CollectorMetric)withHost.GetMetric ("baz");

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
			Assert.Throws<Exception>(() => collector.GetMetric ("foo.bar.baz"));
		}

		[Test]
		public void TestGetMetricNoTagsWithName()
		{
			var collector = getRootCollector ()
				.WithName ("foo")
				.WithName ("bar");

			Assert.Throws<Exception>(() => collector.GetMetric ("baz"));
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
	}
}
