using System;

namespace imperugo.linkflood.scheduler.Entities {
	[Serializable]
	public class Tweet {
		public DateTime CreationDate { get; set; }
		public long Id { get; set; }
		public string Text { get; set; }
	}
}