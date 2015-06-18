using NUnit;
using NUnit.Framework;

using System;

using Watchstander;

namespace WatchstanderTests.Integration
{
	[TestFixture]
	public class UsageExample
	{
		[Test]
		public void UsageExampleTest ()
		{
			var options = new  CollectorOptions (new Uri("http://localhost:8070"));

			var collector = Bosun.Collector (options)
				.WithTag ("host", "foobar")
				.WithName ("tests");
			
			var integrationTests = collector.GetMetric ("integration")
 				.WithTagger<bool> ("fruit", b => b ? "banana" : "apple");

			var bananas = integrationTests.GetTimeSeries<long, bool> ("fruit", true);

			bananas.Record (10);

			collector.Shutdown ();
		}
	}
}

