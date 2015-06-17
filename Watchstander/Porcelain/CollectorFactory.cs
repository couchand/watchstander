using System;
using System.Collections.Generic;
using Watchstander.Utilities;
using Watchstander.Plumbing;

namespace Watchstander.Porcelain
{
	public static class CollectorFactory
	{
		public static ICollector LimitCollectorName (ICollector parent, string namePrefix)
		{
			if (parent is RootCollector)
			{
				return new LimitedCollector ((RootCollector)parent, new NameLimiter(namePrefix), new TagLimiter());
			}
			else if (parent is LimitedCollector)
			{
				var limited = parent as LimitedCollector;
				return new LimitedCollector (limited.Root, limited.NameLimiter.AddPrefix(namePrefix), new TagLimiter(limited.Tags, null));
			}
			else
			{
				// uh-oh!
				throw new Exception("unknown collector type!");
			}
		}

		public static ICollector LimitCollectorTags (ICollector parent, string tagKey, string tagValue)
		{
			var dict = new Dictionary<string, string> ();
			dict [tagKey] = tagValue;
			return LimitCollectorTags (parent, dict.AsReadOnly ());
		}

		public static ICollector LimitCollectorTags(ICollector parent, IReadOnlyDictionary<string, string> tags)
		{
			if (parent is RootCollector)
			{
				return new LimitedCollector ((RootCollector)parent, new NameLimiter(), new TagLimiter(tags, null));
			}
			else if (parent is LimitedCollector)
			{
				var limited = parent as LimitedCollector;
				var newLimiter = limited.TagLimiter.Add (tags);

				return new LimitedCollector (limited.Root, limited.NameLimiter, newLimiter);
			}
			else
			{
				// uh-oh!
				throw new Exception("unknown collector type!");
			}
		}

		public static ICollector LimitCollectorTags<TValue>(ICollector parent, string tagKey, Func<TValue, string> tagger)
		{
			if (parent is RootCollector)
			{
				var newLimiter = new TagLimiter ().Add (tagKey, tagger);

				return new LimitedCollector ((RootCollector)parent, new NameLimiter(), newLimiter);
			}
			else if (parent is LimitedCollector)
			{
				var limited = parent as LimitedCollector;
				var newLimiter = limited.TagLimiter.Add (tagKey, tagger);

				return new LimitedCollector(limited.Root, limited.NameLimiter, newLimiter);
			}
			else
			{
				// uh-oh!
				throw new Exception("unknown collector type!");
			}
		}

		public static CollectorMetric GetLimitedMetric(ICollector parent, string name)
		{
			if (parent is LimitedCollector)
			{
				var limited = parent as LimitedCollector;

				return new CollectorMetric(limited.Root, limited.NameLimiter.Resolve(name), limited.TagLimiter);
			}
			else
			{
				// uh-oh!
				throw new Exception("unknown/illegal collector type!");
			}
		}
	}
}

