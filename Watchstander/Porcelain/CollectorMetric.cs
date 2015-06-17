﻿using System;
using System.Collections.Generic;
using Watchstander.Common;
using Watchstander.Plumbing;

namespace Watchstander.Porcelain
{
	public class CollectorMetric : ICollectorMetric
	{
		private readonly RootCollector collector;
		private readonly string name;

		private string description;
		private bool descriptionIsDirty;

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

		public CollectorMetric (RootCollector collector, string name, TagLimiter Limiter)
		{
			this.collector = collector;
			this.Limiter = Limiter;
			this.name = name;

			this.description = null;
			this.descriptionIsDirty = false;
		}

		private CollectorMetric (CollectorMetric copy, TagLimiter Limiter)
		{
			this.Limiter = Limiter;

			this.collector = copy.collector;
			this.name = copy.name;

			this.description = copy.description;
			this.descriptionIsDirty = copy.descriptionIsDirty;
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
			throw new NotImplementedException ("collector metric needz tags");
		}
	}
}

