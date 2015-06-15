using System;
using System.Collections.Generic;

namespace Watchstander.Common
{
	public interface IDataPointConsumer<TValue>
	{
		void Consume(IEnumerable<IDataPoint<TValue>> points);
	}
}