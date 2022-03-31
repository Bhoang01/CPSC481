public class Trip
{
	public int id;
	private string name;
	private DateTime startDate;
	private DateTime endDate;
	private string city;

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
}
