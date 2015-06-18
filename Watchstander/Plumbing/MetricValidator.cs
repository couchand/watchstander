using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Watchstander.Common;
using Watchstander.Options;

namespace Watchstander.Plumbing
{
	public class MetricValidatorOptions
	{
		public RequireCase RequireCase { get; set; }
		public string AllowedCharacters { get; set; }
		public int MinimumLength { get; set; }
		public int MaximumLength { get; set; }

		public MetricValidatorOptions()
		{
			this.RequireCase = RequireCase.Lower;
			this.AllowedCharacters = ".";
			this.MinimumLength = 3;
			this.MaximumLength = 100; // TODO: check on a good limit
		}
	}

	public class MetricValidator
	{
		List<Func<string, string>> nameValidators;
		List<Func<IMetric, string>> metricValidators;

		public MetricValidator (MetricValidatorOptions options)
		{
			nameValidators = new List<Func<string, string>> ();
			metricValidators = new List<Func<IMetric, string>> ();

			if (options.RequireCase != RequireCase.None)
			{
				nameValidators.Add (ValidateCase (options.RequireCase));
			}

			if (options.AllowedCharacters != null)
			{
				nameValidators.Add (ValidateCharacters (options.AllowedCharacters));
			}

			if (options.MinimumLength < 1)
				options.MinimumLength = 1;
			if (options.MaximumLength < options.MinimumLength)
				options.MaximumLength = options.MinimumLength;

			nameValidators.Add (ValidateLength (options.MinimumLength, options.MaximumLength));
		}

		public Func<string, string> ValidateCase(RequireCase force)
		{
			Func<string, string> m = force == RequireCase.Lower
				? (Func<string, string>)(s => s.ToLower ())
				: (Func<string, string>)(s => s.ToUpper ());
			string e = "should be " + force.ToString ().ToLower () + " case";
			
			return s => s == m (s) ? null : e;
		}

		public Func<string, string> ValidateCharacters(string allowed)
		{
			var pattern = "^[A-Za-z" + allowed + "]+$";
			var re = new Regex (pattern);

			return s => re.IsMatch (s) ? null : "disallowed character found";
		}

		public Func<string, string> ValidateLength(int min, int max)
		{
			return s => s.Length < min ? "too short" :
				s.Length > max ? "too long" : null;
		}

		public void ValidateName(string metricName)
		{
			foreach (var validator in nameValidators)
			{
				var error = validator (metricName);

				if (error != null)
				{
					var msg = String.Format ("Name \"{0}\"invalid: {1}", metricName, error);
					throw new Exception (msg);
				}
			}
		}

		public void Validate(IMetric metric)
		{
			foreach (var validator in metricValidators)
			{
				var error = validator (metric);

				if (error != null)
				{
					throw new Exception ("Metric invalid: " + error);
				}
			}
		}
	}
}

