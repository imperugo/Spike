using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using imperugo.linkflood.scheduler.Entities;

namespace imperugo.linkflood.scheduler.Data{
	public class FileSystemStorage : IStorage{
		#region IStorage Members

		public void Save(Tweet item){
			IList<Tweet> existingItems = GetTweets(item.CreationDate.Date);

			if(existingItems.Any(x => x.Id == item.Id))
				return;

			SaveTweets(DateTime.UtcNow.Date, existingItems);
		}

		public void Save(IList<Tweet> items){

			IList<Tuple<IList<Tweet>,DateTime>> toBeSaved = new List<Tuple<IList<Tweet>, DateTime>>();

			foreach (var tweet in items){
				IList<Tweet> ls = toBeSaved.Where(x => x.Item2 == tweet.CreationDate.Date).Select(x => x.Item1).SingleOrDefault();

				if (ls == null)
				{
					ls = GetTweets(DateTime.UtcNow.Date);
					toBeSaved.Add(new Tuple<IList<Tweet>, DateTime>(ls, DateTime.UtcNow.Date));
				}
				
				if(!ls.Any(x => x.Id == tweet.Id))
					ls.Add(tweet);
			}

			foreach (var tuple in toBeSaved){
				SaveTweets(tuple.Item2, tuple.Item1);
			}
		}

		public void Delete(Tweet item){
			throw new NotImplementedException();
		}

		public IList<Tweet> GetTweets(DateTime date){
			string filePath = GetFilePath(date);

			if (!File.Exists(filePath))
				return new List<Tweet>();

			string serializedData;

			using (var reader = new StreamReader(filePath)){
				serializedData = reader.ReadToEnd();
			}

			return serializedData.Deserialize<List<Tweet>>();
		}

		
		public long? GetLastTweetId(){
			throw new NotImplementedException();
		}

		#endregion

		static string GetFilePath(DateTime date)
		{
			return string.Format("{0}App_Data{1}\\Tweets\\{2}.txt", AppDomain.CurrentDomain.BaseDirectory, LinkfloodConfiguration.StoragePath, date.Date.ToString("dd-MM-yyyy"));
		}


		void SaveTweets(DateTime date, IList<Tweet> items){
			string filePath = GetFilePath(date);

			if (File.Exists(filePath))
				File.Delete(filePath);

			string serialized = items.Serialize();

			using (StreamWriter outfile = new StreamWriter(filePath))
			{
				outfile.Write(serialized);
			}
		}
	}
}