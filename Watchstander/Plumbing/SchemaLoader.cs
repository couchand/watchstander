using System;
using System.Collections.Generic;
using System.Linq;
using Watchstander.Common;
using Watchstander.Utilities;

namespace Watchstander.Plumbing
{
	public enum LoadMetricTagValues
	{
		None,
		ByMetric,
		Global
	}

	public class SchemaLoaderOptions
	{
		// each option assumes all the previous ones
		public bool LoadMetricMetadata { get; set; }
		public bool LoadMetricTagKeys { get; set; }
		public bool LoadTimeSeriesMetadata { get; set; }
		public LoadMetricTagValues LoadMetricTagValues { get; set; }

		public static SchemaLoaderOptions Defaults = new SchemaLoaderOptions (false) {
			LoadMetricMetadata = true,
			LoadMetricTagKeys = true,
			LoadTimeSeriesMetadata = true,
			LoadMetricTagValues = LoadMetricTagValues.None,
		};

		public SchemaLoaderOptions(bool loadDefaults = true)
		{
			if (!loadDefaults)
				return;

			CopyFrom (Defaults);
		}

		public SchemaLoaderOptions(SchemaLoaderOptions copy)
		{
			CopyFrom (copy);
		}

		private void CopyFrom(SchemaLoaderOptions copy)
		{
			this.LoadMetricMetadata = copy.LoadMetricMetadata;
			this.LoadMetricTagKeys = copy.LoadMetricTagKeys;
			this.LoadTimeSeriesMetadata = copy.LoadTimeSeriesMetadata;
			this.LoadMetricTagValues = copy.LoadMetricTagValues;
		}
	}

	public class SchemaLoader
	{
		private readonly SchemaLoaderOptions options;

		public SchemaLoader (SchemaLoaderOptions options)
		{
			this.options = new SchemaLoaderOptions(options);
		}

		private static bool areSame(string a, string b)
		{
			return String.Equals (a, b, StringComparison.OrdinalIgnoreCase);
		}

		private static void LoadMetricMetadata(SchemaEntry entry, IMetadata item)
		{
			if (areSame(item.Name, "desc"))
			{
				entry.metric.Description = item.Value;
			}
			else if (areSame(item.Name, "unit"))
			{
				entry.metric.Unit = item.Value;
			}
			else if (areSame(item.Name, "rate"))
			{
				if (areSame(item.Value, "\"counter\""))
				{
					entry.metric.Rate = Rate.Counter;
				}
				else if (areSame(item.Value, "\"gauge\""))
				{
					entry.metric.Rate = Rate.Gauge;
				}
				else if (areSame(item.Value, "\"rate\""))
				{
					entry.metric.Rate = Rate.Rate;
				}
			}
		}

		private static void LoadMetricTagKeys(SchemaEntry entry, Api api)
		{
			var tagKeys = api.ListMetricTagKeys (entry.metric.Name);
			entry.metric.AddTagKeys (tagKeys);
		}

		private static void LoadTimeSeriesMetadata(SchemaTimeSeries series, IMetadata item)
		{
			if (areSame(item.Name, "desc"))
			{
				series.Description = item.Value;
			}
		}

		private static void LoadTimeSeriesMetadata(SchemaEntry entry, IList<IMetadata> metadata)
		{
			var tagKeys = entry.Metric.TagKeys;
			Func<IReadOnlyDictionary<string, string>, string> makeKey = tags =>
			{
				var key = "";

				foreach (var tagKey in tagKeys)
				{
					if (key != "")
						key += "|";

					if (!tags.ContainsKey(tagKey))
						continue;

					key += tags[tagKey];
				}

				return key;
			};

			var seriesByKey = new Dictionary<string, IList<IMetadata>> ();

			foreach (var item in metadata)
			{
				var key = makeKey (item.Tags);

				if (!seriesByKey.ContainsKey (key))
				{
					seriesByKey [key] = new List<IMetadata> { item };
				}
				else
				{
					seriesByKey [key].Add (item);
				}
			}

			foreach (var items in seriesByKey.Values)
			{
				var series = new SchemaTimeSeries (entry.Metric, items[0]);

				foreach (var item in items)
				{
					LoadTimeSeriesMetadata (series, item);
				}

				entry.timeSeries.Add (series);
			}
		}

		private static void LoadMetricTagValues(SchemaEntry entry, LoadMetricTagValues type)
		{
			throw new Exception ("unimplemented");
		}

		private static ISchema GetSchema(IDictionary<string, SchemaEntry> schema)
		{
			return new Schema (schema);
		}

		public ISchema LoadSchema(Api api)
		{
			var metrics = api
				.ListMetrics ()
				.ToDictionary (
					name => name,
					name => new SchemaEntry (name)
				);

			if (!options.LoadMetricMetadata)
			{
				return GetSchema (metrics);
			}

			var metadata = api.ListMetadata ();

			var timeSeriesMetadata = new List<IMetadata> ();

			foreach (var item in metadata)
			{
				// host level metadata
				if (item.Metric == null)
					continue;

				// doesn't conform to the spec, should never happen
				if (!metrics.ContainsKey (item.Metric))
					continue;

				var metric = metrics [item.Metric];

				// metric level metadata
				if (item.Tags == null)
				{
					LoadMetricMetadata (metric, item);
				}
				else
				{
					timeSeriesMetadata.Add (item);
				}
			}

			if (!options.LoadMetricTagKeys)
			{
				return GetSchema (metrics);
			}

			foreach (var metric in metrics.Values)
			{
				LoadMetricTagKeys (metric, api);
			}

			if (!options.LoadTimeSeriesMetadata)
			{
				return GetSchema (metrics);
			}

			var metadataByMetric = new Dictionary<string, List<IMetadata>> ();

			foreach (var item in timeSeriesMetadata)
			{
				if (!metadataByMetric.ContainsKey (item.Metric))
				{
					metadataByMetric [item.Metric] = new List<IMetadata>{ item };
				}
				else
				{
					metadataByMetric [item.Metric].Add (item);
				}
			}

			foreach (var metric in metrics.Keys)
			{
				if (!metadataByMetric.ContainsKey (metric))
					continue;

				LoadTimeSeriesMetadata (metrics [metric], metadataByMetric [metric]);
			}

			if (options.LoadMetricTagValues == Plumbing.LoadMetricTagValues.None)
			{
				return GetSchema (metrics);
			}

			foreach (var metric in metrics.Values)
			{
				LoadMetricTagValues (metric, options.LoadMetricTagValues);
			}

			return GetSchema (metrics);
		}
	}
}

