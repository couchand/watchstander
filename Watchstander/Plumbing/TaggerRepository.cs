using System;
using System.Collections.Generic;
using System.Linq;
using Watchstander.Common;

namespace Watchstander.Plumbing
{
	public class TaggerRepository
	{
		private readonly Dictionary<string, StaticTag> tags;
		private readonly Dictionary<string, Type> types;
		private readonly Dictionary<string, ITag> taggers;

		public TaggerRepository ()
		{
			tags = new Dictionary<string, StaticTag> ();
			types = new Dictionary<string, Type> ();
			taggers = new Dictionary<string, ITag> ();
		}

		public TaggerRepository (TaggerRepository copy)
		{
			this.tags = new Dictionary<string, StaticTag> (copy.tags);
			this.types = new Dictionary<string, Type> (copy.types);
			this.taggers = new Dictionary<string, ITag> (copy.taggers);
		}

		public IEnumerable<string> GetAllTagKeys()
		{
			return tags.Keys.Concat (types.Keys);
		}

		public virtual void AddStatic (string tagKey, string tagValue)
		{
			if (types.ContainsKey (tagKey))
			{
				throw new Exception ("already exists!");
			}

			StaticTag tag;

			if (tags.ContainsKey (tagKey))
			{
				tag = tags [tagKey];
			}
			else
			{
				tag = new StaticTag (tagKey);
				tags [tagKey] = tag;
			}

			tag.AddValue (tagValue);
		}

		public virtual void AddStatic (string tagKey, IEnumerable<string> tagValues)
		{
			if (types.ContainsKey (tagKey))
			{
				throw new Exception ("already exists!");
			}

			StaticTag tag;

			if (tags.ContainsKey (tagKey))
			{
				tag = tags [tagKey];
			}
			else
			{
				tag = new StaticTag (tagKey);
				tags [tagKey] = tag;
			}

			tag.AddValues (tagValues);
		}

		public bool ContainsStatic (string tagKey)
		{
			if (types.ContainsKey (tagKey))
			{
				return false;
			}

			return tags.ContainsKey (tagKey);
		}

		public StaticTag GetStatic (string tagKey)
		{
			if (!ContainsStatic (tagKey))
			{
				throw new Exception ("not found!");
			}

			return tags [tagKey];
		}

		public virtual void AddDynamic<TTaggable> (Tagger<TTaggable> tagger)
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

	public class ReadOnlyTaggerRepository : TaggerRepository
	{
		public ReadOnlyTaggerRepository () : base () {}
		public ReadOnlyTaggerRepository (TaggerRepository other)
			: base (other) {}

		public virtual void AddStatic (string tagKey, string tagValue)
		{
			throw new Exception ("tagger repo is read-only");
		}

		public virtual void AddStatic (string tagKey, IEnumerable<string> tagValues)
		{
			throw new Exception ("tagger repo is read-only");
		}

		public virtual void AddDynamic<TTaggable> (Tagger<TTaggable> tagger)
		{
			throw new Exception ("tagger repo is read-only");
		}
	}

	public static class TaggerExtensions
	{
		public static ReadOnlyTaggerRepository AsReadOnly (this TaggerRepository repo)
		{
			return new ReadOnlyTaggerRepository (repo);
		}
	}
}

