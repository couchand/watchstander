using System;
using System.Collections.Generic;
using Watchstander.Plumbing;

namespace Watchstander.Porcelain
{
	public class RootCollector : ICollector
	{
		public string Description { get; set; }

		public RootCollector () {}

		public ICollector WithName(string name)
		{
			var nameLimiter = new NameLimiter ().Add(name);
			return new LimitedCollector (this, nameLimiter, new TagLimiter());
		}

		public ICollector WithNamePrefix(string namePrefix)
		{
			var nameLimiter = new NameLimiter ().AddPrefix(namePrefix);
			return new LimitedCollector (this, nameLimiter, new TagLimiter());
		}

		public ICollector WithTag (string tagKey, string tagValue)
		{
			return new LimitedCollector(this, new NameLimiter(), new TagLimiter ().Add(tagKey, tagValue));
		}

		public ICollector WithTags (IReadOnlyDictionary<string, string> tags)
		{
			return new LimitedCollector (this, new NameLimiter(), new TagLimiter ().Add(tags));
		}

		public ICollector WithTagger<TValue>(string tagKey, Func<TValue, string> tagger)
		{
			return new LimitedCollector (this, new NameLimiter(), new TagLimiter ().Add (tagKey, tagger));
		}

		public ICollector WithTag<TValue> (string tagKey, TValue tagValue)
		{
			// needz tagger
			throw new Exception("you must provide a tagger");
		}

		public ICollectorMetric GetMetric(string name)
		{
			// needz one tag
			throw new Exception ("you must provide at least one tag");
		}
	}
}

