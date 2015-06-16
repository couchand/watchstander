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
				return new LimitedCollector ((RootCollector)parent, namePrefix, null, null);
			}
			else if (parent is LimitedCollector)
			{
				var limited = parent as LimitedCollector;
				var newName = limited.NamePrefix + namePrefix;

				return new LimitedCollector (limited.Root, newName, limited.Tags, null);
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
				return new LimitedCollector ((RootCollector)parent, "", tags, null);
			}
			else if (parent is LimitedCollector)
			{
				var limited = parent as LimitedCollector;
				var newTags = limited.Tags.CombineDictionaries (tags);

				return new LimitedCollector (limited.Root, "", newTags, null);
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
				var newTaggers = new TaggerDictionary ();
				newTaggers.Add (tagKey, tagger);

				return new LimitedCollector ((RootCollector)parent, "", null, newTaggers);
			}
			else if (parent is LimitedCollector)
			{
				var limited = parent as LimitedCollector;
				var newTaggers = new TaggerDictionary(limited.taggers);
				newTaggers.Add (tagKey, tagger);

				return new LimitedCollector(limited.Root, limited.NamePrefix, limited.Tags, newTaggers);
			}
			else
			{
				// uh-oh!
				throw new Exception("unknown collector type!");
			}
		}
	}
}

