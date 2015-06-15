/*
 * RateTests.cs
 *
 * Tests for the Rate enum (such as they are).
 *
 */

using NUnit;
using NUnit.Framework;
using System;

using Watchstander.Common;
using Watchstander.Plumbing;

namespace WatchstanderTests
{
	[TestFixture]
	public class RateTests
	{
		[Test]
		public void TestToMetadataString()
		{
			var gauge = Rate.Gauge;
			var rate = Rate.Rate;
			var counter = Rate.Counter;

			Assert.AreEqual(
				"gauge",
				gauge.ToMetadataString()
			);

			Assert.AreEqual(
				"rate",
				rate.ToMetadataString()
			);

			Assert.AreEqual(
				"counter",
				counter.ToMetadataString()
			);
		}
	}
}
