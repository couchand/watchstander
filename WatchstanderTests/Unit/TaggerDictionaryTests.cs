using NUnit;
using NUnit.Framework;

using System;
using System.Collections.Generic;

using Watchstander.Common;
using Watchstander.Porcelain;

namespace WatchstanderTests.Unit
{
	[TestFixture]
	public class TaggerDictionaryTests
	{
		[Test]
		public void TestStoreValue ()
		{
			var dict = new TaggerDictionary ();

			dict.Add<bool> ("host", b => b ? "foobar" : "failed");
			dict.Add<int> ("port", i => {
				if (i == 42)
				{
					return "42";
				}
				return "0";
			});

			var host = dict.Get<bool>("host", true);
			var port = dict.Get<int>("port", 42);

			Assert.AreEqual("foobar", host);
			Assert.AreEqual("42", port);
		}
	}
}

