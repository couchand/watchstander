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

		private RootCollector collector;
		private CollectorMetric<TData> metric;

		private bool enabled;

		internal CollectorTimeSeries (RootCollector collector, CollectorMetric<TData> metric, IReadOnlyDictionary<string, string> Tags, bool enabled)
		{
			validate(metric, Tags);

			this.collector = collector;
			this.metric = metric;
			this.Tags = Tags;
			this.enabled = enabled;
		}

		private CollectorTimeSeries (CollectorTimeSeries<TData> copy, bool enabled)
		{
			this.collector = copy.collector;
			this.metric = copy.metric;
			this.Tags = copy.Tags;
			this.enabled = enabled;
		}

		public ICollectorTimeSeries<TData> Disabled()
		{
			return new CollectorTimeSeries<TData> (this, false);
		}

		public ICollectorTimeSeries<TData> Reenabled()
		{
			return new CollectorTimeSeries<TData> (this, true);
		}

		private static void validate(CollectorMetric<TData> metric, IReadOnlyDictionary<string, string> Tags)
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

		public void Record(TData value, DateTime now)
		{
			if (!enabled)
				return;

			var dataPoint = new CollectedDataPoint<TData>(this, now, value);
			var points = new List<IDataPoint<TData>> { dataPoint };

			var consumer = collector as IDataPointConsumer<TData>;

			if (consumer == null)
			{
				// should never happen
				throw new Exception("unknown data point type & illegal state");
			}

			consumer.Consume(points);
		}
	}
}

