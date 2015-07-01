using System;

namespace Watchstander.Expressions
{
	public class Aggregator : IQuerySegment
	{
		internal string aggregator;

		public Aggregator (string aggregator)
		{
			this.aggregator = aggregator;
		}

		public string GetAggregator()
		{
			return aggregator;
		}

		public string GetQuerySegment()
		{
			return GetAggregator ();
		}
	}

	public static class Aggregators
	{
		public static readonly Aggregator Avg = new Aggregator("avg");
		public static readonly Aggregator Dev = new Aggregator("dev");
		public static readonly Aggregator Max = new Aggregator("max");
		public static readonly Aggregator Min = new Aggregator("min");
		public static readonly Aggregator MimMax = new Aggregator("mimmax");
		public static readonly Aggregator MimMin = new Aggregator("mimmin");
		public static readonly Aggregator Sum = new Aggregator("sum");
		public static readonly Aggregator ZimSum = new Aggregator("zimsum");
	}
}

