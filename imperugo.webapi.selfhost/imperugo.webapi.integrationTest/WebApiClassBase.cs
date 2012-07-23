using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.SelfHost;
using SharpTestsEx;

namespace imperugo.webapi.integrationTest
{
	public abstract class WebApiClassBase : IDisposable
	{
		private readonly string baseAddress;
		private HttpSelfHostConfiguration configuration;
		private HttpSelfHostServer server;
		private readonly Type controllerType;

		protected WebApiClassBase(Type controllerType)
			: this("localhost", 8080, controllerType)
		{
			
		}

		protected WebApiClassBase(string host, int port, Type controllerType)
		{
			this.controllerType = controllerType;
			if (string.IsNullOrEmpty(host))
			{
				host = "localhost";
			}

			baseAddress = string.Format("http://{0}:{1}", host, port);
		}

		public virtual HttpSelfHostConfiguration Configuration
		{
			get
			{
				if(configuration == null)
				{
					configuration = new HttpSelfHostConfiguration(baseAddress);
					configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
					configuration.Services.Replace(typeof(IAssembliesResolver), new TestAssemblyResolver(controllerType));
					configuration.Routes.MapHttpRoute("Default", "{controller}", new { controller = "Home" });
				}

				return configuration;
			}
		}

		public virtual HttpSelfHostServer Server
		{
			get { return server ?? (server = new HttpSelfHostServer(Configuration)); }
		}

		public string BaseAddress
		{
			get { return baseAddress; }
		}

		public void Start()
		{
			Server.OpenAsync().Wait();
		}

		public void Close()
		{
			Server.CloseAsync().Wait();
		}

		protected HttpResponseMessage CreateRequest(string url, HttpMethod method, string acceptedMediaType = null)
		{
			var request = new HttpRequestMessage();

			request.RequestUri = new Uri(baseAddress + url);

			if (acceptedMediaType != null)
				request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptedMediaType));

			request.Method = method;

			var client = new HttpClient(this.Server);
			using (HttpResponseMessage response = client.SendAsync(request).Result)
			{
				return response;
			}
		}

		#region Implementation of IDisposable

		public void Dispose()
		{
			if (configuration != null)
			{
				configuration.Dispose();
				configuration = null;
			}

			if (server != null)
			{
				server.Dispose();
				server = null;
			}
		}

		#endregion

		public class TestAssemblyResolver : IAssembliesResolver
		{
			private readonly Type controllerType;

			public TestAssemblyResolver(Type controllerType)
			{
				this.controllerType = controllerType;
			}

			public ICollection<Assembly> GetAssemblies()
			{
				List<Assembly> baseAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

				if (!baseAssemblies.Contains(controllerType.Assembly))
					baseAssemblies.Add(controllerType.Assembly);

				return baseAssemblies;
			}
		}
	}
}