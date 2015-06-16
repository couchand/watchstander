/*
 * Rate.cs
 *
 * The three options for rate in Bosun.
 *
 */

using System;

namespace Watchstander.Common
{
	public enum Rate
	{
		Unknown,
		Gauge,
		Counter,
		Rate
	}

	public static class RateExtensions
	{
		public static string ToMetadataString(this Rate rate)
		{
			return rate.ToString().ToLower();
		}
	}
}
