using System;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace imperugo.webapi.selfhost
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var config = new HttpSelfHostConfiguration("http://localhost:8080");

			config.Routes.MapHttpRoute("Api", "Api/{controller}", new {controller = "Home"});

			// Create server 
			var server = new HttpSelfHostServer(config);

			// Start the server 
			server.OpenAsync().Wait();


			Console.WriteLine("WebServer Started");
			Console.WriteLine("Press enter to exit");
			Console.ReadLine();
		}
	}
}