using System;
using System.Collections.Generic;
using System.Linq;


namespace Watchstander.Expressions
{
	public class MetricQuery : IQuerySegment
	{
		internal readonly string metric;
		internal readonly Dictionary<string, string> tags;

		public MetricQuery (string metric, IDictionary<string, string> tags)
		{
			this.metric = metric;
			this.tags = new Dictionary<string, string> (tags);
		}

		public MetricQuery (string metric)
			: this (metric, new Dictionary<string, string> ()) {}

		public string GetQuerySegment()
		{
			string tagStr;

			if (tags.Count > 0)
			{
				var tagPairs = String.Join (
					",",
					tags.Select (t => String.Format ("{0}={1}", t.Key, t.Value))
				);

				tagStr = "{" + tagPairs + "}";
			}
			else
			{
				tagStr = "";
			}

			return metric + tagStr;
		}
	}
}

