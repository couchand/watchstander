using System;
using System.Collections.Generic;
using Watchstander.Plumbing;
using Watchstander.Utilities;

namespace Watchstander.Porcelain
{
	public class LimitedCollector : ICollector
	{
		private RootCollector Root { get; }

		internal NameLimiter NameLimiter { get; }
		public string NamePrefix => NameLimiter.NamePrefix;

		internal TagLimiter TagLimiter { get; }
		public IReadOnlyDictionary<string, string> Tags => TagLimiter.Tags;
		public TaggerDictionary Taggers => TagLimiter.Taggers;

		public string Description { get; set; }

		public LimitedCollector (RootCollector Root, NameLimiter NameLimiter, TagLimiter TagLimiter)
		{
			this.Root = Root;
			this.NameLimiter = NameLimiter;
			this.TagLimiter = TagLimiter;
		}

		public ICollector WithName(string name)
		{
			return new LimitedCollector (Root, NameLimiter.Add(name), TagLimiter);
		}

		public ICollector WithNamePrefix(string namePrefix)
		{
			return new LimitedCollector (Root, NameLimiter.AddPrefix(namePrefix), TagLimiter);
		}

		public ICollector WithTag (string tagKey, string tagValue)
		{
			return new LimitedCollector(Root, NameLimiter, TagLimiter.Add(tagKey, tagValue));
		}

		public ICollector WithTags (IReadOnlyDictionary<string, string> tags)
		{
			return new LimitedCollector(Root, NameLimiter, TagLimiter.Add(Tags));
		}

		public ICollector WithTagger<TValue> (string tagKey, Func<TValue, string> tagger)
		{
			return new LimitedCollector(Root, NameLimiter, TagLimiter.Add (tagKey, tagger));
		}

		public ICollector WithTag<TValue> (string tagKey, TValue tagValue)
		{
			if (Taggers == null || !Taggers.Contains<TValue> (tagKey))
			{
				throw new ArgumentOutOfRangeException ("tagKey", tagKey, "You must provide a tagger.");
			}

			if (Tags != null && Tags.ContainsKey (tagKey))
			{
				throw new ArgumentException ("Tag already applied.", "tagKey");
			}

			var value = Taggers.Get (tagKey, tagValue);

			return WithTag (tagKey, value);
		}

		public ICollectorMetric GetMetric(string name)
		{
			return new CollectorMetric(Root, NameLimiter.Resolve(name), TagLimiter);
		}
	}
}

