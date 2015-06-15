using System;
using System.Collections.Generic;
using Watchstander.Common;
using Watchstander.Utilities;

namespace Watchstander.Plumbing
{
	public class Schema : ISchema
	{
		public IReadOnlyDictionary<string, ISchemaEntry> Entries
		{
			get
			{
				IDictionary<string, ISchemaEntry> dict = new Dictionary<string, ISchemaEntry> ();

				foreach (var metric in schema.Keys)
				{
					dict [metric] = (ISchemaEntry)schema [metric];
				}

				return dict.AsReadOnly ();
			}
		}

		private IDictionary<string, SchemaEntry> schema;

		public Schema(IDictionary<string, SchemaEntry> schema)
		{
			this.schema = schema;
		}
	}

	public class SchemaEntry : ISchemaEntry
	{
		public IMetric Metric => metric;
		public IReadOnlyList<ITimeSeries> TimeSeries => timeSeries.AsReadOnly();

		internal SchemaMetric metric;
		internal List<ITimeSeries> timeSeries;

		public SchemaEntry(string name)
		{
			metric = new SchemaMetric (name);
			timeSeries = new List<ITimeSeries> ();
		}
	}

	public class SchemaMetric : IMetric
	{
		public string Name { get; }

		// default description for time series without a more specific one
		public string Description { get; set; }

		public Rate Rate { get; set; }
		public string Unit { get; set; }

		private List<string> tagKeys;

		public IReadOnlyList<string> TagKeys => tagKeys.AsReadOnly();

		public SchemaMetric(string name)
		{
			this.Name = name;
			this.Rate = Rate.Unknown;
			this.tagKeys = new List<string>();
		}

		public void AddTagKeys(IEnumerable<string> newTagKeys)
		{
			tagKeys.AddRange (newTagKeys);
		}
	}

	public class SchemaTimeSeries : ITimeSeries
	{
		public IMetric Metric { get; }
		public string Description { get; set; }

		public IReadOnlyDictionary<string, string> TagValues => tagValues.AsReadOnly();

		private IDictionary<string, string> tagValues;

		public SchemaTimeSeries(IMetric Metric, IMetadata metadata)
		{
			this.Metric = Metric;
			this.tagValues = new Dictionary<string, string>();

			foreach (var tagKey in metadata.Tags.Keys)
			{
				this.tagValues[tagKey] = metadata.Tags[tagKey];
			}
		}
	}
}

