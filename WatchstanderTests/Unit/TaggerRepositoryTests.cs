using NUnit;
using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;

using Watchstander.Plumbing;

namespace WatchstanderTests.Unit
{
	[TestFixture]
	public class TaggerRepositoryTests
	{
		internal static TaggerRepository getRepo()
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

			var tag = repo.GetStatic ("fruit");

			Assert.AreEqual (1, tag.TagValues.Count ());
			Assert.That (tag.TagValues.Contains ("banana"));
		}

		[Test]
		public void TestAddRangeStatic ()
		{
			var repo = getRepo ();

			repo.AddStatic ("fruit", new List<string>{ "banana", "apple" });

			var tagKeys = repo.GetAllTagKeys ();

			Assert.AreEqual (1, tagKeys.Count ());
			Assert.That (tagKeys.Contains ("fruit"));

			var tag = repo.GetStatic ("fruit");

			Assert.AreEqual (2, tag.TagValues.Count ());
			Assert.That (tag.TagValues.Contains ("banana"));
			Assert.That (tag.TagValues.Contains ("apple"));
		}
		
		[Test]
		public void TestAddExistingAppendsStatic()
		{
			var repo = getRepo ();
			
			repo.AddStatic ("fruit", "banana");
			repo.AddStatic ("fruit", "apple");
			repo.AddStatic ("fruit", "orange");

			var tagKeys = repo.GetAllTagKeys ();

			Assert.AreEqual (1, tagKeys.Count ());
			Assert.That (tagKeys.Contains ("fruit"));

			var tag = repo.GetStatic ("fruit");

			Assert.AreEqual (3, tag.TagValues.Count ());
			Assert.That (tag.TagValues.Contains ("banana"));
			Assert.That (tag.TagValues.Contains ("apple"));
			Assert.That (tag.TagValues.Contains ("orange"));
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

			Assert.AreEqual ("fruit", fruit.TagKey);
			Assert.AreEqual (1, fruit.TagValues.Count ());
			Assert.AreEqual ("banana", fruit.TagValues.First ());
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

	[TestFixture]
	public class ReadOnlyTaggerRepositoryTests
	{
		private TaggerRepository getMutableRepo()
		{
			return TaggerRepositoryTests.getRepo ();
		}

		private ReadOnlyTaggerRepository getRepo()
		{
			return getRepo (new TaggerRepository ());
		}

		private ReadOnlyTaggerRepository getRepo(TaggerRepository repo)
		{
			return repo.AsReadOnly();
		}

		[Test]
		public void TestAddThrowsStatic ()
		{
			var repo = getRepo ();

			Assert.Throws<Exception>(() => repo.AddStatic ("fruit", "banana"));
		}

		[Test]
		public void TestAddRangeThrowsStatic ()
		{
			var repo = getRepo ();

			Assert.Throws<Exception> (() => repo.AddStatic ("fruit", new List<string>{ "banana", "apple" }));
		}

		[Test]
		public void TestAddExistingAppendsThrowsStatic()
		{
			var repo = getMutableRepo ();

			repo.AddStatic ("fruit", "banana");

			var readOnly = repo.AsReadOnly ();

			Assert.Throws<Exception>(() => readOnly.AddStatic ("fruit", "orange"));
		}

		[Test]
		public void TestContainsStatic()
		{
			var repo = getMutableRepo ();

			repo.AddStatic ("fruit", "banana");

			var readOnly = repo.AsReadOnly ();

			Assert.That (readOnly.ContainsStatic ("fruit"));
			Assert.False (readOnly.ContainsStatic ("vegetable"));
		}

		[Test]
		public void TestGetStatic ()
		{
			var repo = getMutableRepo ();

			repo.AddStatic ("fruit", "banana");

			var readOnly = repo.AsReadOnly ();

			var fruit = readOnly.GetStatic ("fruit");

			Assert.AreEqual ("fruit", fruit.TagKey);
			Assert.AreEqual (1, fruit.TagValues.Count ());
			Assert.AreEqual ("banana", fruit.TagValues.First ());
		}

		[Test]
		public void TestGetMissingThrowsStatic()
		{
			var repo = getRepo ();

			Assert.Throws<Exception> (() => repo.GetStatic ("foobar"));
		}

		[Test]
		public void TestAddThrowsDynamic ()
		{
			var repo = getRepo ();

			var tagger = new Tagger<bool> ("fruit", b => b ? "banana" : "apple");
			Assert.Throws<Exception>(() => repo.AddDynamic (tagger));
		}

		[Test]
		public void TestAddExistingThrowsDynamic()
		{
			var repo = getMutableRepo ();

			var tagger = new Tagger<bool> ("fruit", b => b ? "banana" : "apple");
			repo.AddDynamic (tagger);

			var readOnly = repo.AsReadOnly ();

			var otherTagger = new Tagger<bool> ("fruit", b => b ? "orange" : "apple");

			Assert.Throws<Exception> (() => readOnly.AddDynamic (otherTagger));
		}

		[Test]
		public void TestContainsDynamic()
		{
			var repo = getMutableRepo ();

			var tagger = new Tagger<bool> ("fruit", b => b ? "banana" : "apple");
			repo.AddDynamic (tagger);

			var readOnly = repo.AsReadOnly ();

			Assert.That (readOnly.ContainsDynamic<bool> ("fruit"));
			Assert.False (readOnly.ContainsDynamic<bool> ("vegetable"));
		}

		[Test]
		public void TestGetDynamic ()
		{
			var repo = getMutableRepo ();

			var tagger = new Tagger<bool> ("fruit", b => b ? "banana" : "apple");
			repo.AddDynamic (tagger);

			var readOnly = repo.AsReadOnly ();

			var retrieved = readOnly.GetDynamic<bool> ("fruit");

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
			var repo = getMutableRepo ();

			repo.AddStatic ("fruit", "banana");

			var otherTagger = new Tagger<bool> ("fruit", b => b ? "orange" : "apple");

			var readOnly = repo.AsReadOnly ();

			Assert.Throws<Exception> (() => readOnly.AddDynamic (otherTagger));
		}

		[Test]
		public void TestContainsStaticToDynamic()
		{
			var repo = getMutableRepo ();

			repo.AddStatic ("fruit", "banana");

			var readOnly = repo.AsReadOnly ();

			Assert.False (readOnly.ContainsDynamic<bool> ("fruit"));
		}

		[Test]
		public void TestGetMissingThrowsStaticToDynamic()
		{
			var repo = getMutableRepo ();

			repo.AddStatic ("fruit", "banana");

			var readOnly = repo.AsReadOnly ();

			Assert.Throws<Exception> (() => readOnly.GetDynamic<bool> ("fruit"));
		}

		[Test]
		public void TestAddExistingThrowsDynamicToStatic()
		{
			var repo = getMutableRepo ();

			var tagger = new Tagger<bool> ("fruit", b => b ? "banana" : "apple");
			repo.AddDynamic (tagger);

			var readOnly = repo.AsReadOnly ();

			Assert.Throws<Exception> (() => readOnly.AddStatic ("fruit", "mango"));
		}

		[Test]
		public void TestContainsDynamicToStatic()
		{
			var repo = getMutableRepo ();

			var tagger = new Tagger<bool> ("fruit", b => b ? "banana" : "apple");
			repo.AddDynamic (tagger);

			var readOnly = repo.AsReadOnly ();

			Assert.False (readOnly.ContainsStatic ("fruit"));
		}

		[Test]
		public void TestGetMissingThrowsDynamicToStatic()
		{
			var repo = getMutableRepo ();

			var tagger = new Tagger<bool> ("fruit", b => b ? "banana" : "apple");
			repo.AddDynamic (tagger);

			var readOnly = repo.AsReadOnly ();

			Assert.Throws<Exception> (() => readOnly.GetStatic ("fruit"));
		}

		[Test]
		public void TestContainsDynamicTypeMismatch()
		{
			var repo = getMutableRepo ();

			var tagger = new Tagger<bool> ("fruit", b => b ? "banana" : "apple");
			repo.AddDynamic (tagger);

			var readOnly = repo.AsReadOnly ();

			Assert.Throws<Exception> (() => readOnly.ContainsDynamic<int> ("fruit"));
		}

		[Test]
		public void TestGetDynamicTypeMismatch ()
		{
			var repo = getMutableRepo ();

			var tagger = new Tagger<bool> ("fruit", b => b ? "banana" : "apple");
			repo.AddDynamic (tagger);

			var readOnly = repo.AsReadOnly ();

			Assert.Throws<Exception> (() => readOnly.GetDynamic<int> ("fruit"));
		}

		[Test]
		public void TestContainsAnyStatic()
		{
			var repo = getMutableRepo ();

			repo.AddStatic ("fruit", "banana");

			var readOnly = repo.AsReadOnly ();

			Assert.That (readOnly.ContainsAny ("fruit"));
			Assert.False (readOnly.ContainsAny ("vegetable"));
		}

		[Test]
		public void TestContainsAnyDynamic()
		{
			var repo = getMutableRepo ();

			var tagger = new Tagger<bool> ("fruit", b => b ? "banana" : "apple");
			repo.AddDynamic (tagger);

			var readOnly = repo.AsReadOnly ();

			Assert.That (readOnly.ContainsAny ("fruit"));
			Assert.False (readOnly.ContainsAny ("vegetable"));
		}
	}
}

