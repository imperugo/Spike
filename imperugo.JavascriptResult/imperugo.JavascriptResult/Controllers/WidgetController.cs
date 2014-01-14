namespace imperugo.JavascriptResult.Controllers
{
	using System;
	using System.Web.Mvc;

	public class WidgetController : Controller
	{
		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult MyWidget(int id)
		{
			PartialModel model = new PartialModel();

			if (id == 1)
			{
				model.Firstname = "Ugo";
				model.Lastname = "Lattanzi";
				model.Website = new Uri("http://tostring.it");
			}

			if (id == 2)
			{
				model.Firstname = "Matteo";
				model.Lastname = "Pagani";
				model.Website = new Uri("http://wp.qmatteoq.com");
			}

			if (id == 3)
			{
				model.Firstname = "Daniel";
				model.Lastname = "Crisp";
				model.Website = new Uri("http://danielcrisp.com/");
			}

			return PartialView(model).AsJavaScriptDocumentDotWrite();
		}
	}

	public class PartialModel
	{
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public Uri Website { get; set; }
	}

	public static class PartialViewResultExtensions
	{
		public static JavascriptResult AsJavaScriptDocumentDotWrite(this PartialViewResult partial)
		{
			return new JavascriptResult(partial);
		}
	}
}