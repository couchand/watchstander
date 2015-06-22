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
			var flushOptions = new FlusherOptions (metricsQueue, api);
			flushOptions.Timeout = options.FlushTimeout;

			Console.WriteLine ("Creating metrics flusher");
			var apiFlusher = new Flusher (flushOptions);
			disposables.Add (apiFlusher);

			Console.WriteLine ("Creating collector");
			return new RootCollector (metricsQueue, disposables);
		}
	}
}

