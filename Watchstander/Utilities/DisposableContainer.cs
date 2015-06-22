using System;
using System.Collections.Generic;

namespace Watchstander.Utilities
{
	public class DisposableContainer : IDisposable
	{
		private readonly List<IDisposable> disposables;

		public DisposableContainer ()
		{
			disposables = new List<IDisposable> ();
		}

		public void Add(IDisposable disposable)
		{
			disposables.Add (disposable);
		}

		public void Dispose()
		{
			foreach (var disposable in disposables)
			{
				disposable.Dispose ();
			}
		}
	}
}

