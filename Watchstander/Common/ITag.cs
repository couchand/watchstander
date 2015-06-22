using System;
using System.Collections.Generic;

namespace Watchstander.Common
{
	public interface ITag
	{
		string TagKey { get; }

		IEnumerable<string> TagValues { get; }
	}
}

