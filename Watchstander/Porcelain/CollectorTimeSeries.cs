using System;
using System.Collections.Generic;
using Watchstander.Common;

namespace Watchstander.Porcelain
{
	public class CollectorTimeSeries<TData> : ICollectorTimeSeries<TData>
	{
		public string Description { get; set; }

		public IMetric Metric => metric;
		public IReadOnlyDictionary<string, string> Tags { get; }

		private CollectorMetric metric;

		public CollectorTimeSeries (CollectorMetric metric, IReadOnlyDictionary<string, string> Tags)
		{
			validate(metric, Tags);

			this.metric = metric;
			this.Tags = Tags;
		}

		private static void validate(CollectorMetric metric, IReadOnlyDictionary<string, string> Tags)
		{
			var valueType = typeof(TData);
			if (typeof(long) != valueType && typeof(float) != valueType)
			{
				// we don't deal in strings around here
				throw new Exception("unknown data point type");
			}

			if (Tags == null || Tags.Count == 0)
			{
				// needz one tag
				throw new Exception ("you must provide at least one tag");
			}

			// TODO: check for schema-completeness
		}
	}
}

