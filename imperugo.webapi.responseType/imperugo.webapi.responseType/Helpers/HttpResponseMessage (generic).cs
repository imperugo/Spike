using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace imperugo.webapi.responseType.Helpers
{
	public class HttpResponseMessage<T> : HttpResponseMessage
	{
		public HttpResponseMessage(T value, HttpStatusCode statusCode, MediaTypeFormatter formatter)
		{
			StatusCode = statusCode;
			Content = new ObjectContent<T>(value, formatter);
		}
	}
}