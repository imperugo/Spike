using System.Collections.Generic;
using System.Configuration;
using imperugo.linkflood.scheduler.Entities;

namespace imperugo.linkflood.scheduler{
	internal static class LinkfloodConfiguration{
		static readonly IList<Category> categories;

		/// <summary>
		/// 	Initializes a new instance of the <see cref="T:System.Object" /> class.
		/// </summary>
		static LinkfloodConfiguration(){
			categories = new List<Category>();

			categories.Add(new Category("Technical", new[] {"#tech"}));
			categories.Add(new Category("Dev", new[] {"#dev ", "@zite"}));
			categories.Add(new Category("UX & Design", new[] {"#ux", "#design"}));
		}

		public static string TwitterUsername{
			get { return "imperugo"; }
		}

		public static IList<Category> Categories{
			get { return categories; }
		}

		public static string StoragePath{
			get { return ""; }
		}

		public static string TitleFormat{
			get { return "Linkflood: {0}"; }
		}


		public static string SubPageSlugFormat
		{
			get { return "Linkflood-{0}"; }
		}

		public static string Url
		{
			get { return ConfigurationManager.AppSettings["wordpress.url"]; }
		}
		
		public static string Username{
			get { return ConfigurationManager.AppSettings["wordpress.username"]; }
		}

		public static string Password{
			get { return ConfigurationManager.AppSettings["wordpress.password"]; }
		}
	}
}