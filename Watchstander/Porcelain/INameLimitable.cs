using System;

namespace Watchstander.Porcelain
{
	public interface INameLimitable
	{
		string NamePrefix { get; }

		INameLimitable WithName (string namePrefix);
		INameLimitable WithNamePrefix (string namePrefix);
	}
}

