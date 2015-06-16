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
			INameLimitable collector = getRootCollector ();

			var withFoo = collector.WithName ("foo");
			var withBar = withFoo.WithName ("bar");

			Assert.AreEqual ("foo.bar.", withBar.NamePrefix);
		}

		[Test]
		public void TestLimitNamePrefix()
		{
			INameLimitable collector = getRootCollector ();

			var withFoo = collector.WithNamePrefix ("foo");
			var withBar = withFoo.WithName ("bar");

			Assert.AreEqual ("foobar.", withBar.NamePrefix);
		}

		[Test]
		public void TestLimitTagByValue()
		{
			var collector = getRootCollector ();

			var withHost = collector.WithTag ("host", "foobar");

			Assert.AreEqual (1, withHost.Tags.Count);
			Assert.That (withHost.Tags.ContainsKey ("host"));
			Assert.AreEqual ("foobar", withHost.Tags ["host"]);
		}

		[Test]
		public void TestLimitTagsByValueDictionary()
		{
			var collector = getRootCollector ();

			var tags = new Dictionary<string, string> ();
			tags ["host"] = "foobar";
			tags ["widget"] = "qux";

			var withHost = collector.WithTags (tags.AsReadOnly ());

			Assert.AreEqual (2, withHost.Tags.Count);

			Assert.That (withHost.Tags.ContainsKey ("host"));
			Assert.AreEqual ("foobar", withHost.Tags ["host"]);

			Assert.That (withHost.Tags.ContainsKey ("widget"));
			Assert.AreEqual ("qux", withHost.Tags ["widget"]);
		}

		[Test]
		public void TestLimitTagByTagger()
		{
			var collector = getRootCollector ();

			var hasHost = collector.WithTagger<bool> ("host", b => b ? "foobar" : "failed");
			var withHost = hasHost.WithTag<bool> ("host", true);

			Assert.AreEqual (1, withHost.Tags.Count);
			Assert.That (withHost.Tags.ContainsKey ("host"));
			Assert.AreEqual ("foobar", withHost.Tags ["host"]);
		}

		[Test]
		public void TestGetMetricNoTags()
		{
			var collector = getRootCollector ();

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

			var metric = collector.GetMetric ("baz");

			Assert.AreEqual ("foo.bar.baz", metric.Name);
			Assert.AreEqual (1, metric.Tags.Count);
			Assert.That (metric.Tags.ContainsKey ("host"));
			Assert.AreEqual ("foobar", metric.Tags ["host"]);
		}
	}
}
