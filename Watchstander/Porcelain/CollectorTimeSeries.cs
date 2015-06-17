using System;
using System.Collections.Generic;
using Watchstander.Common;

namespace Watchstander.Porcelain
{
	public class CollectorTimeSeries : ICollectorTimeSeries
	{
		public string Description { get; set; }

		public IMetric Metric => metric;
		public IReadOnlyDictionary<string, string> Tags { get; }

		private CollectorMetric metric;

		public CollectorTimeSeries (CollectorMetric metric, IReadOnlyDictionary<string, string> Tags)
		{
			this.metric = metric;
			this.Tags = Tags;
		}
	}
}

