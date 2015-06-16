using System;
using System.Collections.Generic;

namespace Watchstander.Porcelain
{
	public interface ITagLimitable
	{
		IReadOnlyDictionary<string, string> Tags { get; }

		ICollector WithTag (string tagKey, string tagValue);
		ICollector WithTags (IReadOnlyDictionary<string, string> tags);

		ICollector WithTagger<TValue> (string tagKey, Func<TValue, string> tagger);
		ICollector WithTag<TValue> (string tagKey, TValue tagValue);
	}
}

