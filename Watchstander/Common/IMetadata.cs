using System;
using System.Collections.Generic;

namespace Watchstander.Common
{
	/// <summary>
	/// A single entry of Bosun metadata.
	/// </summary>
	public interface IMetadata
	{
		/// <summary>
		/// The metric name this metadata applies to.
		/// </summary>
		/// <value></value>
		string Metric { get; }

		/// <summary>
		/// Tags to specify the time series this metadata applies to.
		/// </summary>
		/// <value></value>
		IReadOnlyDictionary<string, string> Tags { get; }

		/// <summary>
		/// The name of the metadata (e.g. desc, rate, unit).
		/// </summary>
		/// <value></value>
		string Name { get; }

		/// <summary>
		/// The metadata value.
		/// </summary>
		/// <value></value>
		string Value { get; }
	}
}