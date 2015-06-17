using System;
using System.Collections.Generic;
using Watchstander.Common;

namespace Watchstander.Porcelain
{
	public class CollectedDataPoint<TValue> : IDataPoint<TValue>
	{
		public DateTime Timestamp { get;}
		public TValue Value { get; }

		public string Metric => timeSeries.Metric.Name;
		public IReadOnlyDictionary<string, string> Tags => timeSeries.Tags;

		private readonly ITimeSeries timeSeries;

		public CollectedDataPoint(ITimeSeries timeSeries, DateTime Timestamp, TValue Value)
		{
			this.timeSeries = timeSeries;
			this.Timestamp = Timestamp;
			this.Value = Value;
		}
	}
}

