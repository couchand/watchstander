using System;

namespace Watchstander.Expressions
{
	public class Q : SeriesSet
	{
		private QueryBuilder query;
		private IDuration start;
		private IDuration end;

		public Q (QueryBuilder query, IDuration start, IDuration end)
		{
			this.query = query;
			this.start = start;
			this.end = end;
		}

		public override string GetExpression ()
		{
			return String.Format ("q(\"{0}\",\"{1}\",\"{2}\")", query.GetQuery(), start.GetDuration(), end.GetDuration());
		}
	}

	public class Band : SeriesSet
	{
		private QueryBuilder query;
		private IDuration duration;
		private IDuration period;
		private int count;

		public Band (QueryBuilder query, IDuration duration, IDuration period, int count)
		{
			this.query = query;
			this.duration = duration;
			this.period = period;
			this.count = count;
		}

		public override string GetExpression ()
		{
			return String.Format ("band(\"{0}\",\"{1}\",\"{2}\",{3})", query.GetQuery(), duration.GetDuration(), period.GetDuration(), count);
		}
	}

	public class Change : SeriesSet
	{
		private QueryBuilder query;
		private IDuration start;
		private IDuration end;

		public Change (QueryBuilder query, IDuration start, IDuration end)
		{
			this.query = query;
			this.start = start;
			this.end = end;
		}

		public override string GetExpression ()
		{
			return String.Format ("change(\"{0}\",\"{1}\",\"{2}\")", query.GetQuery(), start.GetDuration(), end.GetDuration());
		}
	}

	public class Count : SeriesSet
	{
		private QueryBuilder query;
		private IDuration start;
		private IDuration end;

		public Count (QueryBuilder query, IDuration start, IDuration end)
		{
			this.query = query;
			this.start = start;
			this.end = end;
		}

		public override string GetExpression ()
		{
			return String.Format ("count(\"{0}\",\"{1}\",\"{2}\")", query.GetQuery(), start.GetDuration(), end.GetDuration());
		}
	}

	public class Window : SeriesSet
	{
		private QueryBuilder query;
		private IDuration duration;
		private IDuration period;
		private int count;
		private Aggregator aggregator;

		public Window (QueryBuilder query, IDuration duration, IDuration period, int count, Aggregator aggregator)
		{
			this.query = query;
			this.duration = duration;
			this.period = period;
			this.count = count;
			this.aggregator = aggregator;
		}

		public override string GetExpression ()
		{
			return String.Format ("window(\"{0}\",\"{1}\",\"{2}\",{3},\"{4}\")", query.GetQuery(), duration.GetDuration(), period.GetDuration(), count, aggregator.GetAggregator());
		}
	}

	public class Avg : NumberSet
	{
		private SeriesSet series;

		public Avg(SeriesSet series)
		{
			this.series = series;
		}

		public override string GetExpression ()
		{
			return String.Format ("avg({0})", series.GetExpression());
		}
	}

	public class Dev : NumberSet
	{
		private SeriesSet series;

		public Dev(SeriesSet series)
		{
			this.series = series;
		}

		public override string GetExpression ()
		{
			return String.Format ("dev({0})", series.GetExpression());
		}
	}

	public class Diff : NumberSet
	{
		private SeriesSet series;

		public Diff(SeriesSet series)
		{
			this.series = series;
		}

		public override string GetExpression ()
		{
			return String.Format ("diff({0})", series.GetExpression());
		}
	}

	public class First : NumberSet
	{
		private SeriesSet series;

		public First(SeriesSet series)
		{
			this.series = series;
		}

		public override string GetExpression ()
		{
			return String.Format ("first({0})", series.GetExpression());
		}
	}

	public class Last : NumberSet
	{
		private SeriesSet series;

		public Last(SeriesSet series)
		{
			this.series = series;
		}

		public override string GetExpression ()
		{
			return String.Format ("last({0})", series.GetExpression());
		}
	}

	public class Len : NumberSet
	{
		private SeriesSet series;

		public Len(SeriesSet series)
		{
			this.series = series;
		}

		public override string GetExpression ()
		{
			return String.Format ("len({0})", series.GetExpression());
		}
	}

	public class Max : NumberSet
	{
		private SeriesSet series;

		public Max(SeriesSet series)
		{
			this.series = series;
		}

		public override string GetExpression ()
		{
			return String.Format ("max({0})", series.GetExpression());
		}
	}

	public class Median : NumberSet
	{
		private SeriesSet series;

		public Median(SeriesSet series)
		{
			this.series = series;
		}

		public override string GetExpression ()
		{
			return String.Format ("median({0})", series.GetExpression());
		}
	}

	public class Min : NumberSet
	{
		private SeriesSet series;

		public Min(SeriesSet series)
		{
			this.series = series;
		}

		public override string GetExpression ()
		{
			return String.Format ("min({0})", series.GetExpression());
		}
	}

	public class Since : NumberSet
	{
		private SeriesSet series;

		public Since(SeriesSet series)
		{
			this.series = series;
		}

		public override string GetExpression ()
		{
			return String.Format ("since({0})", series.GetExpression());
		}
	}

	public class Streak : NumberSet
	{
		private SeriesSet series;

		public Streak(SeriesSet series)
		{
			this.series = series;
		}

		public override string GetExpression ()
		{
			return String.Format ("streak({0})", series.GetExpression());
		}
	}

	public class Sum : NumberSet
	{
		private SeriesSet series;

		public Sum(SeriesSet series)
		{
			this.series = series;
		}

		public override string GetExpression ()
		{
			return String.Format ("sum({0})", series.GetExpression());
		}
	}

	public class ForecastLR : NumberSet
	{
		private SeriesSet series;
		private Scalar target;

		public ForecastLR(SeriesSet series, Scalar target)
		{
			this.series = series;
			this.target = target;
		}

		public override string GetExpression ()
		{
			return String.Format ("forecastlr({0},{1})", series.GetExpression(), target.GetExpression());
		}
	}

	public class Percentile : NumberSet
	{
		private SeriesSet series;
		private Scalar level;

		public Percentile(SeriesSet series, Scalar level)
		{
			this.series = series;
			this.level = level;
		}

		public override string GetExpression ()
		{
			return String.Format ("percentile({0},{1})", series.GetExpression(), level.GetExpression());
		}
	}
}

