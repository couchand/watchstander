using System;
using System.Collections.Generic;
using Watchstander.Common;
using Watchstander.Plumbing;

namespace Watchstander.Porcelain
{
	public class CollectorMetric<TData> : ICollectorMetric<TData>
	{
		private readonly RootCollector collector;
		private readonly AccumulatingSchemaEntry schemaEntry;
		private readonly string name;
		private readonly bool enabled;

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

		public string Description => schemaEntry.Metric.Description;
		public Rate Rate => schemaEntry.Metric.Rate;
		public string Unit => schemaEntry.Metric.Unit;

		public ICollectorMetric<TData> SetDescription(string description)
		{
			schemaEntry.SetDescription(description);
			return this;
		}

		public ICollectorMetric<TData> SetRate(Rate rate)
		{
			schemaEntry.SetRate (rate);
			return this;
		}

		public ICollectorMetric<TData> SetUnit(string unit)
		{
			schemaEntry.SetUnit(unit);
			return this;
		}

		internal CollectorMetric (RootCollector collector, AccumulatingSchemaEntry schemaEntry, string name, TagLimiter Limiter, bool enabled)
		{
			this.collector = collector;
			this.schemaEntry = schemaEntry;
			this.Limiter = Limiter;
			this.name = name;
			this.enabled = enabled;
		}

		private CollectorMetric (CollectorMetric<TData> copy, TagLimiter Limiter, bool? enabled = null)
		{
			this.Limiter = Limiter;

			this.collector = copy.collector;
			this.schemaEntry = copy.schemaEntry;
			this.name = copy.name;
			this.enabled = enabled ?? copy.enabled;
		}

		public ICollectorMetric<TData> Disabled()
		{
			return new CollectorMetric<TData>(this, Limiter, false);
		}

		public ICollectorMetric<TData> Reenabled()
		{
			return new CollectorMetric<TData>(this, Limiter, true);
		}

		public ICollectorMetric<TData> WithTag (string tagKey, string tagValue)
		{
			return new CollectorMetric<TData>(this, Limiter.Add(tagKey, tagValue));
		}

		public ICollectorMetric<TData> WithTags (IReadOnlyDictionary<string, string> tags)
		{
			return new CollectorMetric<TData>(this, Limiter.Add(tags));
		}

		public ICollectorMetric<TData> WithTagger<TTaggable> (string tagKey, Func<TTaggable, string> tagger)
		{
			return new CollectorMetric<TData>(this, Limiter.Add(tagKey, tagger));
		}

		public ICollectorMetric<TData> WithTag<TTaggable> (string tagKey, TTaggable tagValue)
		{
			return new CollectorMetric<TData> (this, Limiter.Resolve (tagKey, tagValue));
		}

		public ICollectorTimeSeries<TData> GetTimeSeries()
		{
			return new CollectorTimeSeries<TData>(collector, this, Tags, enabled);
		}
	}
}

