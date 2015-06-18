using NUnit;
using NUnit.Framework;

using System;

using Watchstander;

namespace WatchstanderTests.Integration
{
	[TestFixture]
	public class UsageExample
	{
		Random myRandom = new Random();

		[Test]
		public void UsageExampleTest ()
		{
			var options = new  CollectorOptions (new Uri("http://localhost:8070"));

			var collector = Bosun.Collector (options)
				.WithTag ("host", "foobar")
				.WithName ("tests");
			
			var integrationTests = collector.GetMetric<long> ("integration")
 				.WithTagger<bool> ("fruit", b => b ? "banana" : "apple");

			// i like bananas
			var isBanana = myRandom.NextDouble() > 0.3 ? true : false;

			var fruitCount = integrationTests.GetTimeSeries<bool> ("fruit", isBanana);

			var count = (long)Math.Floor (10 * myRandom.NextDouble ());

			fruitCount.Record (count);

			collector.Shutdown ();
		}
	}
}

