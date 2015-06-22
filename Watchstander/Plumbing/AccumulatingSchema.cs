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

	public class AccumulatingSchema : ISchema
	{
		private IDictionary<string, AccumulatingSchemaEntry> entries;

		public IReadOnlyDictionary<string, ISchemaEntry> Entries => (IReadOnlyDictionary<string, ISchemaEntry>)entries.AsReadOnly();

		public AccumulatingSchema()
		{
			this.entries = new Dictionary<string, AccumulatingSchemaEntry>();
		}

		public void AddEntry(AccumulatingMetric metric)
		{
			this.entries [metric.Name] = new AccumulatingSchemaEntry(metric);
		}
	}
}

