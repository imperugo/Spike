using System.Globalization;
using System.Web.Mvc;

namespace imperugo.jsLocalization.Controllers{
	public class LocalizationController : Controller{
		
		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult DateFormat(string id){
			if(string.IsNullOrEmpty(id)){
				id = "en-US";
			}

			var culture = new CultureInfo(id);

			return Json(culture.DateTimeFormat,JsonRequestBehavior.AllowGet);
		}
	}
}