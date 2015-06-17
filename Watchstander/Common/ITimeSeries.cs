using System;
using System.Collections.Generic;

namespace Watchstander.Common
{
	// A time series, which is a unique combinations of metric and tags.
	/// <summary>
	/// An OpenTSDB time series, which is a unique combinations of metric and tags.
	/// Can have an associated description specific to the time series as well.
	/// </summary>
	public interface ITimeSeries
	{
		/// <summary>
		/// The description of the time series.
		/// </summary>
		/// <value></value>
		string Description { get; }

		/// <summary>
		/// The metric this time series is associated with.
		/// </summary>
		/// <value></value>
		IMetric Metric { get; }

		/// <summary>
		/// The tags that identify the time series on the metric.
		/// </summary>
		/// <value></value>
		IReadOnlyDictionary<string, string> Tags { get; }
	}
}

