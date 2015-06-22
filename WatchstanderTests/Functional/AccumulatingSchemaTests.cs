using NUnit;
using NUnit.Framework;

using System;

using Watchstander.Plumbing;

namespace WatchstanderTests.Functional
{
	[TestFixture]
	public class AccumulatingSchemaTests
	{
		[Test]
		public void TestSetOnceSchema ()
		{
			var options = new AccumulatingSchemaOptions ();

			var schema = new AccumulatingSchema (options);

			var metric = schema.AddEntry ("foo.bar.baz");

			var wittyDescription = "The number of baz it took to bar.";
			metric.SetDescription (wittyDescription);

			var resolved = schema.Entries ["foo.bar.baz"];
			Assert.AreEqual (wittyDescription, resolved.Metric.Description);
			
			Assert.Throws<Exception> (() => metric.SetDescription ("How much we baz."));
		}

		[Test]
		public void TestAccumulatingSchema ()
		{
			var options = new AccumulatingSchemaOptions ();
			options.AllowMetadataUpdates = true;

			var schema = new AccumulatingSchema (options);

			var metric = schema.AddEntry ("foo.bar.baz");

			metric.SetDescription ("The number of baz it took to bar.");

			var wittyDescription = "How much we baz.";
			metric.SetDescription (wittyDescription);

			var resolved = schema.Entries ["foo.bar.baz"];
			Assert.AreEqual (wittyDescription, resolved.Metric.Description);
		}
	}
}

