using System;
using Watchstander.Common;

namespace Watchstander.Plumbing
{
	public interface IPipelineElement : IDataPointConsumer<long>, IDataPointConsumer<float>
	{
	}
}

