using NUnit;
using NUnit.Framework;
using System;

using Watchstander.Expressions;

namespace WatchstanderTests.Unit.Expressions
{
	[TestFixture]
	public class CurrentTimeTests
	{
		[Test]
		public void TestCurrentTime()
		{
			var now = new CurrentTime ();

			Assert.AreEqual ("", now.GetDuration ());
		}
	}

	[TestFixture]
	public class StringDurationTests
	{
		[Test]
		[TestCase("1h")]
		[TestCase("3d")]
		[TestCase("100y")]
		[TestCase("oneGoogleplexFoobars")]
		public void TestDuration(string value)
		{
			var duration = new StringDuration (value);

			Assert.AreEqual (value, duration.GetDuration());
		}
	}

	[TestFixture]
	public class RelativeDurationTests
	{
		[Test]
		[TestCase(                 1,   "1ms")]
		[TestCase(                10,  "10ms")]
		[TestCase(               100, "100ms")]
		[TestCase(              1000,   "1s")]
		[TestCase(          2 * 1000,   "2s")]
		[TestCase(         42 * 1000,  "42s")]
		[TestCase(         60 * 1000,   "1m")]
		[TestCase(     2 * 60 * 1000,   "2m")]
		[TestCase(    42 * 60 * 1000,  "42m")]
		[TestCase(    60 * 60 * 1000,   "1h")]
		[TestCase(2 * 60 * 60 * 1000,   "2h")]
		public void TestSmallIntervals(int ms, string expected)
		{
			var span = TimeSpan.FromMilliseconds (ms);

			var relative = new RelativeDuration (span);

			Assert.AreEqual (expected, relative.GetDuration ());
		}

		[Test]
		[TestCase(           1,  "1h")]
		[TestCase(          10, "10h")]
		[TestCase(          24,  "1d")]
		[TestCase(      2 * 24,  "2d")]
		[TestCase(      7 * 24,  "1w")]
		[TestCase(     30 * 24,  "1n")]
		[TestCase( 2 * 30 * 24,  "2n")]
		[TestCase(10 * 30 * 24, "10n")]
		[TestCase(    365 * 24,  "1y")]
		public void TestLargeIntervals(int h, string expected)
		{
			var span = TimeSpan.FromHours (h);

			var relative = new RelativeDuration (span);

			Assert.AreEqual (expected, relative.GetDuration ());
		}
	}
}
