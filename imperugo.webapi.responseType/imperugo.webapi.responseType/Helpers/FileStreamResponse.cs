using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace imperugo.webapi.responseType.Helpers
{
	public class FileStreamResponse
	{
		public HttpResponseMessage Post(string version, string environment, string filetype)
		{
			var path = @"C:\Temp\test.exe";
			HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
			var stream = new FileStream(path, FileMode.Open);
			result.Content = new StreamContent(stream);
			result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
			return result;
		}
	}
}