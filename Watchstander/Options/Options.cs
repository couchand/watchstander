using System;

/// <summary>
/// A general container for options enums.
/// </summary>
namespace Watchstander.Options
{
	/// <summary>
	/// Require a particular case in metric names and tag keys.
	/// </summary>
	public enum RequireCase
	{
		None,
		Lower,
		Upper
	}
}

