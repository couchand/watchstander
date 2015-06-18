using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using Jil;
using Watchstander.Common;

namespace Watchstander.Plumbing
{
	public class ApiOptions
	{
		public Uri InstanceUrl { get; set; }

		public SerializerOptions SerializerOptions { get; set; }

		public ApiOptions (Uri instanceUrl, SerializerOptions serializerOptions)
		{
			this.InstanceUrl = instanceUrl;
			this.SerializerOptions = serializerOptions;
		}
	}

	public class Api : IPipelineElement, IDataPointConsumer<string>
	{
		public static readonly string PUT_PATH 				= "/api/put";
		public static readonly string PUT_METADATA_PATH 	= "/api/metadata/put";

		public static readonly string LIST_METRICS_PATH		= "/api/metric";
		public static readonly string LIST_METADATA_PATH 	= "/api/metadata/get";

		public static readonly string LIST_TAGK_TEMPLATE	= "/api/tagk/{0}";	// 0: metric

		public event EventHandler<RequestInfo> BeforePost;
		public event EventHandler<RequestInfo> BeforeGet;

		protected void OnBeforePost(RequestInfo e)
		{
			if (BeforePost != null)
				BeforePost(this, e);
		}

		protected void OnBeforeGet(RequestInfo e)
		{
			if (BeforeGet != null)
				BeforeGet (this, e);
		}
		
		public class RequestInfo : EventArgs
		{
			public bool IsMetadataRequest { get; set; }
			public WebRequest Request { get; }
			public Stream Stream { get; set;}
			
			internal RequestInfo(WebRequest request, Stream stream, bool isMetadata)
			{
				this.Request = request;
				this.Stream = stream;
				this.IsMetadataRequest = isMetadata;
			}
		}

		private readonly ApiOptions options;

		private readonly Serializer dataSerializer;
		private readonly Serializer metadataSerializer;

		public Api(ApiOptions options)
		{
			this.options = options;

			var metadataOptions = new SerializerOptions (options.SerializerOptions);
			metadataOptions.ExcludeNulls = true;
			metadataOptions.TimestampPrecision = TimestampPrecision.ISO8601;

			this.dataSerializer = new Serializer (options.SerializerOptions);
			this.metadataSerializer = new Serializer (metadataOptions);
		}

		private T Get<T>(bool isMetadata, string path, Func<TextReader, T> read, IReadOnlyDictionary<string, string> query = null)
		{
			var request = WebRequest.Create (new Uri (options.InstanceUrl, path));
			request.Method = "GET";

			try
			{
				var data = new RequestInfo (request, null, isMetadata);
				OnBeforeGet (data);

				using (var response = request.GetResponse ())
				{
					var stream = response.GetResponseStream();

					using (var reader = new StreamReader(stream))
					{
						return read(reader);
					}
				}
			}
			catch (WebException ex)
			{
				// the tubes are full
				throw;
			}
		}

		private void Post(bool isMetadata, string path, Action<TextWriter> write)
		{
			var request = WebRequest.Create (new Uri (options.InstanceUrl, path));
			request.Method = "POST";
			request.ContentType = "application/json";
			try
			{
				using (var stream = request.GetRequestStream ())
				{
					var data = new RequestInfo (request, stream, isMetadata);
					OnBeforePost (data);
				
					using (var writer = new StreamWriter(data.Stream, new UTF8Encoding(false)))
					{
						write (writer);
					}
				}

				var response = request.GetResponse ();
				response.Close ();
			}
			catch (WebException ex)
			{
				// the tubes are full
				throw;
			}
		}

		public IList<string> ListMetrics()
		{
			return Get<IList<string>> (false, LIST_METRICS_PATH, dataSerializer.Read<IList<string>>);
		}

		public static string ConvertObject(object obj)
		{
			if (obj.GetType () == Type.GetType ("string"))
			{
				return (string)obj;
			}
			else
			{
				return obj.ToString ();
			}
		}

		public IList<IMetadata> ListMetadata()
		{
			var metadata = Get<IList<RemoteMetadata<object>>> (true, LIST_METADATA_PATH, metadataSerializer.Read<IList<RemoteMetadata<object>>>);

			var converted = new List<IMetadata> (metadata.Count);

			foreach (var item in metadata)
			{
				converted.Add (item.GetMetadata (ConvertObject));
			}

			return converted;
		}

		public IList<string> ListMetricTagKeys(string metric)
		{
			var path = String.Format (LIST_TAGK_TEMPLATE, metric);

			return Get<IList<string>> (false, path, metadataSerializer.Read<IList<string>>);
		}

		public void Put<T>(IDataPoint<T> point)
		{
			Post(false, PUT_PATH, dataSerializer.Write(point));
		}

		public void Put<T>(IList<IDataPoint<T>> points)
		{
			Post(false, PUT_PATH, dataSerializer.Write(points));
		}
				
		public void Consume(IEnumerable<IDataPoint<long>> points)
		{
			Put<long> (new List<IDataPoint<long>>(points));
		}
		
		public void Consume(IEnumerable<IDataPoint<float>> points)
		{
			Put<float> (new List<IDataPoint<float>>(points));
		}

		public void Consume(IEnumerable<IDataPoint<string>> points)
		{
			Put<string> (new List<IDataPoint<string>>(points));
		}

		public void PutMetadata<T>(IList<T> metadata)
		{
			Post (true, PUT_METADATA_PATH, metadataSerializer.Write(metadata));
		}
	}

	public static class GZip
	{
		public static void AddHeaderAndCompressStream(object sender, Api.RequestInfo a)
		{
			if (a.IsMetadataRequest)
				return;

			a.Request.Headers["Content-Encoding"] = "gzip";
			a.Stream = new GZipStream(a.Stream, CompressionMode.Compress);
		}
		
		public static void Use(Api api)
		{
			api.BeforePost += AddHeaderAndCompressStream;

			// api.BeforeGet += ??
		}
	}
}