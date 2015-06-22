using System;
using System.Collections.Generic;
using System.Linq;
using Watchstander.Common;
using Watchstander.Plumbing;

namespace Watchstander.Porcelain
{
	public class RootCollector : ICollector, IPipelineElement
	{
		private CollectorContext context;

		public RootCollector (CollectorContext context)
		{
			this.context = context;
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
			context.Disposables.Dispose ();
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

		public AccumulatingSchemaEntry GetSchemaEntry<TData>(string metricName)
		{
			return context.Schema.GetEntry (metricName);
		}

		public ICollectorMetric<TData> GetMetric<TData>(string name)
		{
			var entry = GetSchemaEntry<TData> (name);
			return new CollectorMetric<TData> (this, entry, name, new TagLimiter (), true);
		}

		public void Consume(IEnumerable<IDataPoint<long>> values)
		{
			Console.WriteLine ("RootCollector consuming " + values.Count() + " data points");
			context.DataConsumer.Consume(values);
		}

		public void Consume(IEnumerable<IDataPoint<float>> values)
		{
			Console.WriteLine ("RootCollector consuming " + values.Count() + " data points");
			context.DataConsumer.Consume(values);
		}
	}
}

