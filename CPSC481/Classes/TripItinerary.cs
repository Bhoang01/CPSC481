public class TripItinerary
{
	private List<Item> activities = new List<Item>();
	private List<DateTime> startDates = new List<DateTime>();
	private List<string> startTimes = new List<string>();
	private List<DateTime> endDates = new List<DateTime>();
	private List<string> endTimes = new List<string>();

	public TripItinerary() { }

	public bool addActivity(Item activity, DateTime startDate, string startTime, DateTime endDate, string endTime)
	{
		if (startDate.CompareTo(endDate) == 1) return false;
		if (endDate.CompareTo(startDate) == -1) return false;
		for (int i = 0; i < activities.Count; i++)
		{
			Item a = activities[i];
			if (a == activity && startDates[i].ToString("MM dd yyyy").CompareTo(startDate.ToString("MM dd yyyy")) == 0) return false;
		}
		activities.Add(activity);
		startDates.Add(startDate);
		startTimes.Add(startTime);
		endDates.Add(endDate);
		endTimes.Add(endTime);
		return true;
	}
	public bool removeActivity(Item activity)
	{
		int index = activities.IndexOf(activity);
		if (index == -1) return false;
		activities.RemoveAt(index);
		startDates.RemoveAt(index);
		endDates.RemoveAt(index);
		return true;
	}

	public ItemFormat[] getActivities()
	{
		ItemFormat[] activities = new ItemFormat[this.activities.Count];
		for (int i = 0; i < this.activities.Count; i++)
		{
			activities[i] = new ItemFormat(this.activities[i], this.startDates[i], this.startTimes[i], this.endDates[i], this.endTimes[i]);
		}
		return activities;
	}
}

public class ItemFormat
{
	public Item activity;
	public DateTime startDate;
	public string startTime;
	public DateTime endDate;
	public string endTime;
	public ItemFormat(Item activity, DateTime startDate, string startTime, DateTime endDate, string endTime)
	{
		this.activity = activity;
		this.startDate = startDate;
		this.startTime = startTime;
		this.endDate = endDate;
		this.endTime = endTime;
	}
}