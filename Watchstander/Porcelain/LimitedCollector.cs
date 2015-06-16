using System;
using System.Collections.Generic;
using Watchstander.Plumbing;
using Watchstander.Utilities;

namespace Watchstander.Porcelain
{
	public class LimitedCollector : ICollector
	{
		public RootCollector Root { get; }
		public string NamePrefix { get; }
		public IReadOnlyDictionary<string, string> Tags { get; }

		public readonly TaggerDictionary taggers;

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

		public LimitedCollector (RootCollector Root, string NamePrefix, IReadOnlyDictionary<string, string> Tags, TaggerDictionary taggers)
		{
			this.Root = Root;
			this.NamePrefix = NamePrefix;
			this.Tags = Tags;
			this.taggers = taggers;

			this.description = null;
			this.descriptionIsDirty = false;
		}

		public ICollector WithName(string namePrefix)
		{
			return CollectorFactory.LimitCollectorName (this, namePrefix + ".");
		}

		public ICollector WithNamePrefix(string namePrefix)
		{
			return CollectorFactory.LimitCollectorName (this, namePrefix);
		}

		public ICollector WithTag (string tagKey, string tagValue)
		{
			return CollectorFactory.LimitCollectorTags (this, tagKey, tagValue);
		}

		public ICollector WithTags (IReadOnlyDictionary<string, string> tags)
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

			return new CollectorMetric (this, name);
		}

		public ICollector WithTagger<TValue> (string tagKey, Func<TValue, string> tagger)
		{
			return CollectorFactory.LimitCollectorTags<TValue> (this, tagKey, tagger);
		}

		public ICollector WithTag<TValue> (string tagKey, TValue tagValue)
		{
			if (taggers == null || !taggers.Contains<TValue> (tagKey))
			{
				throw new ArgumentOutOfRangeException ("tagKey", tagKey, "You must provide a tagger.");
			}

			if (Tags != null && Tags.ContainsKey (tagKey))
			{
				throw new ArgumentException ("Tag already applied.", "tagKey");
			}

			var value = taggers.Get (tagKey, tagValue);

			return CollectorFactory.LimitCollectorTags (this, tagKey, value);
		}
	}
}

