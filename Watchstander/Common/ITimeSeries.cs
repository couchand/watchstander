using System;
using System.Collections.Generic;

namespace Watchstander.Common
{
	// A time series, which is a unique combinations of metric and tags.
	public interface ITimeSeries
	{
		string Description { get; }

		IMetric Metric { get; }
		IReadOnlyDictionary<string, string> Tags { get; }
	}
}

