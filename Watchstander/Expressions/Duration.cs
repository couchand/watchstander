using System;
using System.Collections.Generic;

namespace Watchstander.Expressions
{
	public interface IDuration
	{
		string GetDuration();
	}

	public class CurrentTime : IDuration
	{
		public string GetDuration()
		{
			return "";
		}
	}

	public class StringDuration : IDuration
	{
		internal string value;

		public StringDuration(string value)
		{
			this.value = value;
		}

		public string GetDuration()
		{
			return value;
		}
	}

	public class RelativeDuration : IDuration
	{
		public static double MIN_UNIT_COUNT = 1;
		public static double MAX_UNIT_COUNT = 1000;

		private TimeSpan difference;

		public RelativeDuration(TimeSpan difference)
		{
			this.difference = difference;
		}

		public string GetDuration()
		{
			var relative = getRelative ();

			var count = relative.Item1;
			var unit = relative.Item2;

			return String.Format ("{0}{1}", count, unit);
		}

		private Tuple<int, string> getRelative()
		{
			var units = new List<Tuple<string, string>> ()
			{
				new Tuple<string, string> ("Years", "y"),
				new Tuple<string, string> ("Months", "n"),
				new Tuple<string, string> ("Weeks", "w"),
				new Tuple<string, string> ("Days", "d"),
				new Tuple<string, string> ("Hours", "h"),
				new Tuple<string, string> ("Minutes", "m"),
				new Tuple<string, string> ("Seconds", "s"),
				new Tuple<string, string> ("Milliseconds", "ms")
			};

			foreach (var unit in units)
			{
				var name = unit.Item1;
				var abbr = unit.Item2;
				var multiplier = 1;

				switch (name)
				{
				case "Weeks":
					name = "Days";
					multiplier = 7;
					break;
				case "Months":
					name = "Days";
					multiplier = 30;
					break;
				case "Years":
					name = "Days";
					multiplier = 365;
					break;
				}

				var diffValue = double.Parse(difference
					.GetType ()
					.GetProperty (name)
					.GetValue (difference)
					.ToString());

				var inUnit = diffValue / multiplier;

				if (inUnit >= MIN_UNIT_COUNT && inUnit <= MAX_UNIT_COUNT)
				{
					var count = (int)inUnit;
					return new Tuple<int, string> (count, abbr);
				}
			}

			if (difference < TimeSpan.FromMilliseconds (10))
			{
				return new Tuple<int, string> (10, "ms");
			}
			else
			{
				return new Tuple<int, string> (1, "y");
			}
		}
	}
}

