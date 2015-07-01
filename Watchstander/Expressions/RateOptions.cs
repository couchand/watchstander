using System;

namespace Watchstander.Expressions
{
	public class RateOptions : IQuerySegment
	{
		internal int? counterMax;
		internal int? resetValue;

		public RateOptions (int? counterMax, int? resetValue)
		{
			this.counterMax = counterMax;
			this.resetValue = resetValue;
		}

		public string GetRate()
		{
			var max = counterMax.HasValue ? counterMax.Value.ToString () : "";
			var reset = resetValue.HasValue ? resetValue.Value.ToString () : "";

			return String.Format ("rate{{counter,{0},{1}}}", max, reset);
		}

		public string GetQuerySegment()
		{
			return GetRate();
		}

		public RateOptions WithCounterMax (int newMax)
		{
			return new RateOptions (newMax, resetValue);
		}

		public RateOptions WithResetValue (int newReset)
		{
			return new RateOptions (counterMax, newReset);
		}
	}
}

