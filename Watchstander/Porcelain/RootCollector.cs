using System;
using System.Collections.Generic;
using Watchstander.Common;
using Watchstander.Plumbing;

namespace Watchstander.Porcelain
{
	public class RootCollector : ICollector, IPipelineElement
	{
		public string Description { get; set; }

		private IPipelineElement consumer;
		private IDisposable timer;

		public RootCollector (IPipelineElement consumer, IDisposable timer)
		{
			this.consumer = consumer;
			this.timer = timer;
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

		public void Shutdown()
		{
			timer.Dispose ();
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
			consumer.Consume(values);
		}

		public void Consume(IEnumerable<IDataPoint<float>> values)
		{
			consumer.Consume(values);
		}
	}
}

