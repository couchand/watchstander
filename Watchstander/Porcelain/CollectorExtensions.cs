using System;
using System.Collections.Generic;

namespace Watchstander.Porcelain
{
	public static class CollectorExtensions
	{
		public static ICollectorTimeSeries<TData> GetTimeSeries<TData> (this ICollector collector, string metricName)
		{
			var metric = collector.GetMetric (metricName);
			return metric.GetTimeSeries<TData> ();
		}

		public static ICollectorTimeSeries<TData> GetTimeSeries<TData> (this ICollector collector, string metricName, string tagKey, string tagValue)
		{
			var metric = collector.GetMetric (metricName);
			return metric.GetTimeSeries<TData> (tagKey, tagValue);
		}

		public static ICollectorTimeSeries<TData> GetTimeSeries<TData> (this ICollector collector, string metricName, IReadOnlyDictionary<string, string> tags)
		{
			var metric = collector.GetMetric (metricName);
			return metric.GetTimeSeries<TData> (tags);
		}

		public static ICollectorTimeSeries<TData> GetTimeSeries<TData, TValue> (this ICollector collector, string metricName, string tagKey, TValue tagValue)
		{
			var metric = collector.GetMetric (metricName);
			return metric.GetTimeSeries<TData, TValue> (tagKey, tagValue);
		}

		public static void Record<TData> (this ICollectorMetric metric, TData data)
		{
			var timeSeries = metric.GetTimeSeries<TData> ();
			timeSeries.Record (data);
		}

		public static void Record<TData> (this ICollectorMetric metric, TData data, DateTime now)
		{
			var timeSeries = metric.GetTimeSeries<TData> ();
			timeSeries.Record (data, now);
		}

		public static void Record<TData> (this IRecorder<TData> recorder, TData data)
		{
			recorder.Record (data, DateTime.UtcNow);
		}
	}
}

