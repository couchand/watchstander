using System;
using System.Collections.Generic;
using Watchstander.Plumbing;

namespace Watchstander.Porcelain
{
	public class RootCollector : ICollector
	{
		public string NamePrefix => "";
		public IReadOnlyList<string> TagKeys => null;
		public IReadOnlyDictionary<string, string> Tags => null;

		private string description;
		private bool descriptionIsDirty;

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

		public RootCollector ()
		{
			this.description = null;
			this.descriptionIsDirty = false;
		}

		public INameLimitable WithName(string namePrefix)
		{
			return CollectorFactory.LimitCollectorName (this, namePrefix + ".");
		}

		public INameLimitable WithNamePrefix(string namePrefix)
		{
			return CollectorFactory.LimitCollectorName (this, namePrefix);
		}

		public ITagLimitable WithTag (string tagKey, string tagValue)
		{
			return CollectorFactory.LimitCollectorTags (this, tagKey, tagValue);
		}

		public ITagLimitable WithTags (IReadOnlyDictionary<string, string> tags)
		{
			return CollectorFactory.LimitCollectorTags (this, tags);
		}

		public CollectorMetric GetMetric(string name)
		{
			// needz one tag
			throw new Exception ("you must provide at least one tag");
		}

		public ITagLimitable WithTagger<TValue>(string tagKey, Func<TValue, string> tagger)
		{
			return CollectorFactory.LimitCollectorTags<TValue> (this, tagKey, tagger);
		}

		public ITagLimitable WithTag<TValue> (string tagKey, TValue tagValue)
		{
			// needz tagger
			throw new Exception("you must provide a tagger");
		}
	}
}

