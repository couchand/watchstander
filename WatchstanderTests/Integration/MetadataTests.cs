using NUnit;
using NUnit.Framework;

using System;
using System.Threading;

using Watchstander.Common;
using Watchstander.Plumbing;

using WatchstanderTests.Common;

namespace WatchstanderTests.Integration
{
	[TestFixture]
	public class MetadataTests
	{
		private Api GetApi()
		{
			var url = new Uri("http://localhost:8070");
			var api = new Api(new ApiOptions(url, new SerializerOptions()));

			GZip.Use(api);

			return api;
		}

		[Test]
		public void TestMetadataSender ()
		{
			var schema = new MockSchema ();
			schema.AddEntry ("tests.integration", "The metadata sender sends metadata automatically.", Rate.Gauge, "items");

			var options = new MetadataSenderOptions (schema, GetApi());
			options.Timeout = TimeSpan.FromMilliseconds (100);

			var sender = new MetadataSender (options);

			Thread.Sleep (300);

			sender.Shutdown ();
		}
	}
}

