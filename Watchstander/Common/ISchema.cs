using System;
using System.Collections.Generic;

namespace Watchstander.Common
{
	/// <summary>
	/// The Bosun metrics schema.
	/// </summary>
	public interface ISchema
	{
		// Entries by metric name.
		IReadOnlyDictionary<string, ISchemaEntry> Entries { get; }
	}

	/// <summary>
	/// The schema for a single metric.
	/// </summary>
	public interface ISchemaEntry
	{
		// The metric metadata.
		IMetric Metric { get; }

		// All known series on this metric.
		IReadOnlyList<ITimeSeries> TimeSeries { get; }
	}
}