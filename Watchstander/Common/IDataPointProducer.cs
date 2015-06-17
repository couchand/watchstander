using System;
using System.Collections.Generic;

namespace Watchstander.Common
{
	/// <summary>
	/// An interface that represents something that can produce data points (maybe) on call.
	/// </summary>
	public interface IDataPointProducer<TValue>
	{
		/// <summary>
		/// Produce the next batch of data points.
		/// </summary>
		/// <returns>Any data points that can be produced synchronously.</returns>
		IEnumerable<IDataPoint<TValue>> Produce();
	}
}