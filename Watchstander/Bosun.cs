using System;
using Watchstander.Plumbing;
using Watchstander.Porcelain;
using Watchstander.Utilities;

namespace Watchstander
{
	public static class Bosun
	{
		public static ICollector Collector (CollectorOptions options)
		{
			var uriValidator = new UriValidator ();
			uriValidator.Validate (options.InstanceUrl);

			Console.WriteLine ("Starting Bosun collector for url " + options.InstanceUrl);
			var api = new Api (options.ApiOptions);

			Console.WriteLine ("Creating disposables holder");
			var disposables = new DisposableContainer ();

			Console.WriteLine ("Creating metrics queue");
			var metricsQueue = new DataQueue ();

			Console.WriteLine ("Creating metrics flusher");
			var flushOptions = new FlusherOptions (metricsQueue, api);
			flushOptions.Timeout = options.FlushTimeout;
			var apiFlusher = new Flusher (flushOptions);
			disposables.Add (apiFlusher);

			Console.WriteLine ("Creating accumulating schema");
			var schemaOptions = new AccumulatingSchemaOptions ();
			schemaOptions.AllowMetadataUpdates = options.AllowMetadataUpdates;
			var schema = new AccumulatingSchema (schemaOptions);

			Console.WriteLine ("Creating collector context");
			var context = new CollectorContext (metricsQueue, disposables, schema);

			Console.WriteLine ("Creating collector");
			return new RootCollector (context);
		}
	}
}

