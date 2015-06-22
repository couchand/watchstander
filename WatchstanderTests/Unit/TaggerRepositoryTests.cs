using NUnit;
using NUnit.Framework;

using System;
using System.Linq;

using Watchstander.Plumbing;

namespace WatchstanderTests.Unit
{
	[TestFixture]
	public class TaggerRepositoryTests
	{
		private TaggerRepository getRepo()
		{
			return new TaggerRepository();
		}

		[Test]
		public void TestAddStatic ()
		{
			var repo = getRepo ();

			repo.AddStatic ("fruit", "banana");

			var tagKeys = repo.GetAllTagKeys ();

			Assert.AreEqual (1, tagKeys.Count ());
			Assert.That (tagKeys.Contains ("fruit"));
		}
		
		[Test]
		public void TestAddExistingThrowsStatic()
		{
			var repo = getRepo ();
			
			repo.AddStatic ("fruit", "banana");
			
			Assert.Throws<Exception> (() => repo.AddStatic ("fruit", "orange"));
		}
		
		[Test]
		public void TestContainsStatic()
		{
			var repo = getRepo ();
			
			repo.AddStatic ("fruit", "banana");
			
			Assert.That (repo.ContainsStatic ("fruit"));
			Assert.False (repo.ContainsStatic ("vegetable"));
		}

		[Test]
		public void TestGetStatic ()
		{
			var repo = getRepo ();

			repo.AddStatic ("fruit", "banana");

			var fruit = repo.GetStatic ("fruit");

			Assert.AreEqual ("banana", fruit);
		}
		
		[Test]
		public void TestGetMissingThrowsStatic()
		{
			var repo = getRepo ();
			
			Assert.Throws<Exception> (() => repo.GetStatic ("foobar"));
		}

		[Test]
		public void TestAddDynamic ()
		{
			var repo = getRepo ();

			var tagger = new Tagger<bool> ("fruit", b => b ? "banana" : "apple");
			repo.AddDynamic (tagger);

			var tagKeys = repo.GetAllTagKeys ();

			Assert.AreEqual (1, tagKeys.Count ());
			Assert.That (tagKeys.Contains ("fruit"));
		}

		[Test]
		public void TestAddExistingThrowsDynamic()
		{
			var repo = getRepo ();
			
			var tagger = new Tagger<bool> ("fruit", b => b ? "banana" : "apple");
			repo.AddDynamic (tagger);

			var otherTagger = new Tagger<bool> ("fruit", b => b ? "orange" : "apple");

			Assert.Throws<Exception> (() => repo.AddDynamic (otherTagger));
		}

		[Test]
		public void TestContainsDynamic()
		{
			var repo = getRepo ();

			var tagger = new Tagger<bool> ("fruit", b => b ? "banana" : "apple");
			repo.AddDynamic (tagger);

			Assert.That (repo.ContainsDynamic<bool> ("fruit"));
			Assert.False (repo.ContainsDynamic<bool> ("vegetable"));
		}

		[Test]
		public void TestGetDynamic ()
		{
			var repo = getRepo ();

			var tagger = new Tagger<bool> ("fruit", b => b ? "banana" : "apple");
			repo.AddDynamic (tagger);

			var retrieved = repo.GetDynamic<bool> ("fruit");

			Assert.AreSame (tagger, retrieved);
		}

		[Test]
		public void TestGetMissingThrowsDynamic()
		{
			var repo = getRepo ();

			Assert.Throws<Exception> (() => repo.GetDynamic<bool> ("foobar"));
		}

		[Test]
		public void TestAddExistingThrowsStaticToDynamic()
		{
			var repo = getRepo ();

			repo.AddStatic ("fruit", "banana");

			var otherTagger = new Tagger<bool> ("fruit", b => b ? "orange" : "apple");

			Assert.Throws<Exception> (() => repo.AddDynamic (otherTagger));
		}

		[Test]
		public void TestContainsStaticToDynamic()
		{
			var repo = getRepo ();

			repo.AddStatic ("fruit", "banana");

			Assert.False (repo.ContainsDynamic<bool> ("fruit"));
		}

		[Test]
		public void TestGetMissingThrowsStaticToDynamic()
		{
			var repo = getRepo ();

			repo.AddStatic ("fruit", "banana");

			Assert.Throws<Exception> (() => repo.GetDynamic<bool> ("fruit"));
		}

		[Test]
		public void TestAddExistingThrowsDynamicToStatic()
		{
			var repo = getRepo ();

			var tagger = new Tagger<bool> ("fruit", b => b ? "banana" : "apple");
			repo.AddDynamic (tagger);

			Assert.Throws<Exception> (() => repo.AddStatic ("fruit", "mango"));
		}

		[Test]
		public void TestContainsDynamicToStatic()
		{
			var repo = getRepo ();

			var tagger = new Tagger<bool> ("fruit", b => b ? "banana" : "apple");
			repo.AddDynamic (tagger);

			Assert.False (repo.ContainsStatic ("fruit"));
		}

		[Test]
		public void TestGetMissingThrowsDynamicToStatic()
		{
			var repo = getRepo ();

			var tagger = new Tagger<bool> ("fruit", b => b ? "banana" : "apple");
			repo.AddDynamic (tagger);

			Assert.Throws<Exception> (() => repo.GetStatic ("fruit"));
		}

		[Test]
		public void TestContainsDynamicTypeMismatch()
		{
			var repo = getRepo ();

			var tagger = new Tagger<bool> ("fruit", b => b ? "banana" : "apple");
			repo.AddDynamic (tagger);

			Assert.Throws<Exception> (() => repo.ContainsDynamic<int> ("fruit"));
		}

		[Test]
		public void TestGetDynamicTypeMismatch ()
		{
			var repo = getRepo ();

			var tagger = new Tagger<bool> ("fruit", b => b ? "banana" : "apple");
			repo.AddDynamic (tagger);

			Assert.Throws<Exception> (() => repo.GetDynamic<int> ("fruit"));
		}

		[Test]
		public void TestContainsAnyStatic()
		{
			var repo = getRepo ();

			repo.AddStatic ("fruit", "banana");

			Assert.That (repo.ContainsAny ("fruit"));
			Assert.False (repo.ContainsAny ("vegetable"));
		}

		[Test]
		public void TestContainsAnyDynamic()
		{
			var repo = getRepo ();

			var tagger = new Tagger<bool> ("fruit", b => b ? "banana" : "apple");
			repo.AddDynamic (tagger);

			Assert.That (repo.ContainsAny ("fruit"));
			Assert.False (repo.ContainsAny ("vegetable"));
		}
	}
}

