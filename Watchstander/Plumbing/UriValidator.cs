using System;
using System.Web;

namespace Watchstander.Plumbing
{
	public class UriValidator
	{
		public UriValidator (/*options??*/)
		{
		}

		public void Validate(Uri uri)
		{
			if (uri.Scheme != "http" && uri.Scheme != "https") {
				throw new Exception ("needz moar http");
			}
		}
	}
}

