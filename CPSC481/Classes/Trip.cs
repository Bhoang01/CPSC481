public class Trip
{
	public int id;
	private string name;
	public DateTime startDate;
	public DateTime endDate;
	private string city;
	private TripItinerary itinerary = new TripItinerary();

	public Trip(int id, string name, DateTime startDate, DateTime endDate, string city)
	{
		this.id = id;
		this.name = name;
		this.startDate = startDate;
		this.endDate = endDate;
		this.city = city;
	}
	public string getName()
	{
		return name;
	}
	public string getCity()
	{
		return city;
	}

	public string getFormatedStartDate()
	{
		return startDate.ToString("MM-dd-yyyy");
	}
	public string getFormatedEndDate()
	{
		return endDate.ToString("MM-dd-yyyy");
	}

	public bool addToItinerary(Item activity, DateTime startDate, string startTime, DateTime endDate, string endTime)
	{
		if (startDate.CompareTo(this.startDate) == -1 || endDate.CompareTo(this.endDate) == 1) return false;
		if (startDate.CompareTo(this.endDate) == 1 || endDate.CompareTo(this.startDate) == -1) return false;
		return itinerary.addActivity(activity, startDate, startTime, endDate, endTime);
	}

	public bool removeFromItinerary(Item activity)
	{
		return itinerary.removeActivity(activity);
	}

	public ItemFormat[] getItinerary()
	{
		return itinerary.getActivities();
	}
}
