namespace CPSC481.Classes
{
	public class ItemReview
	{
		public int id;
		public string name;
		public double rating;
		public string comment;
		public ItemReview(int id, string name, double rating, string comment)
		{
			this.id = id;
			this.name = name;
			this.rating = rating;
			this.comment = comment;
		}
	}
}