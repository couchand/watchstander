using System;
using System.Collections.Generic;

namespace Watchstander.Common
{
	/// <summary>
	/// The Bosun metrics schema.
	/// </summary>
	public interface ISchema
	{
		/// <summary>
		/// The schema entries by metric name.
		/// </summary>
		/// <value></value>
		IReadOnlyDictionary<string, ISchemaEntry> Entries { get; }
	}

	/// <summary>
	/// The schema for a single metric.
	/// </summary>
	public interface ISchemaEntry
	{
		/// <summary>
		/// Metadata about the metric.
		/// </summary>
		/// <value></value>
		IMetric Metric { get; }

		/// <summary>
		/// All known time series on the metric.
		/// </summary>
		/// <value></value>
		IReadOnlyList<ITimeSeries> TimeSeries { get; }
	}
}