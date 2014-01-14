namespace imperugo.JavascriptResult.Controllers
{
	using System;
	using System.IO;
	using System.Text;
	using System.Web;
	using System.Web.Mvc;

	public class JavascriptResult : PartialViewResult
	{
		public JavascriptResult(PartialViewResult innerPartial)
		{
			this.InnerPartialViewResult = innerPartial;
		}

		protected PartialViewResult InnerPartialViewResult { get; set; }

		public override void ExecuteResult(ControllerContext context)
		{
			TextWriter old = context.HttpContext.Response.Output;
			StringBuilder sb = new StringBuilder();

			using (var tw = new StringWriter(sb))
			{
				context.HttpContext.Response.Output = tw;
				this.InnerPartialViewResult.ExecuteResult(context);
				context.HttpContext.Response.Output = old;
			}

			context.HttpContext.Response.ContentType = "text/javascript";
			context.HttpContext.Response.Write(String.Format(@"document.write('{0}');", HttpUtility.JavaScriptStringEncode(sb.ToString())));
		}
	}
}