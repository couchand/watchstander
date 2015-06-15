using System;
using System.Collections.Generic;

namespace Watchstander.Common
{
	/// <summary>
	/// A single entry of Bosun metadata.
	/// </summary>
	public interface IMetadata
	{
		// The metric name.
		string Metric { get; }

		// Tags to specify the time series this metadata applies to.
		IReadOnlyDictionary<string, string> Tags { get; }

		// The name of the metadata (e.g. desc, rate, unit)
		string Name { get; }

		// The metadata value.
		string Value { get; }
	}
}