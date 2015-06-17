using System;
using System.Collections.Generic;
using System.Linq;
using Watchstander.Utilities;

namespace Watchstander.Porcelain
{
	public class TagLimiter
	{
		public IReadOnlyDictionary<string, string> Tags { get; }
		public IReadOnlyList<string> TagKeys => Tags.Keys.ToList().AsReadOnly();
		public TaggerDictionary Taggers { get; }

		public TagLimiter ()
		{
			this.Tags = new Dictionary<string, string>().AsReadOnly();
			this.Taggers = new TaggerDictionary();
		}

		private TagLimiter (IReadOnlyDictionary<string, string> Tags, TaggerDictionary Taggers)
		{
			this.Tags = Tags;
			this.Taggers = Taggers;
		}

		public TagLimiter Add(string tagKey, string tagValue)
		{
			var newTags = new Dictionary<string, string> ();
			newTags [tagKey] = tagValue;

			return Add (newTags);
		}

		public TagLimiter Add(IReadOnlyDictionary<string, string> tagsToAdd)
		{
			var newTags = Tags.CombineDictionaries (tagsToAdd);

			return new TagLimiter (newTags, Taggers);
		}

		public TagLimiter Add<TValue>(string tagKey, Func<TValue, string> tagger)
		{
			var newTaggers = new TaggerDictionary (Taggers);
			newTaggers.Add(tagKey, tagger);

			return new TagLimiter(Tags, newTaggers);
		}

		public TagLimiter Resolve<TValue>(string tagKey, TValue tagValue)
		{
			if (Taggers == null || !Taggers.Contains<TValue> (tagKey))
			{
				throw new ArgumentOutOfRangeException ("tagKey", tagKey, "You must provide a tagger.");
			}

			if (Tags != null && Tags.ContainsKey (tagKey))
			{
				throw new ArgumentException ("Tag already applied.", "tagKey");
			}

			var value = Taggers.Get (tagKey, tagValue);

			return Add(tagKey, value);
		}
	}
}

