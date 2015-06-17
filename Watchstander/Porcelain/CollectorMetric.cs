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
			return new CollectorTimeSeries<TData>(collector, this, Tags);
		}

		public ICollectorTimeSeries<TData> GetTimeSeries<TData>(string tagKey, string tagValue)
		{
			var updated = Limiter.Add (tagKey, tagValue);
			return new CollectorTimeSeries<TData> (collector, this, updated.Tags);
		}

		public ICollectorTimeSeries<TData> GetTimeSeries<TData>(IReadOnlyDictionary<string, string> tags)
		{
			var updated = Limiter.Add (tags);
			return new CollectorTimeSeries<TData> (collector, this, updated.Tags);
		}

		public ICollectorTimeSeries<TData> GetTimeSeries<TData, TTaggable>(string tagKey, TTaggable tagValue)
		{
			var updated = Limiter.Resolve (tagKey, tagValue);
			return new CollectorTimeSeries<TData> (collector, this, updated.Tags);
		}
	}
}

