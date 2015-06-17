using System;

namespace Watchstander.Porcelain
{
	public class NameLimiter
	{
		public string NamePrefix { get; }

		public NameLimiter ()
			: this ("") {}

		public NameLimiter (string NamePrefix)
		{
			this.NamePrefix = NamePrefix;
		}

		public NameLimiter AddPrefix (string prefix)
		{
			return new NameLimiter(NamePrefix + prefix);
		}

		public NameLimiter Add(string name)
		{
			return AddPrefix (name + ".");
		}

		public string Resolve(string name)
		{
			return NamePrefix + name;
		}
	}
}

