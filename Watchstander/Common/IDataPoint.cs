using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Watchstander.Common
{
	/// <summary>
	/// An OpenTSDB data point.
	/// </summary>
	public interface IDataPoint<TValue>
	{
		[DataMember(Name="metric")]
		string Metric { get; }

		[DataMember(Name="timestamp")]
		DateTime Timestamp { get; }

		[DataMember(Name="value")]
		TValue Value { get; }

		[DataMember(Name="tags")]
		IReadOnlyDictionary<string, string> Tags { get; }
	}
}