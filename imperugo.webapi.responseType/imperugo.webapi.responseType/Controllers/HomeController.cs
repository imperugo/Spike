using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace imperugo.webapi.responseType.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult Get()
		{
			return Json(new string[] {"value1", "value2"}, JsonRequestBehavior.AllowGet);
		}
	}
}
