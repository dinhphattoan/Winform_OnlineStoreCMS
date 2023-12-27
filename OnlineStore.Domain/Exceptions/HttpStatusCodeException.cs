using System.Net;
using System.Text.Json.Nodes;

namespace OnlineStore.Domain.Exceptions
{
	public class HttpStatusCodeException : Exception
	{
		public HttpStatusCode StatusCode { get; set; }
		public string ContentType { get; set; } = @"text/plain";

		public HttpStatusCodeException(HttpStatusCode statusCode)
		{
			this.StatusCode = statusCode;
		}

		public HttpStatusCodeException(HttpStatusCode statusCode, string message)
			: base(message)
		{
			this.StatusCode = statusCode;
		}

		public HttpStatusCodeException(HttpStatusCode statusCode, Exception inner)
			: this(statusCode, inner.ToString()) { }

		public HttpStatusCodeException(HttpStatusCode statusCode, JsonObject errorObject)
			: this(statusCode, errorObject.ToString())
		{
			this.ContentType = @"application/json";
		}

	}
}
