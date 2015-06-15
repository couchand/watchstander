using System;
using System.Collections.Generic;

namespace Watchstander.Common
{
	// The required metadata for meaningful metric handling.
	// Note: Not strictly necessary for just sending data, only required
	//       for validation and sending metadata.
	public interface IMetric
	{
		string Name { get; }

		// default description for time series without a more specific one
		string Description { get; }

		Rate Rate { get; }
		string Unit { get; }

		IReadOnlyList<string> TagKeys { get; }
	}
}