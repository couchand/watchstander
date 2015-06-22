using System;
using System.Collections.Generic;
using Watchstander.Plumbing;
using Watchstander.Utilities;

namespace Watchstander.Porcelain
{
	public class LimitedCollector : ICollector
	{
		private RootCollector Root { get; }

		internal NameLimiter NameLimiter { get; }
		public string NamePrefix => NameLimiter.NamePrefix;

		internal TagLimiter TagLimiter { get; }
		public IReadOnlyDictionary<string, string> Tags => TagLimiter.Tags;
		public TaggerDictionary Taggers => TagLimiter.Taggers;

		public string Description { get; set; }

		internal bool Enabled { get; }

		internal LimitedCollector (RootCollector Root, NameLimiter NameLimiter, TagLimiter TagLimiter, bool Enabled)
		{
			this.Root = Root;
			this.NameLimiter = NameLimiter;
			this.TagLimiter = TagLimiter;
			this.Enabled = Enabled;
		}

		public ICollector Disabled()
		{
			return new LimitedCollector(Root, NameLimiter, TagLimiter, false);
		}

		public ICollector Reenabled()
		{
			return new LimitedCollector(Root, NameLimiter, TagLimiter, true);
		}

		public void Shutdown()
		{
			Root.Shutdown ();
		}

		public ICollector WithName(string name)
		{
			return new LimitedCollector (Root, NameLimiter.Add(name), TagLimiter, Enabled);
		}

		public ICollector WithNamePrefix(string namePrefix)
		{
			return new LimitedCollector (Root, NameLimiter.AddPrefix(namePrefix), TagLimiter, Enabled);
		}

		public ICollector WithTag (string tagKey, string tagValue)
		{
			return new LimitedCollector(Root, NameLimiter, TagLimiter.Add(tagKey, tagValue), Enabled);
		}

		public ICollector WithTags (IReadOnlyDictionary<string, string> tags)
		{
			return new LimitedCollector(Root, NameLimiter, TagLimiter.Add(Tags), Enabled);
		}

		public ICollector WithTagger<TTaggable> (string tagKey, Func<TTaggable, string> tagger)
		{
			return new LimitedCollector(Root, NameLimiter, TagLimiter.Add (tagKey, tagger), Enabled);
		}

		public ICollector WithTag<TTaggable> (string tagKey, TTaggable tagValue)
		{
			if (Taggers == null || !Taggers.Contains<TTaggable> (tagKey))
			{
				throw new ArgumentOutOfRangeException ("tagKey", tagKey, "You must provide a tagger.");
			}

			if (Tags != null && Tags.ContainsKey (tagKey))
			{
				throw new ArgumentException ("Tag already applied.", "tagKey");
			}

			var value = Taggers.Get (tagKey, tagValue);

			return WithTag (tagKey, value);
		}

		public ICollectorMetric<TData> GetMetric<TData>(string name)
		{
			var entry = Root.GetSchemaEntry<TData> (name);
			return new CollectorMetric<TData> (Root, entry, NameLimiter.Resolve(name), TagLimiter, Enabled);
		}
	}
}

