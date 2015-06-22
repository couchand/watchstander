using System;
using System.Linq;
using System.Threading;
using Watchstander.Common;
using Watchstander.Utilities;

namespace Watchstander.Plumbing
{
	public class MetadataSenderOptions
	{
		public ISchema Schema { get; }
		public Api Api { get; }

		public TimeSpan Timeout { get; set; }
		public int BatchSize { get; set; }

		public MetadataSenderOptions(ISchema Schema, Api Api)
		{
			this.Schema = Schema;
			this.Api = Api;
			this.Timeout = new TimeSpan (TimeSpan.TicksPerSecond * 60);
			this.BatchSize = 250;
		}
	}

	public class MetadataSender : IDisposable
	{
		private readonly ISchema schema;
		private readonly Api api;
		private readonly int batchSize;

		private FlusherState state;
		private Timer timer;

		public MetadataSender (MetadataSenderOptions options)
		{
			this.schema = options.Schema;
			this.api = options.Api;
			this.batchSize = options.BatchSize;

			this.state = FlusherState.Timer;
			this.timer = new Timer (Flush, FlusherState.Timer, options.Timeout, options.Timeout);
		}

		private void Flush(object payload)
		{
			var flushState = (FlusherState)payload;

			var shuttingDown = state == FlusherState.Shutdown;

			if (shuttingDown && flushState != FlusherState.Shutdown)
			{
				return;
			}

			var stateIsLegal = shuttingDown || (state == FlusherState.Timer && flushState == FlusherState.Timer);

			if (!stateIsLegal)
			{
				throw new Exception ("MetadataSender state error");
			}

			var batches = schema.Entries.Values.BreakIntoBatches (batchSize / 3);

			foreach (var batch in batches)
			{
				var metrics = batch.Select (e => e.Metric);

				var descriptions = metrics
					.Where (m => m.Description != null)
					.Select (MetadataFactory.GetDescriptionMetadata);
				var rates = metrics
					.Where (m => m.Rate != Rate.Unknown)
					.Select (MetadataFactory.GetRateMetadata);
				var units = metrics
					.Where (m => m.Unit != null)
					.Select (MetadataFactory.GetUnitMetadata);

				var metadata = descriptions.Concat (rates).Concat (units).ToList();

				api.PutMetadata<IMetadata> (metadata);

				var series = batch.SelectMany (e => e.TimeSeries);

				var seriesDescriptions = series
					.Where (s => s.Description != null)
					.Select (MetadataFactory.GetDescriptionMetadata)
					.ToList();

				api.PutMetadata<IMetadata> (seriesDescriptions);
			}
		}

		public void Shutdown()
		{
			if (state == FlusherState.Shutdown)
				return;

			state = FlusherState.Shutdown;

			var waitHandle = new AutoResetEvent (false);
			timer.Dispose (waitHandle);
			waitHandle.WaitOne ();
		}

		public void Dispose()
		{
			Shutdown ();
		}
	}
}