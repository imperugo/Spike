using System;
using System.Collections.Generic;
using imperugo.linkflood.scheduler.Entities;

namespace imperugo.linkflood.scheduler.Data{
	public interface IStorage{
		void Save(Tweet item);
		void Save(IList<Tweet> items);
		void Delete(Tweet item);
		IList<Tweet> GetTweets(DateTime date);
		long? GetLastTweetId();
	}
}