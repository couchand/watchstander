using System;

namespace Watchstander.Porcelain
{
	public interface INameLimitable
	{
		string NamePrefix { get; }

		ICollector WithName (string namePrefix);
		ICollector WithNamePrefix (string namePrefix);
	}
}

