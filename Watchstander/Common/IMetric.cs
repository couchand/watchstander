using System;
using System.Collections.Generic;

namespace Watchstander.Common
{
	/// <summary>
	/// A single metric in a Bosun schema, with the required metadata
	/// for meaningful metric handling.
	/// </summary>
	/// <remarks>
	/// Note: Not strictly necessary for just sending data, only required
	/// for validation and sending metadata.
	/// <remarks>
	public interface IMetric
	{
		/// <summary>
		/// The name of the metric.
		/// </summary>
		/// <value></value>
		string Name { get; }

		/// <summary>
		/// The default description for time series without a more specific one.
		/// </summary>
		/// <value></value>
		string Description { get; }

		/// <summary>
		/// The metric rate (Bosun currently supports counter, gauge, or rate).
		/// </summary>
		/// <value></value>
		Rate Rate { get; }

		/// <summary>
		/// The unit the metric is measured in.
		/// </summary>
		/// <value></value>
		string Unit { get; }

		/// <summary>
		/// The authoritative list of tag keys for this metric.  Every data point
		/// transmitted on this metric should have this list of tag keys and only
		/// these tag keys.
		/// </summary>
		/// <value></value>
		IReadOnlyList<string> TagKeys { get; }
	}
}