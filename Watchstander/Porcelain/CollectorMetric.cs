using System;
using System.Collections.Generic;
using Watchstander.Common;
using Watchstander.Plumbing;

namespace Watchstander.Porcelain
{
	public class CollectorMetric : IMetric, IDescribable
	{
		private readonly ICollector collector;
		private readonly string name;

		private string description;
		private bool descriptionIsDirty;

		public string Name => collector.NamePrefix + name;
		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				descriptionIsDirty = true;
				description = value;
			}
		}

		public Rate Rate => Rate.Unknown;
		public string Unit => "";
		public IReadOnlyList<string> TagKeys => null;

		public IReadOnlyDictionary<string, string> Tags => collector.Tags;

		public CollectorMetric (ICollector collector, string name)
		{
			this.collector = collector;
			this.name = name;

			this.description = null;
			this.descriptionIsDirty = false;
		}
	}
}

