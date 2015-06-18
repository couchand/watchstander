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
		public int NameLengthMinimum { get; set; }
		public int NameLengthMaximum { get; set; }
		public int NameSegmentsMinimum { get; set; }
		public int NameSegmentsMaximum { get; set; }
		public int TagCountMinimum { get; set; }
		public int TagCountMaximum { get; set; }

		public MetricValidatorOptions()
		{
			this.RequireCase = RequireCase.Lower;
			this.AllowedCharacters = ".";
			this.NameLengthMinimum = 3;
			this.NameLengthMaximum = 100; // TODO: check on a good limit
			this.NameSegmentsMinimum = 1;
			this.NameSegmentsMaximum = 12;
			this.TagCountMinimum = 1;
			this.TagCountMaximum = 6; // TODO: check on a good limit
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

			if (options.NameLengthMinimum < 1)
				options.NameLengthMinimum = 1;
			if (options.NameLengthMaximum < options.NameLengthMinimum)
				options.NameLengthMaximum = options.NameLengthMinimum;

			nameValidators.Add (ValidateLength (options.NameLengthMinimum, options.NameLengthMaximum));

			if (options.NameSegmentsMinimum < 1)
				options.NameSegmentsMinimum = 1;
			if (options.NameSegmentsMaximum < options.NameSegmentsMinimum)
				options.NameSegmentsMaximum = options.NameSegmentsMinimum;

			nameValidators.Add (ValidateSegments (options.AllowedCharacters.ToCharArray (), options.NameSegmentsMinimum, options.NameSegmentsMaximum));

			if (options.TagCountMinimum < 1)
				options.TagCountMinimum = 1;
			if (options.TagCountMaximum < options.TagCountMinimum)
				options.TagCountMaximum = options.TagCountMinimum;

			metricValidators.Add (ValidateTagCount(options.TagCountMinimum, options.TagCountMaximum));
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

		public Func<string, string> ValidateSegments(char[] sep, int min, int max)
		{
			if (min < 2) {
				return s => {
					var parts = new List<string> (s.Split (sep, max + 1));
					return parts.Count < min ? "must have at least one segment" :
						parts.Count > max ? "must have " + max + " or fewer segments" : null;
				};
			}

			return s => {
				var parts = new List<string>(s.Split(sep, max + 1));
				return parts.Count < min ? "must have at least " + min + " segments" :
					parts.Count > max ? "must have " + max + " or fewer segments" : null;
			};
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

		public Func<IMetric, string> ValidateTagCount(int min, int max)
		{
			if (min < 2)
			{
				return m => m.TagKeys.Count == 0 ? "must have at least one tag" :
					m.TagKeys.Count > max ? "must have " + max + " or fewer tags" : null;
			}

			return m => m.TagKeys.Count < min ? "must have at least " + min + " tags" :
				m.TagKeys.Count > max ? "must have " + max + " or fewer tags" : null;
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

