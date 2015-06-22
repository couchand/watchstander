using System;
using System.Collections.Generic;
using Watchstander.Common;

namespace Watchstander.Plumbing
{
	public class CollectorContext
	{
		public IPipelineElement DataConsumer { get; }
		public IDisposable Disposables { get; }
		public AccumulatingSchema Schema { get; }

		public CollectorContext (IPipelineElement DataConsumer, IDisposable Disposables, AccumulatingSchema Schema)
		{
			this.DataConsumer = DataConsumer;
			this.Disposables = Disposables;
			this.Schema = Schema;
		}
	}
}

