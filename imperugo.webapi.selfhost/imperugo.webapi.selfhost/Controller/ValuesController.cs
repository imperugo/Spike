using System.Collections.Generic;
using System.Web.Http;

namespace imperugo.webapi.selfhost.Controller
{
	public class ValuesController : ApiController
	{
		public IEnumerable<string> Get()
		{
			return new[]
				       {
					       "http://tostring.it", 
						   "http://imperugo.tostring.it",
						   "http://twitter.com/imperugo",
						   "http://www.linkedin.com/in/imperugo"
				       };
		}
	}
}