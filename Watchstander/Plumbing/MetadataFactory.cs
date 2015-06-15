using System;
using System.Collections.Generic;
using Watchstander.Common;

namespace Watchstander.Plumbing
{
	public class BasicMetadata : IMetadata
	{
		public string Metric { get; }
		public string Name { get; }
		public string Value { get; }
		public DateTime Time { get; }
		public IReadOnlyDictionary<string, string> Tags { get; }

		public BasicMetadata(string Metric, IReadOnlyDictionary<string, string> Tags, string Name, string Value, DateTime Time)
		{
			this.Metric = Metric;
			this.Tags = Tags;
			this.Name = Name;
			this.Value = Value;
			this.Time = Time;
		}
	}

	public class MetricMetadata : IMetadata
	{
		public string Name { get; set; }

		private IMetric metric;
		private string value;

		public MetricMetadata(IMetric metric)
		{
			this.metric = metric;
			this.value = null;
		}

		public string Metric => metric.Name;
		public IReadOnlyDictionary<string, string> Tags => null;

		public string Value
		{
			get
			{
				if (String.Equals(Name, "desc", StringComparison.OrdinalIgnoreCase))
				{
					return metric.Description;
				}
				if (String.Equals(Name, "rate", StringComparison.OrdinalIgnoreCase))
				{
					return metric.Rate.ToMetadataString();
				}
				if (String.Equals(Name, "unit", StringComparison.OrdinalIgnoreCase))
				{
					return metric.Unit;
				}

				return value;
			}
			set
			{
				this.value = value;
			}
		}
	}

	public class TimeSeriesMetadata : IMetadata
	{
		public string Name { get; set; }

		private ITimeSeries timeSeries;
		private string value;

		public TimeSeriesMetadata(ITimeSeries timeSeries)
		{
			this.timeSeries = timeSeries;
			this.value = null;
		}

		public string Metric => timeSeries.Metric.Name;

		public IReadOnlyDictionary<string, string> Tags => timeSeries.TagValues;

		public string Value
		{
			get
			{
				if (String.Equals(Name, "desc", StringComparison.OrdinalIgnoreCase))
				{
					return timeSeries.Description;
				}

				return value;
			}
			set
			{
				this.value = value;
			}
		}
	}

	public class MetadataFactory
	{
		public static IMetadata GetUnit(IMetric metric)
		{
			var metadata = new MetricMetadata (metric);
			metadata.Name = "unit";
			return metadata;
		}

		public static IMetadata GetRate(IMetric metric)
		{
			var metadata = new MetricMetadata (metric);
			metadata.Name = "rate";
			return metadata;
		}

		public static IMetadata GetDescription(IMetric metric)
		{
			var metadata = new MetricMetadata (metric);
			metadata.Name = "desc";
			return metadata;
		}

		public static IMetadata GetDescription(ITimeSeries timeSeries)
		{
			var metadata = new TimeSeriesMetadata (timeSeries);
			metadata.Name = "desc";
			return metadata;
		}
	}
}

