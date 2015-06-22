using System;
using System.Collections.Generic;
using System.Linq;
using Watchstander.Common;

namespace Watchstander.Plumbing
{
	public class TaggerRepository
	{
		private readonly Dictionary<string, string> tags;
		private readonly Dictionary<string, Type> types;
		private readonly Dictionary<string, ITag> taggers;

		public TaggerRepository ()
		{
			tags = new Dictionary<string, string> ();
			types = new Dictionary<string, Type> ();
			taggers = new Dictionary<string, ITag> ();
		}

		public TaggerRepository (TaggerRepository copy)
		{
			this.tags = new Dictionary<string, string> (copy.tags);
			this.types = new Dictionary<string, Type> (copy.types);
			this.taggers = new Dictionary<string, ITag> (copy.taggers);
		}

		public IEnumerable<string> GetAllTagKeys()
		{
			return tags.Keys.Concat (types.Keys);
		}

		public void AddStatic (string tagKey, string tagValue)
		{
			if (types.ContainsKey (tagKey) || tags.ContainsKey (tagKey))
			{
				throw new Exception ("already exists!");
			}

			tags [tagKey] = tagValue;
		}

		public bool ContainsStatic (string tagKey)
		{
			if (types.ContainsKey (tagKey))
			{
				return false;
			}

			return tags.ContainsKey (tagKey);
		}

		public string GetStatic (string tagKey)
		{
			if (!ContainsStatic (tagKey))
			{
				throw new Exception ("not found!");
			}

			return tags [tagKey];
		}

		public void AddDynamic<TTaggable> (Tagger<TTaggable> tagger)
		{
			var tagKey = tagger.TagKey;

			if (types.ContainsKey (tagKey) || tags.ContainsKey (tagKey))
			{
				throw new Exception ("already exists!");
			}

			types [tagKey] = typeof(TTaggable);
			taggers [tagKey] = tagger;
		}

		public bool ContainsDynamic<TTaggable> (string tagKey)
		{
			if (tags.ContainsKey (tagKey) || !types.ContainsKey (tagKey))
			{
				return false;
			}
			
			if (types [tagKey] != typeof(TTaggable))
			{
				throw new Exception ("type error");
			}
			
			return true;
		}

		public Tagger<TTaggable> GetDynamic<TTaggable> (string tagKey)
		{
			if (!ContainsDynamic<TTaggable> (tagKey))
			{
				throw new Exception ("not found!");
			}

			return (Tagger<TTaggable>)taggers [tagKey];
		}

		public bool ContainsAny (string tagKey) 
		{
			return tags.ContainsKey (tagKey) || types.ContainsKey (tagKey);
		}
	}
}

