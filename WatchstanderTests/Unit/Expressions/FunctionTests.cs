using NUnit;
using NUnit.Framework;
using System;

using Watchstander.Expressions;

namespace WatchstanderTests.Unit.Expressions
{
	public class ConstantNumber : NumberSet
	{
		private double number;

		public ConstantNumber(double number)
		{
			this.number = number;
		}

		public override string GetExpression()
		{
			return number.ToString();
		}
	}

	public class ConstantSeries : SeriesSet
	{
		private double number;

		public ConstantSeries(double number)
		{
			this.number = number;
		}

		public override string GetExpression()
		{
			return String.Format ("constant({0})", number);
		}
	}

	[TestFixture]
	public class FunctionTests
	{
		[Test]
		[TestCase("metric", "1h-ago", "", "q(\"sum:metric\",\"1h-ago\",\"\")")]
		[TestCase("metric", "1d-ago", "1h-ago", "q(\"sum:metric\",\"1d-ago\",\"1h-ago\")")]
		[TestCase("thing", "1h-ago", "10m-ago", "q(\"sum:thing\",\"1h-ago\",\"10m-ago\")")]
		public void TestQ(string metric, string start, string end, string expected)
		{
			var queryBuilder = new QueryBuilder (metric);
			var startDuration = new StringDuration (start);
			var endDuration = new StringDuration (end);

			var q = new Q (queryBuilder, startDuration, endDuration);

			Assert.AreEqual (expected, q.GetExpression());
		}

		[Test]
		[TestCase("metric",  1, 1, 7)]
		[TestCase("metric", 10, 1, 7)]
		[TestCase("metric", 12, 1, 7)]
		[TestCase("metric",  1, 2, 7)]
		[TestCase("metric", 10, 2, 7)]
		[TestCase("metric", 12, 2, 7)]
		[TestCase("metric",  1, 1, 42)]
		[TestCase("metric", 10, 1, 42)]
		[TestCase("metric", 12, 1, 42)]
		[TestCase("metric",  1, 2, 42)]
		[TestCase("metric", 10, 2, 42)]
		[TestCase("metric", 12, 2, 42)]
		public void TestBand(string metric, int durationHours, int periodDays, int count)
		{
			var duration = new RelativeDuration (TimeSpan.FromHours (durationHours));
			var period = new RelativeDuration (TimeSpan.FromDays (periodDays));

			var band = new Band (new QueryBuilder(metric), duration, period, count);

			var expected = String.Format ("band(\"sum:{0}\",\"{1}h\",\"{2}d\",{3})", metric, durationHours, periodDays, count);

			Assert.AreEqual (expected, band.GetExpression ());
		}

		[Test]
		[TestCase("metric", "1h", "")]
		[TestCase("metric", "2h", "1h")]
		[TestCase("metric", "1d", "12h")]
		[TestCase("metric", "1w", "6d")]
		[TestCase("metric", "1m", "3w")]
		public void TestChange(string metric, string start, string end)
		{
			var startDuration = new StringDuration (start);
			var endDuration = new StringDuration (end);

			var change = new Change (new QueryBuilder (metric), startDuration, endDuration);

			var expected = String.Format ("change(\"sum:{0}\",\"{1}\",\"{2}\")", metric, start, end);

			Assert.AreEqual (expected, change.GetExpression ());
		}

		[Test]
		[TestCase("metric", "1h", "")]
		[TestCase("metric", "2h", "1h")]
		[TestCase("metric", "1d", "12h")]
		[TestCase("metric", "1w", "6d")]
		[TestCase("metric", "1m", "3w")]
		public void TestCount(string metric, string start, string end)
		{
			var startDuration = new StringDuration (start);
			var endDuration = new StringDuration (end);

			var count = new Count (new QueryBuilder (metric), startDuration, endDuration);

			var expected = String.Format ("count(\"sum:{0}\",\"{1}\",\"{2}\")", metric, start, end);

			Assert.AreEqual (expected, count.GetExpression ());
		}

		[Test]
		[TestCase("metric",  "1h", "1d", 7)]
		[TestCase("metric",  "1h", "3d", 7)]
		[TestCase("metric", "12h", "1d", 7)]
		[TestCase("metric",  "1h", "1d", 42)]
		[TestCase("metric",  "1h", "3d", 42)]
		[TestCase("metric", "12h", "1d", 42)]
		public void TestWindow(string metric, string duration, string period, int count)
		{
			var durationDuration = new StringDuration (duration);
			var periodDuration = new StringDuration (period);

			var window = new Window (new QueryBuilder (metric), durationDuration, periodDuration, count, Aggregators.Avg);

			var expected = String.Format ("window(\"sum:{0}\",\"{1}\",\"{2}\",{3},\"avg\")", metric, duration, period, count);

			Assert.AreEqual (expected, window.GetExpression ());
		}

		[Test]
		[TestCase(42, "avg(constant(42))")]
		[TestCase(0, "avg(constant(0))")]
		public void TestAvg(int number, string expected)
		{
			var avg = new Avg (new ConstantSeries(number));

			Assert.AreEqual (expected, avg.GetExpression());
		}

		[Test]
		[TestCase(42, "dev(constant(42))")]
		[TestCase(0, "dev(constant(0))")]
		public void TestDev(int number, string expected)
		{
			var dev = new Dev (new ConstantSeries(number));

			Assert.AreEqual (expected, dev.GetExpression());
		}

		[Test]
		[TestCase(42, "diff(constant(42))")]
		[TestCase(0, "diff(constant(0))")]
		public void TestDiff(int number, string expected)
		{
			var diff = new Diff (new ConstantSeries(number));

			Assert.AreEqual (expected, diff.GetExpression());
		}

		[Test]
		[TestCase(42, "first(constant(42))")]
		[TestCase(0, "first(constant(0))")]
		public void TestFirst(int number, string expected)
		{
			var first = new First (new ConstantSeries(number));

			Assert.AreEqual (expected, first.GetExpression());
		}

		[Test]
		[TestCase(42, "last(constant(42))")]
		[TestCase(0, "last(constant(0))")]
		public void TestLast(int number, string expected)
		{
			var last = new Last (new ConstantSeries(number));

			Assert.AreEqual (expected, last.GetExpression());
		}

		[Test]
		[TestCase(42, "len(constant(42))")]
		[TestCase(0, "len(constant(0))")]
		public void TestLen(int number, string expected)
		{
			var len = new Len (new ConstantSeries(number));

			Assert.AreEqual (expected, len.GetExpression());
		}

		[Test]
		[TestCase(42, "max(constant(42))")]
		[TestCase(0, "max(constant(0))")]
		public void TestMax(int number, string expected)
		{
			var max = new Max (new ConstantSeries(number));

			Assert.AreEqual (expected, max.GetExpression());
		}

		[Test]
		[TestCase(42, "median(constant(42))")]
		[TestCase(0, "median(constant(0))")]
		public void TestMedian(int number, string expected)
		{
			var median = new Median (new ConstantSeries(number));

			Assert.AreEqual (expected, median.GetExpression());
		}

		[Test]
		[TestCase(42, "min(constant(42))")]
		[TestCase(0, "min(constant(0))")]
		public void TestMin(int number, string expected)
		{
			var min = new Min (new ConstantSeries(number));

			Assert.AreEqual (expected, min.GetExpression());
		}

		[Test]
		[TestCase(42, "since(constant(42))")]
		[TestCase(0, "since(constant(0))")]
		public void TestSince(int number, string expected)
		{
			var since = new Since (new ConstantSeries(number));

			Assert.AreEqual (expected, since.GetExpression());
		}

		[Test]
		[TestCase(42, "streak(constant(42))")]
		[TestCase(0, "streak(constant(0))")]
		public void TestStreak(int number, string expected)
		{
			var streak = new Streak (new ConstantSeries(number));

			Assert.AreEqual (expected, streak.GetExpression());
		}

		[Test]
		[TestCase(42, "sum(constant(42))")]
		[TestCase(0, "sum(constant(0))")]
		public void TestSum(int number, string expected)
		{
			var sum = new Sum (new ConstantSeries(number));

			Assert.AreEqual (expected, sum.GetExpression());
		}

		[Test]
		[TestCase(42, 10, "forecastlr(constant(42),10)")]
		[TestCase( 0, 10, "forecastlr(constant(0),10)")]
		public void TestForecastLR(int number, int target, string expected)
		{
			var forecastlr = new ForecastLR (new ConstantSeries(number), new Scalar(target));

			Assert.AreEqual (expected, forecastlr.GetExpression());
		}

		[Test]
		[TestCase(42, 0.5, "percentile(constant(42),0.5)")]
		[TestCase( 0, 0.5, "percentile(constant(0),0.5)")]
		public void TestPercentile(int number, double level, string expected)
		{
			var percentile = new Percentile (new ConstantSeries(number), new Scalar(level));

			Assert.AreEqual (expected, percentile.GetExpression());
		}
	}
}
