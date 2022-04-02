public class User
{
	public string firstName;
	public string lastName;
	public string email;
	private string password;
	private List<Trip> trips = new List<Trip>();
	private Trip? activeTrip = null;
	public List<Item> favorites = new List<Item>();
	public User(string firstName, string lastName, string email, string password)
	{
		this.firstName = firstName;
		this.lastName = lastName;
		this.email = email;
		this.password = password;
	}
	public bool isPasswordValid(string password) => this.password == password;
	public bool isActiveTrip() => activeTrip != null ? true : false;
	public bool isActiveTrip(int id) => activeTrip != null ? activeTrip.id == id : false;
	public string[] getTripNames() => trips.Select(t => t.getName()).ToArray();
	public Trip[] getTrips() => trips.ToArray();

	public void addTrip(string name, DateTime startDate, DateTime endDate, string city)
	{
		Trip t = new Trip(trips.Count, name, startDate, endDate, city);
		trips.Add(t);
		activeTrip = t;
	}
	public bool setActiveTrip(string name)
	{
		Trip? trip = trips.FirstOrDefault(t => t.getName() == name);
		if (trip == null) return false;
		activeTrip = trip;
		return true;
	}

	public Trip? getActiveTrip() => activeTrip;

	public Trip? getTripDetails(int id)
	{
		Trip? trip = trips.FirstOrDefault(t => t.id == id);
		if (trip == null) return null;
		return trip;
	}

	public bool addToItinerary(Item activity, DateTime startDate, string startTime, DateTime endDate, string endTime)
	{
		if (activeTrip == null) return false;
		foreach (Trip trip in trips)
		{
			if (trip.id == activeTrip.id)
			{
				bool res = trip.addToItinerary(activity, startDate, startTime, endDate, endTime);
				activeTrip = trip;
				return res;
			}
		}
		return false;
	}

	public bool removeFromItinerary(Item activity)
	{
		if (activeTrip == null) return false;
		foreach (Trip trip in trips)
		{
			if (trip.id == activeTrip.id)
			{
				bool res = trip.removeFromItinerary(activity);
				activeTrip = trip;
				return res;
			}
		}
		return false;
	}

	public ItemFormat[]? getItinerary(int id)
	{
		Trip? trip = trips.FirstOrDefault(t => t.id == id);
		if (trip == null) return null;
		return trip.getItinerary();
	}

	public bool addToFavorites(Item activity)
	{
		favorites.Add(activity);
		return true;
	}

	public bool removeFromFavorites(int id)
	{
		Item? activity = favorites.FirstOrDefault(a => a.id == id);
		if (activity == null) return false;
		favorites.Remove(activity);
		return true;
	}
	public Item[] getFavorites() => favorites.ToArray();
	public bool isFavorited(int id) => favorites.Any(a => a.id == id);
}