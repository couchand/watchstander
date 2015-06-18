using NUnit;
using NUnit.Framework;

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

using Watchstander.Common;
using Watchstander.Plumbing;
using Watchstander.Porcelain;
using Watchstander.Utilities;

using WatchstanderTests.Common;

namespace WatchstanderTests.Unit
{
	[TestFixture]
	public class DataQueueTests
	{
		[Test]
		public void TestEnqueue ()
		{
			var queue = new DataQueue ();

			var tags = new Dictionary<string, string>{{ "host", "foobar" }};

			var longDataPoints = new List<IDataPoint<long>>
			{
				new DataTest<long>(
					"foo.bar.count",
					DateTime.UtcNow,
					42,
					tags.AsReadOnly()
				)
			};

			var floatDataPoints = new List<IDataPoint<float>>
			{
				new DataTest<float>(
					"foo.bar.baz",
					DateTime.UtcNow,
					1.2f,
					tags.AsReadOnly()
				)
			};

			queue.Consume (longDataPoints);

			queue.Consume (floatDataPoints);

			queue.Consume (longDataPoints);
			queue.Consume (longDataPoints);

			queue.Consume (floatDataPoints);

			queue.Consume (longDataPoints);

			queue.Consume (floatDataPoints);

			var longs = queue.TakeLongs (10);
			Assert.AreEqual (4, longs.Count ());

			foreach (var dataPoint in longs)
			{
				Assert.AreEqual ("foo.bar.count", dataPoint.Metric);
				Assert.AreEqual (42, dataPoint.Value);

				Assert.AreEqual (1, dataPoint.Tags.Count);
				Assert.That (dataPoint.Tags.ContainsKey ("host"));
				Assert.AreEqual ("foobar", dataPoint.Tags ["host"]);
			}

			var floats = queue.TakeFloats (10);
			Assert.AreEqual (3, floats.Count ());

			foreach (var dataPoint in floats)
			{
				Assert.AreEqual ("foo.bar.baz", dataPoint.Metric);
				Assert.AreEqual (1.2f, dataPoint.Value);

				Assert.AreEqual (1, dataPoint.Tags.Count);
				Assert.That (dataPoint.Tags.ContainsKey ("host"));
				Assert.AreEqual ("foobar", dataPoint.Tags ["host"]);
			}
		}

		public class Enqueuer<TData>
		{
			DataQueue queue;
			IEnumerable<IDataPoint<TData>> data;

			public Enqueuer(DataQueue queue, IEnumerable<IDataPoint<TData>> data)
			{
				this.queue = queue;
				this.data = data;
			}

			public void Enqueue()
			{
				var consumer = queue as IDataPointConsumer<TData>;
				consumer.Consume (data);
			}
		}

		[Test]
		public void TestEnqueueParallel ()
		{
			var queue = new DataQueue ();

			var tags = new Dictionary<string, string>{{ "host", "foobar" }};

			var longDataPoints = new List<IDataPoint<long>>
			{
				new DataTest<long>(
					"foo.bar.count",
					DateTime.UtcNow,
					42,
					tags.AsReadOnly()
				)
			};

			var floatDataPoints = new List<IDataPoint<float>>
			{
				new DataTest<float>(
					"foo.bar.baz",
					DateTime.UtcNow,
					1.2f,
					tags.AsReadOnly()
				)
			};

			var longEnqueuer = new Enqueuer<long> (queue, longDataPoints);
			var floatEnqueuer = new Enqueuer<float> (queue, floatDataPoints);

			var longThreadOne = new Thread (longEnqueuer.Enqueue);
			var floatThreadOne = new Thread (floatEnqueuer.Enqueue);

			var longThreadTwo = new Thread (longEnqueuer.Enqueue);
			var floatThreadTwo = new Thread (floatEnqueuer.Enqueue);

			var longThreadThree = new Thread (longEnqueuer.Enqueue);
			var floatThreadThree = new Thread (floatEnqueuer.Enqueue);

			var longThreadFour = new Thread (longEnqueuer.Enqueue);

			longThreadOne.Start ();

			floatThreadOne.Start ();

			longThreadTwo.Start ();
			longThreadThree.Start ();

			floatThreadTwo.Start ();

			longThreadFour.Start ();

			floatThreadThree.Start ();

			longThreadOne.Join ();

			floatThreadOne.Join ();

			longThreadTwo.Join ();
			longThreadThree.Join ();

			floatThreadTwo.Join ();

			longThreadFour.Join ();

			floatThreadThree.Join ();

			var longs = queue.TakeLongs (10);
			Assert.AreEqual (4, longs.Count ());

			foreach (var dataPoint in longs)
			{
				Assert.AreEqual ("foo.bar.count", dataPoint.Metric);
				Assert.AreEqual (42, dataPoint.Value);

				Assert.AreEqual (1, dataPoint.Tags.Count);
				Assert.That (dataPoint.Tags.ContainsKey ("host"));
				Assert.AreEqual ("foobar", dataPoint.Tags ["host"]);
			}

			var floats = queue.TakeFloats (10);
			Assert.AreEqual (3, floats.Count ());

			foreach (var dataPoint in floats)
			{
				Assert.AreEqual ("foo.bar.baz", dataPoint.Metric);
				Assert.AreEqual (1.2f, dataPoint.Value);

				Assert.AreEqual (1, dataPoint.Tags.Count);
				Assert.That (dataPoint.Tags.ContainsKey ("host"));
				Assert.AreEqual ("foobar", dataPoint.Tags ["host"]);
			}
		}
	}
}

