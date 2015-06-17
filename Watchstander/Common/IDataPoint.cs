using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Watchstander.Common
{
	/// <summary>
	/// An OpenTSDB data point.  This is all that is needed for minimum library use.
	/// </summary>
	public interface IDataPoint<TValue>
	{
		/// <summary>
		/// The name of the metric this data point belongs to.
		/// </summary>
		/// <value></value>
		[DataMember(Name="metric")]
		string Metric { get; }

		/// <summary>
		/// A timestamp of when this data point was measured.
		/// </summary>
		/// <value></value>
		[DataMember(Name="timestamp")]
		DateTime Timestamp { get; }

		/// <summary>
		/// The value of the data point.
		/// </summary>
		/// <value></value>
		[DataMember(Name="value")]
		TValue Value { get; }

		/// <summary>
		/// The tags defining the time series this data point belongs to.
		/// </summary>
		/// <value>The tags.</value>
		[DataMember(Name="tags")]
		IReadOnlyDictionary<string, string> Tags { get; }
	}
}