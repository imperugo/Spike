//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Raven.Client;
//using imperugo.linkflood.scheduler.Entities;

//namespace imperugo.linkflood.scheduler.Data
//{
//	public class RavenStorage : IStorage, IDisposable
//	{
//		public IDocumentSession RavenSession { get; protected set; }

//		public RavenStorage()
//		{
//			RavenSession = MvcApplication.Store.OpenSession();
//		}

//		#region Implementation of IStorage

//		public void Save(Tweet item)
//		{
			

//			throw new NotImplementedException();
//		}

//		public void Save(IList<Tweet> items)
//		{
//			throw new NotImplementedException();
//		}

//		public void Delete(Tweet item)
//		{
//			throw new NotImplementedException();
//		}

//		public IList<Tweet> GetTweets(DateTime date)
//		{
//			throw new NotImplementedException();
//		}

//		public long? GetLastTweetId()
//		{
//			throw new NotImplementedException();
//		}

//		#endregion

//		#region Implementation of IDisposable

//		/// <summary>
//		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
//		/// </summary>
//		/// <filterpriority>2</filterpriority>
//		public void Dispose()
//		{
//			Dispose(false);
//		}

//		public void Dispose(bool rollback)
//		{
//			using (RavenSession)
//			{
//				if (rollback)
//					return;

//				if (RavenSession != null)
//					RavenSession.SaveChanges();
//			}
//		}

//		#endregion
//	}
//}
