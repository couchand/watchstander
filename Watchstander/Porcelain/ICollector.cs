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

		ICollector WithTagger<TTaggable> (string tagKey, Func<TTaggable, string> tagger);
		ICollector WithTag<TTaggable> (string tagKey, TTaggable tagValue);

		ICollectorMetric GetMetric (string metricName);
	}

	public interface ICollectorMetric : IMetric, IDescribable
	{
		ICollectorMetric WithTag (string tagKey, string tagValue);
		ICollectorMetric WithTags (IReadOnlyDictionary<string, string> tags);

		ICollectorMetric WithTagger<TTaggable> (string tagKey, Func<TTaggable, string> tagger);
		ICollectorMetric WithTag<TTaggable> (string tagKey, TTaggable tagValue);

		ICollectorTimeSeries<TData> GetTimeSeries<TData> ();
		ICollectorTimeSeries<TData> GetTimeSeries<TData> (string tagKey, string tagValue);
		ICollectorTimeSeries<TData> GetTimeSeries<TData> (IReadOnlyDictionary<string, string> tags);

		ICollectorTimeSeries<TData> GetTimeSeries<TData, TTag> (string tagKey, TTag tagValue);
	}

	public interface ICollectorTimeSeries<TData> : ITimeSeries, IRecorder<TData>, IDescribable
	{
	}
}

