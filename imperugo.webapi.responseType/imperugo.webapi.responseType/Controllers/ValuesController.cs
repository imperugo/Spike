using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Mvc;

namespace imperugo.webapi.responseType.Controllers
{
	public class ValuesController : ApiController
	{
		public IEnumerable<string> Get()
		{
			this.Request.CreateResponse<string>()
			return new string[] { "value1", "value2" };
		}

		//// GET api/values
		//public HttpResponseMessage Get()
		//{
		//	return new HttpResponseMessage()
		//			{
		//				StatusCode = HttpStatusCode.OK,
		//				Content = new ObjectContent<string[]>(new string[] { "value1", "value2" }, new XmlMediaTypeFormatter())
		//			};
		//}

		// GET api/values/5
		public string Get(int id)
		{
			return "value";
		}

		// POST api/values
		public void Post(string value)
		{
		}

		// PUT api/values/5
		public void Put(int id, string value)
		{
		}

		// DELETE api/values/5
		public void Delete(int id)
		{
		}
	}
}