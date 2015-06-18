using System;
using System.Linq;
using System.Threading;

namespace Watchstander.Plumbing
{
	public class FlusherOptions
	{
		public DataQueue Queue { get; set; }
		public IPipelineElement Consumer { get; set; }
		public TimeSpan Timeout { get; set; }
		public int BatchSize { get; set; }

		public FlusherOptions(DataQueue Queue, IPipelineElement Consumer)
		{
			this.Queue = Queue;
			this.Consumer = Consumer;

			this.Timeout = new TimeSpan (TimeSpan.TicksPerSecond * 1);
			this.BatchSize = 250;
		}
	}

	public enum FlusherState
	{
		Unknown,
		Timer,
		Shutdown,
		Flushing
	}

	public class Flusher : IDisposable
	{
		private readonly DataQueue queue;
		private readonly IPipelineElement consumer;
		private readonly int batchSize;

		private FlusherState state;
		private Timer timer;

		public Flusher (FlusherOptions options)
		{
			this.queue = options.Queue;
			this.consumer = options.Consumer;
			this.batchSize = options.BatchSize;

			this.state = FlusherState.Timer;
			this.timer = new Timer (Flush, FlusherState.Timer, options.Timeout, options.Timeout);
		}

		private void Flush(object payload)
		{
			var flushState = (FlusherState)payload;

			var shuttingDown = state == FlusherState.Shutdown;

			if (shuttingDown && flushState != FlusherState.Shutdown)
			{
				return;
			}

			var stateIsLegal = shuttingDown || (state == FlusherState.Timer && flushState == FlusherState.Timer);

			if (!stateIsLegal)
			{
				throw new Exception ("Flusher state error");
			}

			bool done = false;

			while (!done)
			{
				var longs = queue.TakeLongs (batchSize);
				var floats = queue.TakeFloats (batchSize);


				if (longs.Count() > 0)
				{
					consumer.Consume (longs);
				}

				if (floats.Count() > 0)
				{
					consumer.Consume (floats);
				}

				if (state == FlusherState.Shutdown && (longs.Count() == 0 || floats.Count() == 0 ))
				{
					done = true;
				}
			}
		}

		public void Shutdown()
		{
			if (state == FlusherState.Shutdown)
				return;

			state = FlusherState.Shutdown;

			var waitHandle = new AutoResetEvent (false);
			timer.Dispose (waitHandle);
			waitHandle.WaitOne ();

			Flush (FlusherState.Shutdown);
		}

		public void Dispose()
		{
			Shutdown ();
		}
	}
}