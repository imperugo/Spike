namespace imperugo.linkflood.scheduler.Entities{
	public class Category{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Object"/> class.
		/// </summary>
		public Category(string name, string[] matches){
			Name = name;
			Matches = matches;
		}

		public string Name { get; set; }
		public string[] Matches { get; set; }
	}
}