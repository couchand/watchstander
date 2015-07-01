using NUnit;
using NUnit.Framework;

using System;
using System.Collections.Generic;

using Watchstander.Expressions;

namespace WatchstanderTests.Functional
{
	[TestFixture]
	public class ExpressionTests
	{
		[Test]
		[TestCase(1, "cpu.usage")]
		[TestCase(2, "cpu.usage")]
		[TestCase(3, "cpu.usage")]
		[TestCase(10, "cpu.usage")]
		[TestCase(10, "cpu.usage{host=*}")]
		[TestCase(10, "cpu.usage{host=intweb}")]
		public void TestBuildBasicQuery (int hours, string metric)
		{
			var start = new RelativeDuration (TimeSpan.FromHours (hours));
			var end = new CurrentTime ();

			var query = new QueryBuilder (metric);

			var q = new Q (query, start, end);

			var expected = String.Format ("q(\"sum:{0}\",\"{1}h\",\"\")", metric, hours);

			Assert.AreEqual (expected, q.GetExpression ());
		}

		[Test]
		public void TestBuildComplexQuery ()
		{
			var start = new RelativeDuration (TimeSpan.FromHours (10));
			var end = new CurrentTime ();

			var downsampleWindow = new RelativeDuration (TimeSpan.FromHours (1));

			var query = new QueryBuilder (
				new MetricQuery (
					"os.cpu",
					new Dictionary<string, string> {
						{ "host", "foobar" },
						{ "type", "*" }
					}
				),
				Aggregators.Max,
				new Downsampler (
					downsampleWindow,
					Aggregators.Sum
				),
				new RateOptions (
					33,
					32
				)
			);

			var q = new Q (query, start, end);

			var expected = "q(\"max:1h-sum:rate{counter,33,32}:os.cpu{host=foobar,type=*}\",\"10h\",\"\")";

			Assert.AreEqual (expected, q.GetExpression ());
		}

		[Test]
		public void TestExpressionMath ()
		{
			var span = new RelativeDuration(TimeSpan.FromHours (1));
			var ending = new CurrentTime ();

			var usage = new Q(new QueryBuilder ("api.usage", Aggregators.Max), span, ending);
			var limit = new Q(new QueryBuilder ("api.limit", Aggregators.Max), span, ending);

			var percentage = usage / new Avg(limit) * 100;

			Assert.AreEqual (
				"q(\"max:api.usage\",\"1h\",\"\")/avg(q(\"max:api.limit\",\"1h\",\"\"))*100",
				percentage.GetExpression()
			);
		}
	}
}

