using System;
using System.Collections.Generic;
using Watchstander.Common;
using Watchstander.Utilities;

namespace WatchstanderTests.Common
{
	public class MockMetric : IMetric
	{
		public string Name { get; }

		public string Description { get; set; }
		public Rate Rate { get; set; }
		public string Unit { get; set; }

		public IReadOnlyList<string> TagKeys { get; }

		public MockMetric(string Name)
		{
			this.Name = Name;
		}
	}

	public class MockTimeSeries : ITimeSeries
	{
		public string Description { get; set; }

		public IMetric Metric { get; }
		public IReadOnlyDictionary<string, string> Tags => tags.AsReadOnly();

		private readonly Dictionary<string, string> tags;

		public MockTimeSeries(IMetric Metric)
		{
			this.Metric = Metric;
			this.tags = new Dictionary<string, string> ();
		}
	}

	public class MockSchemaEntry : ISchemaEntry
	{
		public IMetric Metric { get; }
		public IReadOnlyList<ITimeSeries> TimeSeries => timeSeries.AsReadOnly();

		private readonly List<ITimeSeries> timeSeries;

		public MockSchemaEntry(IMetric Metric)
		{
			this.Metric = Metric;
			this.timeSeries = new List<ITimeSeries>();
		}
	}

	public class MockSchema : ISchema
	{
		public IReadOnlyDictionary<string, ISchemaEntry> Entries => entries.AsReadOnly();

		private readonly Dictionary<string, ISchemaEntry> entries;

		public MockSchema ()
		{
			entries = new Dictionary<string, ISchemaEntry>();
		}

		public MockSchemaEntry AddEntry(string metric, string desc, Rate rate, string unit)
		{
			var mock = new MockMetric (metric);
			mock.Description = desc;
			mock.Rate = rate;
			mock.Unit = unit;

			var entry = new MockSchemaEntry(mock);
			entries[metric] = entry;
			return entry;
		}
	}
}

