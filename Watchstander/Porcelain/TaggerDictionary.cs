using System;
using System.Collections.Generic;
using Watchstander.Common;
using Watchstander.Utilities;

namespace Watchstander.Porcelain
{
	public class TaggerDictionary
	{
		private readonly IDictionary<string, Func<object, string>> taggers;

		public TaggerDictionary ()
		{
			this.taggers = new Dictionary<string, Func<object, string>> ();
		}

		public TaggerDictionary (TaggerDictionary copy)
		{
			this.taggers = new Dictionary<string, Func<object, string>> ();
			CopyFrom (copy.taggers.AsReadOnly());
		}

		private void CopyFrom (IReadOnlyDictionary<string, Func<object, string>> copy)
		{
			foreach (var key in copy.Keys)
			{
				taggers [key] = copy [key];
			}
		}

		public void Add<TValue> (string key, Func<TValue, string> value)
		{
			taggers [key] = (o) => value((TValue)o);
		}

		public string Get<TValue> (string key, TValue value)
		{
			return taggers [key](value);
		}

		public bool Contains<TValue> (string key)
		{
			return taggers.ContainsKey (key);
		}
	}
}

