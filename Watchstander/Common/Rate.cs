/*
 * Rate.cs
 *
 * The three options for rate in Bosun.
 *
 */

using System;

namespace Watchstander.Common
{
	/// <summary>
	/// The Bosun metadata Rate.
	/// </summary>
	public enum Rate
	{
		/// <summary>
		/// An unknown rate value.
		/// </summary>
		Unknown,

		/// <summary>
		/// A gauge-type metric.
		/// </summary>
		Gauge,

		/// <summary>
		/// A counter-type metric.
		/// </summary>
		Counter,

		/// <summary>
		/// A metric measuring a rate.
		/// </summary>
		Rate
	}

	public static class RateExtensions
	{
		/// <summary>
		/// Convert the rate to a string for the purpose of metadata upload to Bosun.
		/// </summary>
		/// <returns>The metadata string.</returns>
		/// <param name="rate"></param>
		public static string ToMetadataString(this Rate rate)
		{
			return rate.ToString().ToLower();
		}
	}
}
