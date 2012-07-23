using System;
using System.Net;
using Common.Logging;
using Quartz;

namespace imperugo.linkflood.scheduler
{
	public class WpCronJob : IJob
	{
		private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

		#region Implementation of IJob

		/// <summary>
		/// Called by the <see cref="T:Quartz.IScheduler"/> when a <see cref="T:Quartz.ITrigger"/>
		///             fires that is associated with the <see cref="T:Quartz.IJob"/>.
		/// </summary>
		/// <remarks>
		/// The implementation may wish to set a  result object on the 
		///             JobExecutionContext before this method exits.  The result itself
		///             is meaningless to Quartz, but may be informative to 
		///             <see cref="T:Quartz.IJobListener"/>s or 
		///             <see cref="T:Quartz.ITriggerListener"/>s that are watching the job's 
		///             execution.
		/// </remarks>
		/// <param name="context">The execution context.</param>
		public void Execute(IJobExecutionContext context)
		{
			try
			{
				WebRequest imperugoRequest = WebRequest.Create("http://tostring.it/wp-cron.php?doing_wp_cron&af7605df42d49bc7a5db9d695f8ac87f");
				var imperugoResponse = (HttpWebResponse) imperugoRequest.GetResponse();

				WebRequest qmatteoqRequest = WebRequest.Create("http://wp.qmatteoq.com/wp-cron.php?doing_wp_cron&792a7768a5034724f48229f7e652b48c");
				var qmatteoqResponse = (HttpWebResponse)qmatteoqRequest.GetResponse();
			}
			catch (Exception e)
			{
				Logger.Error(e);
			}
		}

		#endregion
	}
}