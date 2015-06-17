using System;
using System.Collections.Generic;
using Watchstander.Plumbing;
using Watchstander.Utilities;

namespace Watchstander.Porcelain
{
	public class LimitedCollector : ICollector
	{
		public RootCollector Root { get; }

		public NameLimiter NameLimiter { get; }
		public string NamePrefix => NameLimiter.NamePrefix;

		public TagLimiter TagLimiter { get; }
		public IReadOnlyDictionary<string, string> Tags => TagLimiter.Tags;
		public IReadOnlyList<string> TagKeys => TagLimiter.TagKeys;
		public TaggerDictionary Taggers => TagLimiter.Taggers;

		private string description;
		private bool descriptionIsDirty;

		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				descriptionIsDirty = true;
				description = value;
			}
		}

		public LimitedCollector (RootCollector Root, NameLimiter NameLimiter, TagLimiter TagLimiter)
		{
			this.Root = Root;
			this.NameLimiter = NameLimiter;
			this.TagLimiter = TagLimiter;

			this.description = null;
			this.descriptionIsDirty = false;
		}

		public INameLimitable WithName(string namePrefix)
		{
			return CollectorFactory.LimitCollectorName (this, namePrefix + ".");
		}

		public INameLimitable WithNamePrefix(string namePrefix)
		{
			return CollectorFactory.LimitCollectorName (this, namePrefix);
		}

		public ITagLimitable WithTag (string tagKey, string tagValue)
		{
			return CollectorFactory.LimitCollectorTags (this, tagKey, tagValue);
		}

		public ITagLimitable WithTags (IReadOnlyDictionary<string, string> tags)
		{
			return CollectorFactory.LimitCollectorTags (this, tags);
		}

		public CollectorMetric GetMetric(string name)
		{
			if (Tags == null || Tags.Count == 0)
			{
				// needz one tag
				throw new Exception ("you must provide at least one tag");
			}

			return CollectorFactory.GetLimitedMetric(this, name);
		}

		public ITagLimitable WithTagger<TValue> (string tagKey, Func<TValue, string> tagger)
		{
			return CollectorFactory.LimitCollectorTags<TValue> (this, tagKey, tagger);
		}

		public ITagLimitable WithTag<TValue> (string tagKey, TValue tagValue)
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

			return CollectorFactory.LimitCollectorTags (this, tagKey, value);
		}
	}
}

