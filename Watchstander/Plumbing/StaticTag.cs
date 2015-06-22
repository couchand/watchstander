using System;
using System.Collections.Generic;
using System.Linq;
using Watchstander.Common;

namespace Watchstander.Plumbing
{
	public class StaticTag : ITag
	{
		private readonly string tagKey;
		private readonly List<string> range;

		public StaticTag (string tagKey)
		{
			this.tagKey = tagKey;
			this.range = new List<string> ();
		}

		public void AddValue(string tagValue)
		{
			range.Add (tagValue);
		}

		public void AddValues(IEnumerable<string> tagValues)
		{
			range.AddRange (tagValues);
		}

		public string TagKey => tagKey;
		public IEnumerable<string> TagValues => range.Distinct();
	}
}

