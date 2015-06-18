using System;
using System.IO;
using Jil;

namespace Watchstander.Plumbing
{
	public enum TimestampPrecision
	{
		Seconds,
		Milliseconds,
		ISO8601
	}

	public class SerializerOptions
	{
		public TimestampPrecision TimestampPrecision { get; set; }
		public bool PrettyPrint { get; set; }
		public bool ExcludeNulls { get; set; }

		public static SerializerOptions Defaults = new SerializerOptions (false) {
			TimestampPrecision = TimestampPrecision.Milliseconds,
			PrettyPrint = false
		};

		public SerializerOptions(bool loadDefaults = true)
		{
			if (!loadDefaults)
				return;

			CopyFrom (Defaults);
		}

		public SerializerOptions(SerializerOptions copy)
		{
			CopyFrom (copy);
		}

		private void CopyFrom(SerializerOptions copy)
		{
			this.TimestampPrecision = copy.TimestampPrecision;
			this.PrettyPrint = copy.PrettyPrint;
		}
	}

	/// <summary>
	/// A thin wrapper around Jil to provide an Action<TextWriter>.
	/// </summary>
	public class Serializer
	{
		private readonly Jil.Options options;

		public Serializer (SerializerOptions options)
		{
			this.options = GetSerializerOptions (options);
		}

		private static Jil.Options GetSerializerOptions(SerializerOptions options)
		{
			DateTimeFormat dateFormat;
			switch (options.TimestampPrecision)
			{
			case TimestampPrecision.ISO8601:
				// Bosun metadata response
				dateFormat = DateTimeFormat.ISO8601;
				break;

			case TimestampPrecision.Seconds:
				dateFormat = DateTimeFormat.SecondsSinceUnixEpoch;
				break;

			default:
			case TimestampPrecision.Milliseconds:
				dateFormat = DateTimeFormat.MillisecondsSinceUnixEpoch;
				break;
			}

			return new Jil.Options (
				dateFormat: dateFormat,
				prettyPrint: options.PrettyPrint,
				excludeNulls: options.ExcludeNulls
			);
		}

		public Action<TextWriter> Write<T>(T data)
		{
			return writer => Write (data, writer);
		}

		private void Write<T>(T data, TextWriter writer)
		{
			try
			{
				JSON.Serialize<T>(data, writer, options);
			}
			catch (InvalidOperationException ex)
			{
				// Jil internal error
				throw;
			}
			catch (InfiniteRecursionException ex)
			{
				// We must have screwed something up
				throw;
			}
		}

		public T Read<T>(TextReader reader)
		{
			try
			{
				return JSON.Deserialize<T>(reader, options);
			}
			catch (DeserializationException ex)
			{
				// this one's the API's fault
				throw;
			}
			catch (InvalidOperationException ex)
			{
				// Jil internal error
				throw;
			}
			catch (InfiniteRecursionException ex)
			{
				// We must have screwed something up
				throw;
			}
		}
	}
}

