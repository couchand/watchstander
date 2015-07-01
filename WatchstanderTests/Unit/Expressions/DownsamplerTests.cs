using NUnit;
using NUnit.Framework;

using System;

using Watchstander.Expressions;

namespace WatchstanderTests.Unit.Expressions
{
	[TestFixture]
	public class DownsamplerTests
	{
		[Test]
		[TestCase(10, "avg", "10s-avg")]
		[TestCase(60, "avg",  "1m-avg")]
		[TestCase(10, "max", "10s-max")]
		[TestCase(60 * 60, "zimsum", "1h-zimsum")]
		public void TestDownsampler (int s, string agg, string expected)
		{
			var duration = new RelativeDuration (TimeSpan.FromSeconds (s));
			var aggregator = new Aggregator (agg);

			var downsampler = new Downsampler (duration, aggregator);

			Assert.AreEqual (expected, downsampler.GetDownsampler ());
		}
	}
}

