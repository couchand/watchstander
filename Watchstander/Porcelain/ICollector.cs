using System;
using System.Collections.Generic;
using Watchstander.Common;

namespace Watchstander.Porcelain
{
	public interface ICollector
	{
		ICollector Disabled ();
		ICollector Reenabled ();

		void Shutdown ();

		ICollector WithName (string namePrefix);
		ICollector WithNamePrefix (string namePrefix);

		ICollector WithTag (string tagKey, string tagValue);
		ICollector WithTags (IReadOnlyDictionary<string, string> tags);

		ICollector WithTagger<TTaggable> (string tagKey, Func<TTaggable, string> tagger);
		ICollector WithTag<TTaggable> (string tagKey, TTaggable tagValue);

		ICollectorMetric<TData> GetMetric<TData> (string metricName);
	}

	public interface ICollectorMetric<TData> : IMetric
	{
		ICollectorMetric<TData> Disabled ();
		ICollectorMetric<TData> Reenabled ();

		ICollectorMetric<TData> WithTag (string tagKey, string tagValue);
		ICollectorMetric<TData> WithTags (IReadOnlyDictionary<string, string> tags);

		ICollectorMetric<TData> WithTagger<TTaggable> (string tagKey, Func<TTaggable, string> tagger);
		ICollectorMetric<TData> WithTag<TTaggable> (string tagKey, TTaggable tagValue);

		ICollectorTimeSeries<TData> GetTimeSeries ();
	}

	public interface ICollectorTimeSeries<TData> : ITimeSeries, IRecorder<TData>
	{
		ICollectorTimeSeries<TData> Disabled ();
		ICollectorTimeSeries<TData> Reenabled ();
	}
}

