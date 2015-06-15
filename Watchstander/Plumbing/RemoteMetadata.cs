using System;
using System.Collections.Generic;
using Watchstander.Common;
using Watchstander.Utilities;

namespace Watchstander.Plumbing
{
	public class RemoteMetadata<TValue>
	{
		public string Metric { get; set; }
		public Dictionary<string, string> Tags { get; set; }
		public string Name { get; set; }
		public TValue Value { get; set; }
		public DateTime Time { get; set; }

		public IMetadata GetMetadata(Func<TValue, string> stringer)
		{
			return new BasicMetadata (
				Metric,
				Tags.AsReadOnly(),
				Name,
				stringer(Value),
				Time
			);
		}
	}
}

