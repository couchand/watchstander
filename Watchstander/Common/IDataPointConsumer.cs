using System;
using System.Collections.Generic;

namespace Watchstander.Common
{
	/// <summary>
	/// An interface that represents something that can consume data points on call.
	/// </summary>
	public interface IDataPointConsumer<TValue>
	{
		/// <summary>
		/// Consume the next batch of data points.
		/// </summary>
		/// <param name="points"></param>
		void Consume(IEnumerable<IDataPoint<TValue>> points);
	}
}