using NUnit;
using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;

using Watchstander.Plumbing;

namespace WatchstanderTests.Unit
{
	[TestFixture]
	public class TaggerTests
	{
		private Tagger<int> getStatusTagger()
		{
			return new Tagger<int>("status", s => {
				if (s < 100) {
					return "unknown";
				}
				if (s < 200) {
					return "info";
				}
				if (s < 300) {
					return "ok";
				}
				if (s < 400) {
					return "redirect";
				}
				if (s < 500) {
					return "client_error";
				}
				return "server_error";
			});
		}

		[Test]
		public void TestAddValue ()
		{
			var tagger = getStatusTagger ();

			tagger.AddValue (200);
			tagger.AddValue (302);
			tagger.AddValue (304); // maps to the same value
			tagger.AddValue (404);
			tagger.AddValue (500);

			var values = tagger.TagValues;

			Assert.AreEqual (4, values.Count ());
			Assert.That (values.Contains ("ok"));
			Assert.That (values.Contains ("redirect"));
			Assert.That (values.Contains ("client_error"));
			Assert.That (values.Contains ("server_error"));
		}

		[Test]
		public void TestAddValues ()
		{
			var tagger = getStatusTagger ();

			tagger.AddValues (new List<int> { 100, 200, 400 });

			var values = tagger.TagValues;

			Assert.AreEqual (3, values.Count ());
			Assert.That (values.Contains ("info"));
			Assert.That (values.Contains ("ok"));
			Assert.That (values.Contains ("client_error"));
		}
	}
}

