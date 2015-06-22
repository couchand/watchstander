using NUnit;
using NUnit.Framework;

using System;
using System.Collections.Generic;

using Watchstander.Utilities;

namespace WatchstanderTests.Unit
{
	class MyDisposable : IDisposable
	{
		private bool wasDisposed;

		public bool WasDisposed => wasDisposed;

		public MyDisposable()
		{
			wasDisposed = false;
		}

		public void Dispose()
		{
			wasDisposed = true;
		}
	}

	[TestFixture]
	public class DisposableContainerTests
	{
		[Test]
		public void TestDisposeAll ()
		{
			var container = new DisposableContainer ();
			var disposables = new List<MyDisposable> ();

			for (int i = 0; i < 10; i++)
			{
				var disposable = new MyDisposable ();
				disposables.Add (disposable);
				container.Add (disposable);
			}

			container.Dispose ();

			foreach (var disposable in disposables)
			{
				Assert.That (disposable.WasDisposed);
			}
		}
	}
}

