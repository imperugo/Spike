using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.SelfHost;
using SharpTestsEx;
using Xunit;
using imperugo.webapi.selfhost.Controller;

namespace imperugo.webapi.integrationTest
{
	public class ValuesControllerTest : WebApiClassBase
	{
		public ValuesControllerTest()
			: base("localhost", 8080, typeof (ValuesController))
		{
		}


		[Fact]
		public void ValueController_WithGetMethos_ShouldReturnValidData()
		{
			base.Start();

			var response = base.CreateRequest("/Values", HttpMethod.Get);

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
}