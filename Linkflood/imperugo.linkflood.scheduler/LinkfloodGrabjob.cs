using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using Common.Logging;
using Newtonsoft.Json;
using Quartz;
using imperugo.linkflood.scheduler.Data;
using imperugo.linkflood.scheduler.Entities;

namespace imperugo.linkflood.scheduler
{
	public class LinkfloodGrabjob : IJob
	{
		private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

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
		public void Execute(IJobExecutionContext context){
			try{
				Logger.Info("test");

				string twitterUrl = string.Format("https://api.twitter.com/1/statuses/user_timeline.json?include_entities=false&include_rts=false&screen_name={0}&count=200&exclude_replies=true", LinkfloodConfiguration.TwitterUsername);

				var response = this.DownloadWebPage(new Uri(twitterUrl));

				dynamic j = JsonConvert.DeserializeObject<dynamic>(response);

				List<Tweet> tweetsToStore = new List<Tweet>();
				long lastTweetId;

				foreach (var tweet in j)
				{
					string msg = tweet.text;

					foreach (var category in LinkfloodConfiguration.Categories){
						foreach (var match in category.Matches){
							if(msg.Contains(match)){
								Tweet t = ConvertTweet(tweet);

								//if (t.CreationDate.ToUniversalTime() < DateTime.UtcNow)
								//	continue;

								tweetsToStore.Add(t);
							}
						}
					}
				}

				//if (tweetsToStore.Any())
				//	lastTweetId = tweetsToStore.First().Id;

				IStorage storage = new FileSystemStorage();
				storage.Save(tweetsToStore);
			}
			catch (Exception e){
				Logger.Error("[Scheduler] Unable to grab the job", e);
			}
		}

		string DownloadWebPage(Uri url)
		{
			var request = (HttpWebRequest)WebRequest.Create(url);

			request.Headers["Accept-Encoding"] = "gzip";
			request.Headers["Accept-Language"] = "en-us";
			request.UserAgent = "tostring.it - ";
			request.Credentials = CredentialCache.DefaultNetworkCredentials;
			request.AutomaticDecompression = DecompressionMethods.GZip;

			using (WebResponse response = request.GetResponse())
			{
				using (var reader = new StreamReader(response.GetResponseStream()))
				{
					return reader.ReadToEnd();
				}
			}
		}

		Tweet ConvertTweet(dynamic tweet)
		{
			Tweet t = new Tweet();
			t.Id = tweet.id;
			t.Text = tweet.text;
			t.CreationDate = DateTime.ParseExact((string)tweet.created_at, "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture);

			return t;
		}
	}
}
