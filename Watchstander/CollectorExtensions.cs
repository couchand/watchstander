using System;
using System.Collections.Generic;
using Watchstander.Porcelain;

namespace Watchstander
{
	public static class CollectorExtensions
	{
		public static ICollectorTimeSeries<TData> GetTimeSeries<TData> (this ICollector collector, string metricName)
		{
			var metric = collector.GetMetric<TData> (metricName);
			return metric.GetTimeSeries ();
		}

		public static ICollectorTimeSeries<TData> GetTimeSeries<TData> (this ICollector collector, string metricName, string tagKey, string tagValue)
		{
			var metric = collector.GetMetric<TData> (metricName);
			return metric.GetTimeSeries (tagKey, tagValue);
		}

		public static ICollectorTimeSeries<TData> GetTimeSeries<TData> (this ICollector collector, string metricName, IReadOnlyDictionary<string, string> tags)
		{
			var metric = collector.GetMetric<TData> (metricName);
			return metric.GetTimeSeries (tags);
		}

		public static ICollectorTimeSeries<TData> GetTimeSeries<TData, TTag> (this ICollector collector, string metricName, string tagKey, TTag tagValue)
		{
			var metric = collector
				.GetMetric<TData> (metricName)
				.WithTag<TTag> (tagKey, tagValue);
			return metric.GetTimeSeries();
		}

		public static ICollectorTimeSeries<TData> GetTimeSeries<TData> (this ICollectorMetric<TData> metric, string tagKey, string tagValue)
		{
			return metric.WithTag(tagKey, tagValue).GetTimeSeries();
		}

		public static ICollectorTimeSeries<TData> GetTimeSeries<TData> (this ICollectorMetric<TData> metric, IReadOnlyDictionary<string, string> tags)
		{
			return metric.WithTags(tags).GetTimeSeries();
		}

		public static ICollectorTimeSeries<TData> GetTimeSeries<TData, TTaggable> (this ICollectorMetric<TData> metric, string tagKey, TTaggable tagValue)
		{
			return metric.WithTag<TTaggable>(tagKey, tagValue).GetTimeSeries();
		}

		public static void Record<TData> (this ICollectorMetric<TData> metric, TData data)
		{
			var timeSeries = metric.GetTimeSeries ();
			timeSeries.Record (data);
		}

		public static void Record<TData> (this ICollectorMetric<TData> metric, TData data, DateTime now)
		{
			var timeSeries = metric.GetTimeSeries ();
			timeSeries.Record (data, now);
		}

		public static void Record<TData> (this IRecorder<TData> recorder, TData data)
		{
			recorder.Record (data, DateTime.UtcNow);
		}
	}
}

