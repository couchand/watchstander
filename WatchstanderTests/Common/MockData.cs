using System;
using System.Collections.Generic;
using Watchstander.Common;

namespace WatchstanderTests.Common
{
	public class DataTest<TData> : IDataPoint<TData>
	{
		public string Metric { get; }
		public DateTime Timestamp { get; }
		public TData Value { get; }
		public IReadOnlyDictionary<string, string> Tags { get; }

		public DataTest(string metric, DateTime timestamp, TData value, IReadOnlyDictionary<string, string> tags)
		{
			this.Metric = metric;
			this.Timestamp = timestamp;
			this.Value = value;
			this.Tags = tags;
		}
	}

	public class MetadataTest : IMetadata
	{
		public string Metric { get; }
		public IReadOnlyDictionary<string, string> Tags { get; }
		public string Name { get; }
		public string Value { get; }

		public MetadataTest(string metric, IReadOnlyDictionary<string, string> tags, string name, string value)
		{
			this.Metric = metric;
			this.Tags = tags;
			this.Name = name;
			this.Value = value;
		}
	}

}

