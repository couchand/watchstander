using System;
using System.Collections.Generic;
using Watchstander.Common;
using Watchstander.Utilities;

namespace Watchstander.Plumbing
{
	public class AccumulatingSchemaEntry : ISchemaEntry
	{
		private AccumulatingMetric metric;
		public List<ITimeSeries> timeSeries;

		public IMetric Metric => metric;
		public IReadOnlyList<ITimeSeries> TimeSeries => timeSeries.AsReadOnly();

		public AccumulatingSchemaEntry (AccumulatingMetric metric)
		{
			this.metric = metric;
			this.timeSeries = new List<ITimeSeries>();
		}

		public void SetDescription(string description)
		{
			metric.SetDescription (description);
		}

		public void SetRate (Rate rate)
		{
			metric.SetRate (rate);
		}

		public void SetUnit (string unit)
		{
			metric.SetUnit (unit);
		}

		public void AddTimeSeries (ITimeSeries timeSeries)
		{
			this.timeSeries.Add(timeSeries);
		}
	}

	public class AccumulatingSchemaOptions
	{
		/// <summary>
		/// Are redefinitions of existing metadata allowed?
		/// </summary>
		/// <value><c>true</c> to allow metadata updates; otherwise, <c>false</c>.</value>
		public bool AllowMetadataUpdates { get; set; }

		public static AccumulatingSchemaOptions Defaults = new AccumulatingSchemaOptions(false)
		{
			AllowMetadataUpdates = false
		};

		public AccumulatingSchemaOptions(bool loadDefaults = true)
		{
			if (!loadDefaults)
				return;

			this.AllowMetadataUpdates = Defaults.AllowMetadataUpdates;
		}
	}

	public class AccumulatingSchema : ISchema
	{
		private bool allowMetadataUpdates;
		private IDictionary<string, AccumulatingSchemaEntry> entries;

		public IReadOnlyDictionary<string, ISchemaEntry> Entries
		{
			get
			{
				var dict = new Dictionary<string, ISchemaEntry> ();

				foreach (var key in entries.Keys)
				{
					dict [key] = entries [key];
				}

				return dict.AsReadOnly ();
			}
		}

		public AccumulatingSchema(AccumulatingSchemaOptions options)
		{
			this.allowMetadataUpdates = options.AllowMetadataUpdates;
			this.entries = new Dictionary<string, AccumulatingSchemaEntry>();
		}

		public AccumulatingSchemaEntry AddEntry(string metricName)
		{
			var metric = allowMetadataUpdates
				? new AccumulatingMetric(metricName)
				: new SetOnceMetric(metricName);

			var entry = new AccumulatingSchemaEntry(metric);
			entries [metric.Name] = entry;
			return entry;
		}

		public AccumulatingSchemaEntry GetEntry(string metricName)
		{
			if (entries.ContainsKey(metricName))
				return entries[metricName];

			return AddEntry(metricName);
		}
	}
}

