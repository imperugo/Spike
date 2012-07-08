using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlexJamesBrown.JoeBlogs.Structs;
using Common.Logging;
using Quartz;
using imperugo.linkflood.scheduler.Data;
using imperugo.linkflood.scheduler.Entities;
using imperugo.linkflood.scheduler.Helpers;
using AlexJamesBrown.JoeBlogs;

namespace imperugo.linkflood.scheduler{
	public class LinkfloodPostJob : IJob
	{
		private static readonly ILog logger = LogManager.GetCurrentClassLogger();

		#region IJob Members

		/// <summary>
		/// 	Called by the <see cref="T:Quartz.IScheduler" /> when a <see cref="T:Quartz.ITrigger" /> fires that is associated with the <see
		///  	cref="T:Quartz.IJob" /> .
		/// </summary>
		/// <remarks>
		/// 	The implementation may wish to set a result object on the JobExecutionContext before this method exits. The result itself is meaningless to Quartz, but may be informative to <see
		///  	cref="T:Quartz.IJobListener" /> s or <see cref="T:Quartz.ITriggerListener" /> s that are watching the job's execution.
		/// </remarks>
		/// <param name="context"> The execution context. </param>
		public void Execute(IJobExecutionContext context){
			try{
				Dictionary<string, IList<Tweet>> groupedTweets = new Dictionary<string, IList<Tweet>>();

				IStorage storage = new FileSystemStorage();

				IList<Tweet> tweets = storage.GetTweets(DateTime.Today);

				foreach (Tweet tweet in tweets){
					string category = GetCategoryName(tweet);

					if (groupedTweets.ContainsKey(category)){
						IList<Tweet> tws = groupedTweets[category];
						tws.Add(tweet);

						groupedTweets.Remove(category);
						groupedTweets.Add(category, tws);
					}
					else{
						var tws = new List<Tweet>();
						tws.Add(tweet);
						groupedTweets.Add(category, tws);
					}
				}

				if(!tweets.Any())
					return;

				var sortedGroupTweet = groupedTweets.OrderBy(entry => entry.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

				string pageTitle = string.Format(LinkfloodConfiguration.TitleFormat, DateTime.Today.ToString("dd-MMM-yyyy"));

				StringBuilder body = new StringBuilder();

				foreach (var groupedTweet in sortedGroupTweet)
				{
					body.AppendFormat("<h2>{0} ({1})</h2><br />", groupedTweet.Key, groupedTweet.Value.Count);

					body.Append("<ul>");

					foreach (var tweet in groupedTweet.Value) {
						body.AppendFormat("<li>- <strong>{0}</strong></li>", ParseText(tweet.Text));
					}

					body.Append("</ul>");
				}

				Post(pageTitle, body.ToString());
			}
			catch (Exception e){
				logger.Error("[Scheduler] Error posting", e);
			}
		}

		#endregion

		string GetCategoryName(Tweet tweet){
			foreach (Entities.Category category in LinkfloodConfiguration.Categories){
				foreach (string match in category.Matches){
					if (tweet.Text.Contains(match)){
						return category.Name;
					}
				}
			}

			return "Misc";
		}

		string ParseText(string text)
		{
			text = ParserHelper.Linkify(text);
			text = ParserHelper.LinkifyHashtags(text);
			text = ParserHelper.LinkifyMentions(text);

			return text;
		}

		void Post(string title, string body){
			WordPressWrapper wp = new WordPressWrapper(LinkfloodConfiguration.Url, LinkfloodConfiguration.Username, LinkfloodConfiguration.Password);

			var content = new Post();
			content.title = title;
			content.description = body;
			content.categories = new[] { "Linkflood" };
			content.dateCreated = DateTime.UtcNow;
			wp.NewPost(content, true);

			logger.InfoFormat("[Scheduler] Post added '{0}'", title);
		}
	}
}