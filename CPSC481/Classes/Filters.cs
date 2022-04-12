public class Filters
{
	public string city;
	public string activityName;
	public int minPrice;
	public int maxPrice;
	public int people;
	public int rating;
	public int minAge;
	public int maxAge;
	public string category;
	public bool isAdult;
	public bool isChild;
	public Filters()
	{
		this.city = "";
		this.activityName = "";
		// init the rest of the variables
		this.minPrice = 0;
		this.maxPrice = 100000;
		this.people = 1;
		this.rating = 1;
		this.minAge = 0;
		this.maxAge = 100;
		this.category = "";
		this.isAdult = false;
		this.isChild = false;
	}

	public void update(string city, int minPrice, int maxPrice, int people, int rating, int minAge, int maxAge, string category, bool isAdult, bool isChild)
	{
		this.city = city;
		this.minPrice = minPrice;
		this.maxPrice = maxPrice;
		this.people = people;
		this.rating = rating;
		this.minAge = minAge;
		this.maxAge = maxAge;
		this.category = category;
		this.isAdult = isAdult;
		this.isChild = isChild;
	}

	public void updateActivityName(string activityName)
	{
		this.activityName = activityName;
	}
	public string getUrl()
	{
		return $"location={this.city}&minPrice={this.minPrice}&maxPrice={this.maxPrice}&people={this.people}&rating={this.rating}&minAge={this.minAge}&maxAge={this.maxAge}&isAdult={this.isAdult}&isChild={this.isChild}&activityName={this.activityName}&category={this.category}";
	}
}