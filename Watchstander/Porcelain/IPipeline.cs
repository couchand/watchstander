using System;
using Watchstander.Common;

namespace Watchstander.Porcelain
{
	public interface IPipelineElement : IDataPointConsumer<long>, IDataPointConsumer<float>
	{
	}
}

