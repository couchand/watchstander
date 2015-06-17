using System;
using System.Collections.Generic;

namespace Watchstander.Porcelain
{
	public interface ITagLimitable
	{
		IReadOnlyList<string> TagKeys { get; }
		IReadOnlyDictionary<string, string> Tags { get; }

		ITagLimitable WithTag (string tagKey, string tagValue);
		ITagLimitable WithTags (IReadOnlyDictionary<string, string> tags);

		ITagLimitable WithTagger<TValue> (string tagKey, Func<TValue, string> tagger);
		ITagLimitable WithTag<TValue> (string tagKey, TValue tagValue);
	}
}

