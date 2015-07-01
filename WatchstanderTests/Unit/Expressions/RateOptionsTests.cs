using NUnit;
using NUnit.Framework;

using System;

using Watchstander.Expressions;

namespace WatchstanderTests.Unit.Expressions
{
	[TestFixture]
	public class RateOptionsTests
	{
		[Test]
		[TestCase( 1,  1)]
		[TestCase(42, 42)]
		public void TestRateBothOptions (int counterMax, int resetValue)
		{
			var rate = new RateOptions (counterMax, resetValue);

			var expected = String.Format ("rate{{counter,{0},{1}}}", counterMax, resetValue);

			Assert.AreEqual (expected, rate.GetRate ());
		}

		[Test]
		[TestCase(1)]
		[TestCase(42)]
		public void TestRateMaxOption (int counterMax)
		{
			var rate = new RateOptions (counterMax, null);

			var expected = String.Format ("rate{{counter,{0},}}", counterMax);

			Assert.AreEqual (expected, rate.GetRate ());
		}

		[Test]
		[TestCase(1)]
		[TestCase(42)]
		public void TestRateResetOption (int resetValue)
		{
			var rate = new RateOptions (null, resetValue);

			var expected = String.Format ("rate{{counter,,{0}}}", resetValue);

			Assert.AreEqual (expected, rate.GetRate ());
		}

		[Test]
		public void TestRateNoOptions ()
		{
			var rate = new RateOptions (null, null);

			Assert.AreEqual ("rate{counter,,}", rate.GetRate ());
		}

		[Test]
		[TestCase(0,  1)]
		[TestCase(0, 42)]
		public void TestRateWithMax (int initialMax, int finalMax)
		{
			var rate = new RateOptions (initialMax, null);

			var expected = String.Format ("rate{{counter,{0},}}", finalMax);

			var withMax = rate.WithCounterMax (finalMax);

			Assert.AreEqual (expected, withMax.GetRate ());
		}

		[Test]
		[TestCase(0,  1)]
		[TestCase(0, 42)]
		public void TestRateWithReset (int initialReset, int finalReset)
		{
			var rate = new RateOptions (null, initialReset);

			var expected = String.Format ("rate{{counter,,{0}}}", finalReset);

			var withReset = rate.WithResetValue (finalReset);

			Assert.AreEqual (expected, withReset.GetRate ());
		}
	}
}

