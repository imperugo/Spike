using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.SelfHost;
using SharpTestsEx;
using Xunit;
using imperugo.webapi.selfhost.Controller;

namespace imperugo.webapi.integrationTest
{
	public class ValuesControllerWithoutBaseClass
	{
		[Fact]
		public void ValueController_WithGetMethos_ShouldReturnValidData_NoBaseClass()
		{
			HttpSelfHostConfiguration configuration = new HttpSelfHostConfiguration("http://localhost:8080");
			configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
			configuration.Services.Replace(typeof(IAssembliesResolver), new WebApiClassBase.TestAssemblyResolver(typeof(ValuesController)));
			configuration.Routes.MapHttpRoute("Default", "{controller}", new { controller = "Home" });

			HttpSelfHostServer server = new HttpSelfHostServer(configuration);
			try
			{
				server.OpenAsync().Wait();

				var request = new HttpRequestMessage();

				request.RequestUri = new Uri("http://localhost:8080");

				request.Method = HttpMethod.Get;

				var client = new HttpClient(server);
				using (HttpResponseMessage response = client.SendAsync(request).Result)
				{
					response.Should().Not.Be.Null();
					response.IsSuccessStatusCode.Should().Be.True();

					string[] result = response.Content.ReadAsAsync<string[]>().Result;

					result.Length.Should().Be.EqualTo(4);
					result[0].Should().Be.EqualTo("http://tostring.it");
					result[1].Should().Be.EqualTo("http://imperugo.tostring.it");
					result[2].Should().Be.EqualTo("http://twitter.com/imperugo");
					result[3].Should().Be.EqualTo("http://www.linkedin.com/in/imperugo");
				}
			}
			finally
			{
				configuration.Dispose();
				server.Dispose();
			}
		}
	}
}
