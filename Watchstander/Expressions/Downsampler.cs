using System;

namespace Watchstander.Expressions
{
	public class Downsampler : IQuerySegment
	{
		internal IDuration duration;
		internal Aggregator aggregator;

		public Downsampler (IDuration duration, Aggregator aggregator)
		{
			this.duration = duration;
			this.aggregator = aggregator;
		}

		public string GetDownsampler()
		{
			return String.Format ("{0}-{1}", duration.GetDuration(), aggregator.GetAggregator());
		}

		public string GetQuerySegment()
		{
			return GetDownsampler ();
		}
	}
}

