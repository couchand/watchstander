using System;
using System.Collections.Generic;
using Watchstander.Common;
using Watchstander.Plumbing;

namespace Watchstander.Porcelain
{
	public class CollectorMetric<TData> : ICollectorMetric<TData>
	{
		private readonly RootCollector collector;
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
		public string Description { get; set; }

		public Rate Rate => Rate.Unknown;
		public string Unit => "";

		internal CollectorMetric (RootCollector collector, string name, TagLimiter Limiter, bool enabled)
		{
			this.collector = collector;
			this.Limiter = Limiter;
			this.name = name;
			this.enabled = enabled;
		}

		private CollectorMetric (CollectorMetric<TData> copy, TagLimiter Limiter, bool? enabled = null)
		{
			this.Limiter = Limiter;

			this.collector = copy.collector;
			this.name = copy.name;
			this.enabled = enabled ?? copy.enabled;

			this.Description = copy.Description;
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

