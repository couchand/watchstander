using NUnit;
using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Watchstander;
using Watchstander.Porcelain;

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

			var fruitCount = integrationTests.GetTimeSeries ("fruit", isBanana);

			var count = (long)Math.Floor (10 * myRandom.NextDouble ());

			fruitCount.Record (count);

			collector.Shutdown ();
		}

		[Test]
		public void ParallelExampleTest ()
		{
			var options = new  CollectorOptions (new Uri("http://localhost:8070"));

			var collector = Bosun.Collector (options)
				.WithTag ("host", "foobar")
				.WithName ("tests.usage");
				//.WithTagger<int> ("thread", i => i.ToString ())
				//.WithTagger<int> ("job", j => j.ToString ());

			var metric = collector.GetMetric<long> ("parallel");

			List<Timer> timers = new List<Timer> ();

			Parallel.For (0, 8, i => {

				var jobMetric = metric;//.WithTag<int> ("job", i);
				var job = new ParallelJob(jobMetric);
				var timer = new Timer(job.Callback, 1, 100, 100);

				timers.Add(timer);
			});

			Thread.Sleep (500);

			Parallel.ForEach(timers, t => {
				var handle = new ManualResetEvent(false);
				t.Dispose(handle);
				handle.WaitOne();
			});

			collector.Shutdown ();
		}

		public class ParallelJob
		{
			private readonly ICollectorMetric<long> metric;
			private int count;

			public ParallelJob(ICollectorMetric<long> metric)
			{
				this.metric = metric;
				this.count = 0;
			}

			public void Callback(object payload)
			{
				Console.WriteLine ("> job recording count " + count);
				var tagged = metric.WithTag<int> ("thread", Thread.CurrentThread.ManagedThreadId);

				tagged.Record (count);
				Interlocked.Exchange (ref count, count + 1);
			}
		}
	}
}

