using System;
using System.Collections.Generic;

namespace Watchstander.Common
{
	public interface IDataPointProducer<TValue>
	{
		IEnumerable<IDataPoint<TValue>> Produce();
	}
}