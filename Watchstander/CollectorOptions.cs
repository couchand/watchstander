using System;
using Watchstander.Plumbing;

namespace Watchstander
{
	public class CollectorOptions
	{
		public Uri InstanceUrl { get; set; }
		public TimeSpan FlushTimeout { get; set; }

		internal ApiOptions ApiOptions => new ApiOptions(InstanceUrl, new SerializerOptions());

		public CollectorOptions (Uri InstanceUrl)
		{
			this.InstanceUrl = InstanceUrl;
			this.FlushTimeout = new TimeSpan(TimeSpan.TicksPerSecond * 1);
		}
	}
}

