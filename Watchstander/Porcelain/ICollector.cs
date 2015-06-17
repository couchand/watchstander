using System;
using System.Collections.Generic;
using Watchstander.Common;

namespace Watchstander.Porcelain
{
	public interface ICollector : IDescribable
	{
		ICollector WithName (string namePrefix);
		ICollector WithNamePrefix (string namePrefix);

		ICollector WithTag (string tagKey, string tagValue);
		ICollector WithTags (IReadOnlyDictionary<string, string> tags);

		ICollector WithTagger<TValue> (string tagKey, Func<TValue, string> tagger);
		ICollector WithTag<TValue> (string tagKey, TValue tagValue);

		ICollectorMetric GetMetric (string metricName);
	}

	public interface ICollectorMetric : IMetric, IDescribable
	{
		ICollectorMetric WithTag (string tagKey, string tagValue);
		ICollectorMetric WithTags (IReadOnlyDictionary<string, string> tags);

		ICollectorMetric WithTagger<TValue> (string tagKey, Func<TValue, string> tagger);
		ICollectorMetric WithTag<TValue> (string tagKey, TValue tagValue);

		ICollectorTimeSeries GetTimeSeries ();
	}

	public interface ICollectorTimeSeries : ITimeSeries, IDescribable
	{
	}
}

