using System;

namespace Watchstander.Porcelain
{
	public interface IRecorder<TData>
	{
		void Record(TData data, DateTime now);
	}
}

