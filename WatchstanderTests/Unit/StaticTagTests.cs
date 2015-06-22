using NUnit;
using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;

using Watchstander.Plumbing;

namespace WatchstanderTests.Unit
{
	[TestFixture]
	public class StaticTagTests
	{
		private StaticTag getFruitTag()
		{
			return new StaticTag ("status");
		}

		[Test]
		public void TestAddValue ()
		{
			var tag = getFruitTag ();

			tag.AddValue ("banana");
			tag.AddValue ("apple");
			tag.AddValue ("mango");

			var values = tag.TagValues;

			Assert.AreEqual (3, values.Count ());
			Assert.That (values.Contains ("banana"));
			Assert.That (values.Contains ("apple"));
			Assert.That (values.Contains ("mango"));
		}

		[Test]
		public void TestAddValues ()
		{
			var tag = getFruitTag ();

			tag.AddValues (new List<string> { "banana", "apple", "mango" });

			var values = tag.TagValues;

			Assert.AreEqual (3, values.Count ());
			Assert.That (values.Contains ("banana"));
			Assert.That (values.Contains ("apple"));
			Assert.That (values.Contains ("mango"));
		}
	}
}

