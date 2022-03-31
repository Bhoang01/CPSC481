using System.Text.Json;
namespace CPSC481.Classes
{
	public class StateContainer
	{
		private List<User> users = new List<User>();
		private User? currentUser = null;
		private List<Item> activities = new List<Item>();
		public string? searchURL;
		public bool loaded = false;


		public bool isLoggedIn() => currentUser != null;
		public string? getFirstName() => currentUser?.firstName;
		public string? signup(string firstName, string lastName, string email, string password)
		{
			if (currentUser != null) return "You are already signed in";
			if (users.Any(u => u.email == email)) return "Email already exists";
			User user = new User(firstName, lastName, email, password);
			users.Add(user);
			currentUser = user;
			NotifyStateChanged();
			return null;
		}
		public string? login(string email, string password)
		{
			if (currentUser != null) return "You are already signed in";
			User? user = users.FirstOrDefault(u => u.email == email && u.isPasswordValid(password));
			if (user == null) return "Invalid email or password";
			currentUser = user;
			NotifyStateChanged();
			return null;
		}
		public bool logout()
		{
			if (currentUser == null) return false;
			currentUser = null;
			NotifyStateChanged();
			return true;
		}
		public Item[] getActivities(string city, int? minPrice, int? maxPrice, int? people, double? rating, int? minAge, int? maxAge, string? category, bool isAdult, bool isChild)
		{
			searchURL = $"/search?location={city}&minPrice={minPrice}&maxPrice={maxPrice}&people={people}&rating={rating}&minAge={minAge}&maxAge={maxAge}&isAdult={isAdult}&isChild={isChild}&category={category}";
			List<Item> filteredActivities = new List<Item>();
			foreach (Item activity in activities)
			{
				if (activity.city.ToLower() != city.ToLower()) continue;
				if (activity.price < minPrice || activity.price > maxPrice) continue;
				if (activity.people < people) continue;
				if (activity.rating < rating) continue;
				if (activity.minAge > minAge || activity.maxAge > maxAge) continue;
				if (category != String.Empty && activity.category.ToLower() != category?.ToLower()) continue;
				if (isAdult && activity.minAge < 18) continue;
				if (isChild && activity.maxAge > 18) continue;
				filteredActivities.Add(activity);
			}
			return filteredActivities.ToArray();
		}
		public Item? getActivity(int id)
		{
			return activities.FirstOrDefault(a => a.id == id);
		}
		public string? getActivityName(int id)
		{
			Item? activity = getActivity(id);
			if (activity == null) return null;
			return activity.name;
		}
		public bool addTrip(string name, DateTime startDate, DateTime endDate, string city)
		{
			if (currentUser == null) return false;
			currentUser.addTrip(name, startDate, endDate, city);
			NotifyStateChanged();
			return true;
		}

		public string[]? getTripNames()
		{
			if (currentUser == null) return null;
			return currentUser.getTripNames();
		}

		public bool setActiveTrip(string? name)
		{
			if (currentUser == null || name == null) return false;
			NotifyStateChanged();
			return currentUser.setActiveTrip(name);
		}

		public bool isActiveTrip()
		{
			if (currentUser == null) return false;
			return currentUser.isActiveTrip();
		}
		public bool isActiveTrip(int id)
		{
			if (currentUser == null) return false;
			return currentUser.isActiveTrip(id);
		}

		public Trip? getActiveTrip()
		{
			if (currentUser == null) return null;
			return currentUser.getActiveTrip();
		}

		public Trip? getTripDetails(int id)
		{
			if (currentUser == null) return null;
			return currentUser.getTripDetails(id);
		}

		public Trip[]? getTrips()
		{
			if (currentUser == null) return null;
			return currentUser.getTrips();
		}

		public event Action? OnChange;
		private void NotifyStateChanged() => OnChange?.Invoke();
		public StateContainer()
		{
			User user = new User("Zeyad", "Omran", "zeyad@bookie.com", "1234");
			user.addTrip("London 2020", new DateTime(2020, 1, 1), new DateTime(2020, 1, 12), "London");
			user.addTrip("Tokyo 2021", new DateTime(2021, 1, 1), new DateTime(2021, 1, 12), "Tokyo");
			users.Add(user);
			users.Add(new User("Briana", "Hoang", "briana@bookie.com", "1234"));
			users.Add(new User("Youstina", "Attia", "youstina@bookie.com", "1234"));
			users.Add(new User("Vi", "Tsang", "vi@bookie.com", "1234"));
			users.Add(new User("Yanessa", "Lacsamana", "yanessa@bookie.com", "1234"));

			string[] images = {
			"https://s3-media0.fl.yelpcdn.com/bphoto/oI84H-SWvD_XbblbZsgCJg/348s.jpg",
			"https://s3-media0.fl.yelpcdn.com/bphoto/ZaMPU6i9BL_DeRn9tS8OSg/348s.jpg"
		};
			Item activity = new Item(
				0,
				"iQ Food Co.",
				"Toronto",
				"We’re a collection of dreamers that believe food has the power to transcend and fuel the dreams of those around us. The dreams of our team (through providing opportunity and helping develop life skills); the dreams of our guests (by serving nutrient dense food that gives them the energy to pursue their ambitions).",
				"Food",
				"181 Bay Street Brookfield Place, Toronto, ON M5J 2T3",
				"-79.380660",
				"43.662600",
				20.0,
				images,
				0,
				99,
				99
			);
			activity.addReview("Zeyad", 5, "This place is great!");
			activity.addReview("John", 4.2, "Amazing Service!");
			activity.addReview("Peter", 4.8, "Great Food!");
			activities.Add(activity);

			images = new string[]{
			"https://www.cntower.ca/sites/default/files/styles/16_9_scale_and_crop_medium/public/images/tickets_0.jpg?h=5ce0254a&itok=fpvf3trs",
			"https://ak.jogurucdn.com/media/image/p25/place-2018-02-22-2-0ff3d25a192786af95fe0bb5ac479453.jpg"
		};
			activity = new Item(
			 1,
			 "CN Tower",
			 "Toronto",
			 "The CN Tower is a 553.3 m-high concrete communications and observation tower located in the downtown core of Toronto, Ontario, Canada. Built on the former Railway Lands, it was completed in 1976. Its name \"CN\" originally referred to Canadian National, the railway company that built the tower.",
			 "Landmark",
			 "290 Bremner Blvd, Toronto, ON M5V 3L9",
			 "-79.3892455",
			 "43.6425662",
			 29.0,
			 images,
				0,
				99,
				99
			);
			activity.addReview("Zeyad", 5, "Beautiful!");
			activity.addReview("John", 4.2, "Great for family!");
			activity.addReview("Peter", 4.8, "So much fun!");
			activities.Add(activity);
		}
	}
}