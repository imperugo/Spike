using System.Web.Mvc;

namespace imperugo.linkflood.Controllers{
	public class HomeController : Controller{
		public ActionResult Index(){
			ViewBag.Message = "It's a scheduler. Nothing to show!";

			return View();
		}
	}
}