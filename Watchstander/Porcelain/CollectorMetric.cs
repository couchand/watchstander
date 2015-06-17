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

		private CollectorMetric (CollectorMetric copy, TagLimiter Limiter, bool? enabled = null)
		{
			this.Limiter = Limiter;

			this.collector = copy.collector;
			this.name = copy.name;
			this.enabled = enabled ?? copy.enabled;

			this.Description = copy.Description;
		}

		public ICollectorMetric Disabled()
		{
			return new CollectorMetric(this, Limiter, false);
		}

		public ICollectorMetric Reenabled()
		{
			return new CollectorMetric(this, Limiter, true);
		}

		public ICollectorMetric WithTag (string tagKey, string tagValue)
		{
			return new CollectorMetric(this, Limiter.Add(tagKey, tagValue));
		}

		public ICollectorMetric WithTags (IReadOnlyDictionary<string, string> tags)
		{
			return new CollectorMetric(this, Limiter.Add(tags));
		}

		public ICollectorMetric WithTagger<TTaggable> (string tagKey, Func<TTaggable, string> tagger)
		{
			return new CollectorMetric(this, Limiter.Add(tagKey, tagger));
		}

		public ICollectorMetric WithTag<TTaggable> (string tagKey, TTaggable tagValue)
		{
			return new CollectorMetric (this, Limiter.Resolve (tagKey, tagValue));
		}

		public ICollectorTimeSeries<TData> GetTimeSeries<TData>()
		{
			return new CollectorTimeSeries<TData>(collector, this, Tags, enabled);
		}

		public ICollectorTimeSeries<TData> GetTimeSeries<TData>(string tagKey, string tagValue)
		{
			var updated = Limiter.Add (tagKey, tagValue);
			return new CollectorTimeSeries<TData> (collector, this, updated.Tags, enabled);
		}

		public ICollectorTimeSeries<TData> GetTimeSeries<TData>(IReadOnlyDictionary<string, string> tags)
		{
			var updated = Limiter.Add (tags);
			return new CollectorTimeSeries<TData> (collector, this, updated.Tags, enabled);
		}

		public ICollectorTimeSeries<TData> GetTimeSeries<TData, TTaggable>(string tagKey, TTaggable tagValue)
		{
			var updated = Limiter.Resolve (tagKey, tagValue);
			return new CollectorTimeSeries<TData> (collector, this, updated.Tags, enabled);
		}
	}
}

