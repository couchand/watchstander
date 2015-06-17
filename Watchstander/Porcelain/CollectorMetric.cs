using System;
using System.Collections.Generic;
using Watchstander.Common;
using Watchstander.Plumbing;

namespace Watchstander.Porcelain
{
	public class CollectorMetric : ICollectorMetric
	{
		private readonly RootCollector collector;
		private readonly string name;

		internal TagLimiter Limiter { get; }

		public IReadOnlyDictionary<string, string> Tags => Limiter.Tags;
		public TaggerDictionary Taggers => Limiter.Taggers;

		public IReadOnlyList<string> TagKeys
		{
			get
			{
				throw new NotImplementedException("needz moar schema");
			}
		}

		public string Name => name;
		public string Description { get; set; }

		public Rate Rate => Rate.Unknown;
		public string Unit => "";

		public CollectorMetric (RootCollector collector, string name, TagLimiter Limiter)
		{
			this.collector = collector;
			this.Limiter = Limiter;
			this.name = name;
		}

		private CollectorMetric (CollectorMetric copy, TagLimiter Limiter)
		{
			this.Limiter = Limiter;

			this.collector = copy.collector;
			this.name = copy.name;

			this.Description = copy.Description;
		}

		public ICollectorMetric WithTag (string tagKey, string tagValue)
		{
			return new CollectorMetric(this, Limiter.Add(tagKey, tagValue));
		}

		public ICollectorMetric WithTags (IReadOnlyDictionary<string, string> tags)
		{
			return new CollectorMetric(this, Limiter.Add(tags));
		}

		public ICollectorMetric WithTagger<TValue> (string tagKey, Func<TValue, string> tagger)
		{
			return new CollectorMetric(this, Limiter.Add(tagKey, tagger));
		}

		public ICollectorMetric WithTag<TValue> (string tagKey, TValue tagValue)
		{
			return new CollectorMetric (this, Limiter.Resolve (tagKey, tagValue));
		}

		public ICollectorTimeSeries GetTimeSeries()
		{
			// TODO: check for schema-completeness
			return new CollectorTimeSeries(this, Tags);
		}
	}
}

