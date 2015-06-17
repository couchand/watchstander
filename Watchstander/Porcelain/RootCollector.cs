﻿using System;
using System.Collections.Generic;
using Watchstander.Common;
using Watchstander.Plumbing;

namespace Watchstander.Porcelain
{
	public class RootCollector : ICollector, IDataPointConsumer<long>, IDataPointConsumer<float>
	{
		public string Description { get; set; }

		private IDataPointConsumer<long> longConsumer;
		private IDataPointConsumer<float> floatConsumer;

		public RootCollector (IDataPointConsumer<long> longConsumer, IDataPointConsumer<float> floatConsumer)
		{
			this.longConsumer = longConsumer;
			this.floatConsumer = floatConsumer;
		}

		public ICollector Disabled()
		{
			return new LimitedCollector(this, new NameLimiter(), new TagLimiter(), false);
		}

		public ICollector Reenabled()
		{
			// we can be sure it's never been disabled
			return this;
		}

		public ICollector WithName(string name)
		{
			var nameLimiter = new NameLimiter ().Add(name);
			return new LimitedCollector (this, nameLimiter, new TagLimiter(), true);
		}

		public ICollector WithNamePrefix(string namePrefix)
		{
			var nameLimiter = new NameLimiter ().AddPrefix(namePrefix);
			return new LimitedCollector (this, nameLimiter, new TagLimiter(), true);
		}

		public ICollector WithTag (string tagKey, string tagValue)
		{
			return new LimitedCollector(this, new NameLimiter(), new TagLimiter ().Add(tagKey, tagValue), true);
		}

		public ICollector WithTags (IReadOnlyDictionary<string, string> tags)
		{
			return new LimitedCollector (this, new NameLimiter(), new TagLimiter ().Add(tags), true);
		}

		public ICollector WithTagger<TTaggable>(string tagKey, Func<TTaggable, string> tagger)
		{
			return new LimitedCollector (this, new NameLimiter(), new TagLimiter ().Add (tagKey, tagger), true);
		}

		public ICollector WithTag<TTaggable> (string tagKey, TTaggable tagValue)
		{
			// needz tagger
			throw new Exception("you must provide a tagger");
		}

		public ICollectorMetric GetMetric(string name)
		{
			return new CollectorMetric (this, name, new TagLimiter (), true);
		}

		public void Consume(IEnumerable<IDataPoint<long>> values)
		{
			longConsumer.Consume(values);
		}

		public void Consume(IEnumerable<IDataPoint<float>> values)
		{
			floatConsumer.Consume(values);
		}
	}
}

