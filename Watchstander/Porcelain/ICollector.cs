using System;

namespace Watchstander.Porcelain
{
	public interface ICollector : INameLimitable, ITagLimitable, IDescribable
	{
		CollectorMetric GetMetric (string metricName);
	}
}

