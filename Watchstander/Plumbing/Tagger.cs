using System;
using System.Collections.Generic;
using System.Linq;
using Watchstander.Common;

namespace Watchstander.Plumbing
{
	public class Tagger<TTaggable> : ITag
	{
		private readonly string tagKey;
		private readonly Func<TTaggable, string> tagger;
		private readonly List<TTaggable> domain;

		public Tagger (string tagKey, Func<TTaggable, string> tagger)
		{
			this.tagKey = tagKey;
			this.tagger = tagger;

			this.domain = new List<TTaggable> ();
		}

		public void AddValue(TTaggable tagValue)
		{
			domain.Add (tagValue);
		}

		public void AddValues(IEnumerable<TTaggable> tagValues)
		{
			domain.AddRange (tagValues);
		}

		public string TagKey => tagKey;
		public IEnumerable<string> TagValues => domain.Select(tagger).Distinct();
	}
}

