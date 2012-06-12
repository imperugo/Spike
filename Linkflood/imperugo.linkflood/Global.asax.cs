using System;
using System.IO;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Quartz;
using Quartz.Impl;
using imperugo.linkflood.scheduler;
using log4net.Config;

namespace imperugo.linkflood
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);

			BundleTable.Bundles.RegisterTemplateBundles();

			XmlConfigurator.ConfigureAndWatch(GetConfigFile("log4net.config"));

			StartScheduler();
		}

		void StartScheduler(){
			ISchedulerFactory schedFact = new StdSchedulerFactory();

			IScheduler sched = schedFact.GetScheduler();
			sched.Start();

			IJobDetail grabberJob = JobBuilder.Create<LinkfloodGrabjob>()
				.WithIdentity("grabberJob", "linkflood")
				.Build();

			IJobDetail posterJob = JobBuilder.Create<LinkfloodPostJob>()
				.WithIdentity("posterJob", "linkflood")
				.Build();

			ITrigger halfHour = TriggerBuilder.Create()
				.WithIdentity("30Min", "group1")
				.WithCronSchedule("0 0/30 * 1/1 * ? *")
				.Build();

			ITrigger oneMinute = TriggerBuilder.Create()
			.WithIdentity("1Min", "group1")
				.WithCronSchedule("0 0/1 * 1/1 * ? *")
			.Build();

			ITrigger everyDay = TriggerBuilder.Create()
			.WithIdentity("everyDay", "group1")
				.WithCronSchedule("0 0 11 1/1 * ? *")
			.Build();


			sched.ScheduleJob(grabberJob, halfHour);
			sched.ScheduleJob(posterJob, everyDay);

			sched.Start();
		}

		static FileInfo GetConfigFile(string fileName)
		{
			if (Path.IsPathRooted(fileName))
			{
				return new FileInfo(fileName);
			}
			FileInfo r = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName));

			return r;
		}
	}
}