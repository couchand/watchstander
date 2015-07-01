using System;
using System.Collections.Generic;
using System.Linq;

namespace Watchstander.Expressions
{
	public interface IQuery
	{
		string GetQuery ();
	}

	public class QueryBuilder : IQuery
	{
		internal readonly MetricQuery metric;
		internal readonly Aggregator aggregator;
		internal readonly Downsampler downsampler;
		internal readonly RateOptions rateOptions;

		public QueryBuilder (MetricQuery metric, Aggregator aggregator, Downsampler downsampler, RateOptions rateOptions)
		{
			this.metric = metric;
			this.aggregator = aggregator;
			this.downsampler = downsampler;
			this.rateOptions = rateOptions;
		}

		public QueryBuilder (string metric)
			: this(new MetricQuery(metric), Aggregators.Sum, null, null) {}

		public QueryBuilder (MetricQuery metric)
			: this(metric, Aggregators.Sum, null, null) {}

		public QueryBuilder (string metric, Aggregator aggregator)
			: this(new MetricQuery(metric), aggregator, null, null) {}

		public QueryBuilder (MetricQuery metric, Aggregator aggregator)
			: this(metric, aggregator, null, null) {}

		public string GetQuery()
		{
			var segments = new List<IQuerySegment>
			{
				aggregator
			};

			if (downsampler != null)
				segments.Add (downsampler);

			if (rateOptions != null)
				segments.Add (rateOptions);

			segments.Add (metric);

			return String.Join (
				":",
				segments.Select(s => s.GetQuerySegment())
			);
		}

		public QueryBuilder WithAggregator(Aggregator newAggregator)
		{
			return new QueryBuilder (metric, newAggregator, downsampler, rateOptions);
		}

		public QueryBuilder WithDownsample(TimeSpan timeSpan, Aggregator aggregator)
		{
			return WithDownsample(new RelativeDuration(timeSpan), aggregator);
		}

		public QueryBuilder WithDownsample(string duration, Aggregator aggregator)
		{
			return WithDownsample (new StringDuration (duration), aggregator);
		}

		public QueryBuilder WithDownsample(IDuration duration, Aggregator sampleAggregator)
		{
			var newDownsampler = new Downsampler (duration, sampleAggregator);
			return new QueryBuilder (metric, aggregator, newDownsampler, rateOptions);
		}

		public QueryBuilder WithCounter()
		{
			return new QueryBuilder (metric, aggregator, downsampler, new RateOptions (null, null));
		}

		public QueryBuilder WithCounter(int counterMax, int resetValue)
		{
			return new QueryBuilder (metric, aggregator, downsampler, new RateOptions (counterMax, resetValue));
		}

		public QueryBuilder WithCounterMax(int counterMax)
		{
			RateOptions newRate;

			if (rateOptions == null)
			{
				newRate = new RateOptions (counterMax, null);
			}
			else
			{
				newRate = rateOptions.WithCounterMax (counterMax);
			}

			return new QueryBuilder (metric, aggregator, downsampler, newRate);
		}

		public QueryBuilder WithCounterReset(int resetValue)
		{
			RateOptions newRate;

			if (rateOptions == null)
			{
				newRate = new RateOptions (null, resetValue);
			}
			else
			{
				newRate = rateOptions.WithResetValue (resetValue);
			}

			return new QueryBuilder (metric, aggregator, downsampler, newRate);
		}
	}
}

