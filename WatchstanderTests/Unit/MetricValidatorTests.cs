using NUnit;
using NUnit.Framework;

using System;
using System.Collections.Generic;

using Watchstander.Options;
using Watchstander.Common;
using Watchstander.Plumbing;

namespace WatchstanderTests.Unit
{
	[TestFixture]
	public class MetricValidatorTests
	{
		[Test]
		[TestCase("foo$bar")]
		[TestCase("foo-bar")]
		[TestCase("foo bar")]
		[TestCase("foo/bar")]
		public void TestDisallowCharacters (string metricName)
		{
			var validator = new MetricValidator (new MetricValidatorOptions ());

			Assert.Throws<Exception> (() => validator.ValidateName (metricName));
		}

		[Test]
		[TestCase("foo.Bar")]
		[TestCase("Foo.BAR")]
		[TestCase("FOO.BAR")]
		[TestCase("foo.baR")]
		public void TestExpectLowerCase (string metricName)
		{
			var validator = new MetricValidator (new MetricValidatorOptions ());

			Assert.Throws<Exception> (() => validator.ValidateName (metricName));
		}

		[Test]
		[TestCase("foo.Bar")]
		[TestCase("Foo.BAR")]
		[TestCase("foo.bar")]
		[TestCase("foo.baR")]
		public void TestExpectUpperCase (string metricName)
		{
			var opts = new MetricValidatorOptions ();
			opts.RequireCase = RequireCase.Upper;
			var validator = new MetricValidator (opts);

			Assert.Throws<Exception> (() => validator.ValidateName (metricName));
		}

		[Test]
		[TestCase("foo.bar")]
		[TestCase("foo.bar.baz.qux")]
		[TestCase("foobarbazqux")]
		[TestCase("foo")]
		public void TestValidMetricNames(string metricName)
		{
			var validator = new MetricValidator (new MetricValidatorOptions ());

			Assert.DoesNotThrow (() => validator.ValidateName (metricName));
		}
		
		[Test]
		[TestCase("foo!bar")]
		[TestCase("foo@bar")]
		[TestCase("foo#bar")]
		[TestCase("foo$bar")]
		[TestCase("foo%bar")]
		public void TestAllowCharacters(string metricName)
		{
			var opts = new MetricValidatorOptions ();
			opts.AllowedCharacters = "!@#$%";
			var validator = new MetricValidator (opts);
			
			Assert.DoesNotThrow (() => validator.ValidateName (metricName));
		}

		MetricValidator getLengthValidator()
		{
			var opts = new MetricValidatorOptions ();
			opts.NameLengthMinimum = 3;
			opts.NameLengthMaximum = 9;
			return new MetricValidator (opts);
		}

		[Test]
		[TestCase("")]
		[TestCase("f")]
		[TestCase("fo")]
		public void TestTooShort(string metricName)
		{
			var validator = getLengthValidator ();
			Assert.Throws<Exception> (() => validator.ValidateName (metricName));
		}

		[Test]
		[TestCase("foobarbazq")]
		[TestCase("foobarbazqu")]
		[TestCase("foobarbazqux")]
		public void TestTooLong(string metricName)
		{
			var validator = getLengthValidator ();
			Assert.Throws<Exception> (() => validator.ValidateName (metricName));
		}

		[Test]
		[TestCase("foo")]
		[TestCase("foob")]
		[TestCase("fooba")]
		[TestCase("foobar")]
		[TestCase("foobarb")]
		[TestCase("foobarba")]
		[TestCase("foobarbaz")]
		public void TestJustRight(string metricName)
		{
			var validator = getLengthValidator ();
			Assert.DoesNotThrow (() => validator.ValidateName (metricName));
		}

		MetricValidator getSegmentsValidator()
		{
			var opts = new MetricValidatorOptions ();
			opts.NameSegmentsMinimum = 2;
			opts.NameSegmentsMaximum = 3;
			return new MetricValidator (opts);
		}

		[Test]
		[TestCase("foo")]
		public void TestTooFewSegments(string metricName)
		{
			var validator = getSegmentsValidator ();
			Assert.Throws<Exception> (() => validator.ValidateName (metricName));
		}

		[Test]
		[TestCase("foo.bar.baz.qux")]
		public void TestTooManySegments(string metricName)
		{
			var validator = getSegmentsValidator ();
			Assert.Throws<Exception> (() => validator.ValidateName (metricName));
		}

		[Test]
		[TestCase("foo.bar")]
		[TestCase("foo.bar.baz")]
		public void TestJustRightSegments(string metricName)
		{
			var validator = getSegmentsValidator ();
			Assert.DoesNotThrow (() => validator.ValidateName (metricName));
		}
	}
}

