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

		public INameLimitable WithName(string name)
		{
			var nameLimiter = new NameLimiter ().Add(name);
			return new LimitedCollector (this, nameLimiter, new TagLimiter());
		}

		public INameLimitable WithNamePrefix(string namePrefix)
		{
			var nameLimiter = new NameLimiter ().AddPrefix(namePrefix);
			return new LimitedCollector (this, nameLimiter, new TagLimiter());
		}

		public ITagLimitable WithTag (string tagKey, string tagValue)
		{
			return new LimitedCollector(this, new NameLimiter(), new TagLimiter ().Add(tagKey, tagValue));
		}

		public ITagLimitable WithTags (IReadOnlyDictionary<string, string> tags)
		{
			return new LimitedCollector (this, new NameLimiter(), new TagLimiter ().Add(tags));
		}

		public ITagLimitable WithTagger<TValue>(string tagKey, Func<TValue, string> tagger)
		{
			return new LimitedCollector (this, new NameLimiter(), new TagLimiter ().Add (tagKey, tagger));
		}

		public ITagLimitable WithTag<TValue> (string tagKey, TValue tagValue)
		{
			// needz tagger
			throw new Exception("you must provide a tagger");
		}

		public CollectorMetric GetMetric(string name)
		{
			// needz one tag
			throw new Exception ("you must provide at least one tag");
		}
	}
}

