using NUnit;
using NUnit.Framework;

using System;

using Watchstander.Plumbing;

namespace WatchstanderTests.Unit
{
	[TestFixture]
	public class UriValidatorTests
	{
		UriValidator getValidator()
		{
			return new UriValidator ();
		}
		
		[Test]
		[TestCase("localhost:8070")]
		public void TestRequiredProtocol (string url)
		{
			var validator = new UriValidator ();

			// needz moar type
			Assert.Throws<Exception> (() => validator.Validate (new Uri(url)));
		}
	}
}

