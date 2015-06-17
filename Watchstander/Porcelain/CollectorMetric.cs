using System;
using System.Collections.Generic;
using Watchstander.Common;
using Watchstander.Plumbing;

namespace Watchstander.Porcelain
{
	public class CollectorMetric : IMetric, ITagLimitable, IDescribable
	{
		private readonly ICollector collector;
		private readonly string name;

		private string description;
		private bool descriptionIsDirty;

		public TagLimiter Limiter { get; }

		public IReadOnlyDictionary<string, string> Tags => Limiter.Tags;
		public IReadOnlyList<string> TagKeys => Limiter.TagKeys;
		public TaggerDictionary Taggers => Limiter.Taggers;

		public string Name => collector.NamePrefix + name;
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

		public Rate Rate => Rate.Unknown;
		public string Unit => "";

		public CollectorMetric (ICollector collector, string name, TagLimiter Limiter)
		{
			this.collector = collector;
			this.Limiter = Limiter;
			this.name = name;

			this.description = null;
			this.descriptionIsDirty = false;
		}

		public CollectorMetric (CollectorMetric copy, TagLimiter Limiter)
		{
			this.Limiter = Limiter;

			this.collector = copy.collector;
			this.name = copy.name;

			this.description = copy.description;
			this.descriptionIsDirty = copy.descriptionIsDirty;
		}

		public ITagLimitable WithTag (string tagKey, string tagValue)
		{
			return new CollectorMetric(this, Limiter.Add(tagKey, tagValue));
		}

		public ITagLimitable WithTags (IReadOnlyDictionary<string, string> tags)
		{
			return new CollectorMetric(this, Limiter.Add(tags));
		}

		public ITagLimitable WithTagger<TValue> (string tagKey, Func<TValue, string> tagger)
		{
			return null;
		}

		public ITagLimitable WithTag<TValue> (string tagKey, TValue tagValue)
		{
			return null;
		}
	}
}

