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
			// TODO: use twine?

			if (parent is RootCollector)
			{
				return new LimitedCollector ((RootCollector)parent, namePrefix, new TagLimiter());
			}
			else if (parent is LimitedCollector)
			{
				var limited = parent as LimitedCollector;
				var newName = limited.NamePrefix + namePrefix;

				return new LimitedCollector (limited.Root, newName, new TagLimiter(limited.Tags, null));
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
				return new LimitedCollector ((RootCollector)parent, "", new TagLimiter(tags, null));
			}
			else if (parent is LimitedCollector)
			{
				var limited = parent as LimitedCollector;
				var newLimiter = limited.Limiter.Add (tags);

				return new LimitedCollector (limited.Root, "", newLimiter);
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

				return new LimitedCollector ((RootCollector)parent, "", newLimiter);
			}
			else if (parent is LimitedCollector)
			{
				var limited = parent as LimitedCollector;
				var newLimiter = limited.Limiter.Add (tagKey, tagger);

				return new LimitedCollector(limited.Root, limited.NamePrefix, newLimiter);
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

				return new CollectorMetric(limited.Root, limited.NamePrefix + name, limited.Limiter);
			}
			else
			{
				// uh-oh!
				throw new Exception("unknown/illegal collector type!");
			}
		}
	}
}

