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
		public Dictionary<int, Trip> sharedItineraries = new Dictionary<int, Trip>();

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

		public bool addToItinerary(int id, DateTime startDate, string startTime, DateTime endDate, string endTime)
		{
			if (currentUser == null) return false;
			Item? activity = activities.FirstOrDefault(a => a.id == id);
			if (activity == null) return false;
			bool res = currentUser.addToItinerary(activity, startDate, startTime, endDate, endTime);
			NotifyStateChanged();
			return res;
		}
		public bool removeFromItinerary(int id)
		{
			if (currentUser == null) return false;
			Item? activity = activities.FirstOrDefault(a => a.id == id);
			if (activity == null) return false;
			bool res = currentUser.removeFromItinerary(activity);
			NotifyStateChanged();
			return res;
		}
		public ItemFormat[]? getItinerary(int id)
		{
			if (currentUser == null) return null;
			return currentUser.getItinerary(id);
		}
		public bool addToFavorites(int id)
		{
			if (currentUser == null) return false;
			Item? activity = activities.FirstOrDefault(a => a.id == id);
			if (activity == null) return false;
			bool res = currentUser.addToFavorites(activity);
			NotifyStateChanged();
			return res;
		}
		public bool removeFromFavorites(int id)
		{
			if (currentUser == null) return false;
			bool res = currentUser.removeFromFavorites(id);
			NotifyStateChanged();
			return res;
		}

		public void shareItinerary(int id)
		{
			if (currentUser == null) return;
			sharedItineraries.Add(id, currentUser.getTripDetails(id)!);
			NotifyStateChanged();
		}
		public Trip? getSharedItinerary(int id)
		{
			if (sharedItineraries.ContainsKey(id))
				return sharedItineraries[id];
			return null;
		}

		public Item[]? getFavorites()
		{
			if (currentUser == null) return null;
			return currentUser.getFavorites();
		}
		public bool isFavorited(int id)
		{
			if (currentUser == null) return false;
			return currentUser.isFavorited(id);
		}
		public event Action? OnChange;
		private void NotifyStateChanged() => OnChange?.Invoke();
		public StateContainer()
		{
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
				-79.380660,
				43.662600,
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
			 -79.3892455,
			 43.6425662,
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

			images = new string[]{
			"https://viewthevibe.com/wp-content/uploads/2021/06/EFj1mmoW4AAu9xN.jpg",
			"https://media.blogto.com/articles/202167-centreville-amusement-park-1.jpg?w=2048&cmd=resize_then_crop&height=1365&quality=70"
			};
			activity = new Item(
			 2,
			 "Centreville Amusement Park",
			 "Toronto",
			 "With more than 30 rides and attractions and 14 mouth-watering food outlets, Centre Island’s iconic Centreville Amusement Park is the ultimate summer destination for families with young children!",
			 "Popular",
			 "9 Queens Quay W, Toronto, ON M5J 2H3",
			 43.6233056,
			 -79.3747454,
			 38.0,
			 images,
				0,
				99,
				99
			);
			activity.addReview("Sarah", 5, "The kids and I had a great time!");
			activity.addReview("John", 4.6, "Had lots of fun!");
			activity.addReview("Zeyad", 4.8, "Could spend hours here!");
			activities.Add(activity);

			images = new string[]{
			"https://images.squarespace-cdn.com/content/v1/57058af18a65e2f19709cb0d/1599877887207-BT99LDI3HPBU4T95DFUC/120223_SC_0606.jpg",
			"https://scaramoucherestaurant.agencydominion.net/uploads/2021/11/scaramouche-pasta-bar-grill-toronto-restaurant-01-1440x900.jpg"
			};
			activity = new Item(
			 3,
			 "Scaramouche Restaurant",
			 "Toronto",
			 "Scaramouche has long been celebrated by customers and critics for its unwavering commitment to making each dining experience a memorable one. Chef/owner Keith Froggett's sophisticated cuisine is presented simply with care and attention to detail. The service underscores that philosophy: unobtrusive and respectful. The restaurant affords a stunning nighttime vista of the downtown Toronto skyline.",
			 "Food",
			 "1 Benvenuto Pl, Toronto, ON M4V 2L1",
			 43.68163,
			 -79.40027,
			 45.0,
			 images,
				0,
				99,
				99
			);
			activity.addReview("Sarah", 5, "Very good food and environment");
			activity.addReview("Zeyad", 4.6, "Had a nice view of the city!");
			activity.addReview("Luke", 3.8, "Way too overpriced");
			activities.Add(activity);

			images = new string[]{
			"https://michaeleatsdotcom.files.wordpress.com/2019/06/edm1.jpg",
			"https://tastetoronto-files.s3.ca-central-1.amazonaws.com/variants/7za8ufL4K7YnivkrdZxdgief/6c96dc93855d4637148356732cfe63a0758c76f13b08ef6b334565242f75f666?response-content-disposition=inline%3B%20filename%3D%22ed-s-real-scoop.jpg%22%3B%20filename%2A%3DUTF-8%27%27ed-s-real-scoop.jpg&response-content-type=image%2Fjpeg&X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=AKIA3QGUCREVKDDYFP6A%2F20220409%2Fca-central-1%2Fs3%2Faws4_request&X-Amz-Date=20220409T013452Z&X-Amz-Expires=300&X-Amz-SignedHeaders=host&X-Amz-Signature=22524281b8a85a3149ee4d8780f69d330bcb90f12afd85fa5718299f6c29e1f9"
			};
			activity = new Item(
			 4,
			 "Ed's Real Scoop",
			 "Toronto",
			 "A local favourite, Ed's Real Scoop provides a huge choice of handmade ice cream flavours, plus ice cream cakes & jars of artisanal sundae sauce.",
			 "Food",
			 "920 Queen St E, Toronto, ON M4M 1J5",
			 43.66094,
			 -79.34196,
			 6.0,
			 images,
				0,
				99,
				99
			);
			activity.addReview("Sarah", 4.6, "Fast service and good on a hot day");
			activity.addReview("James", 5, "Try their burnt marshmallow flavour!");
			activity.addReview("Luke", 4.3, "The gelato is really good");
			activities.Add(activity);

			images = new string[]{
			"data:image/jpeg;base64,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"
			};
			activity = new Item(
			 5,
			 "CF Toronto Eaton Centre",
			 "Toronto",
			 "Built in 1977, this downtown shopping complex continues to be Toronto's top attraction and North America's busiest mall, offering more than 250 stores and restaurants.",
			 "Popular",
			 "220 Yonge St, Toronto, ON M5B 2H1",
			 43.65460,
			 -79.38050,
			 50.0,
			 images,
				0,
				99,
				99
			);
			activity.addReview("Sarah", 4.6, "Very pretty mall");
			activity.addReview("Ruth", 2.6, "Too crowded and overhyped");
			activity.addReview("Luke", 4.3, "Love all the shops here!");
			activities.Add(activity);

			images = new string[]{
			"https://torontoist.com/wp-content/uploads/2013/09/20130926torontozoo.jpg",
			"https://www.newyorkbyrail.com/wp-content/uploads/2017/07/toronto-zoo.jpg"
			};
			activity = new Item(
			 6,
			 "Toronto Zoo",
			 "Toronto",
			 "Your Toronto Zoo is committed to connecting people, animals and conservation to fight extinction and we are excited to be welcoming back you and your families. Take a drive on the WILD side and experience our Zoo comfortably from your own vehicle with our Scenic Safari Drive-Thru or enjoy our new one-way walking routes. To further enrich your visit, why not add on a Wild Encounter to get up close and personal with some of our animals! .",
			 "Popular",
			 "2000 Meadowvale Rd, Toronto, ON M1B 5K7",
			 43.82076,
			 -79.18154,
			 23.0,
			 images,
				0,
				99,
				99
			);
			activity.addReview("Tim", 4.6, "The kids loved it, there was so much to see");
			activity.addReview("Ruth", 4.2, "Lots of animals to see but the zoo layout was confusing");
			activity.addReview("Luke", 3.7, "Be prepared to walk a lot and remember to book tickets in advance");
			activities.Add(activity);

			images = new string[]{
			"https://www.thegate.ca/wp-content/uploads/2018/12/Legoland.jpg",
			"https://media.tacdn.com/media/attractions-splice-spp-674x446/06/67/ae/90.jpg"
			};
			activity = new Item(
			 7,
			 "LEGOLAND Discovery Center",
			 "Toronto",
			 "LEGOLAND® Discovery Centre Toronto is an indoor, family experience with over 3 million bricks under one roof. Enjoy the Ultimate Indoor LEGO® Playground featuring 2 LEGO®-themed rides, a 4D cinema, Master Model Builder workshops, and more! Also check out Toronto’s iconic attractions made out of LEGO® in MINILAND. We are best for families with children between the ages of 3-10 years old.",
			 "Popular",
			 "1 Bass Pro Mills Dr, Vaughan, ON L4K 5W4",
			 43.82520,
			 -79.53497,
			 27.0,
			 images,
				0,
				99,
				99
			);
			activity.addReview("Tim", 3.4, "Fun for kids, but not for adults");
			activity.addReview("Sam", 2.0, "A lot of the Lego sets showed some aging");
			activity.addReview("Yanessa", 4.5, "The 4D movie was great and the staff were very kind");
			activities.Add(activity);

			images = new string[]{
			"https://nowtoronto.com/wp-content/uploads/2020/09/140815-2000-1-1100x618.jpg",
			"data:image/jpeg;base64,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",
			"https://media.tacdn.com/media/attractions-splice-spp-674x446/06/67/ae/90.jpg"
			};
			activity = new Item(
			 8,
			 "Casa Loma",
			 "Toronto",
			 "Canada's majestic castle and Toronto’s premier historic attraction. In 1911, Sir Henry Pellatt engaged noted architect E.J. Lennox to help him realize a lifelong dream,the creation of an Edwardian castle on the top of a hill overlooking Toronto. It took 300 men nearly three years to complete the nearly to 200,000 square foot castle at a cost $3,500,000 (at the time). Situated on 5 acres, Casa Loma was the largest private residence in Canada. ",
			 "Landmark",
			 "1 Bass Pro Mills Dr, Vaughan, ON L4K 5W4",
			 43.82520,
			 -79.53497,
			 27.0,
			 images,
				0,
				99,
				99
			);
			activity.addReview("Brianna", 4.6, "It's very beautiful and interesting!");
			activity.addReview("Wilbur", 4.0, "The castle is well-cared for with a good cafe at the end");
			activity.addReview("Gary", 4.5, "The audio tour was very interesting and clear");
			activities.Add(activity);

			images = new string[]{
			"https://www.cp24.com/polopoly_fs/1.5487718.1624828250!/httpImage/image.jpg_gen/derivatives/landscape_1020/image.jpg",
			"data:image/jpeg;base64,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"
			};
			activity = new Item(
			 9,
			 "Lake Ontario",
			 "Toronto",
			 "Almost like an ocean — that’s how enormous Lake Ontario seems. One of five North American Great Lakes, the 19,000-square-kilometre freshwater body is flanked on the southeast by New York State and Toronto, Ontario on the west. It has 1,145 kilometres of beautiful shoreline, including offshore islands. And as you might imagine, the shores are lined with tree-filled parks, sandy beaches, and restaurants with a view — all near urban Toronto.",
			 "Nature",
			 "Ontario",
			 43.5687211,
			 -79.19522,
			 0,
			 images,
				0,
				99,
				99
			);
			activity.addReview("Blake", 4.6, "Very beautiful body of water!");
			activity.addReview("Wilbur", 4.0, "Gives a wonderful view of the sunset");
			activity.addReview("Vi", 3.5, "The lake is huge with lots of water sports available to do around it, but the water is quite dirty");
			activities.Add(activity);

			images = new string[]{
			"https://ago.ca/sites/default/files/styles/super_card_image/public/2020-06/101838393_10158309896514144_1312539256878530560_n.png?itok=t3urikVi",
			"https://www.artnews.com/wp-content/uploads/2019/05/ago.jpg"
			};
			activity = new Item(
			 10,
			 "Art Gallery of Ontario",
			 "Toronto",
			 "With a collection of more than 90,000 works of art, the Art Gallery of Ontario (AGO) is among the largest and most distinguished art museums in North America. An international landmark, the AGO is also one of Canada’s most innovative cultural destinations. Highlights of the Gallery’s world-class collection include iconic Canadian and Inuit works, along with European and contemporary art – all on view in a spectacular building transformed by renowned Toronto-born architect Frank Gehry.",
			 "Popular",
			 "317 Dundas St W, Toronto, ON M5T 1G4",
			 43.653777, 
			 -79.392523,
			 25.0,
			 images,
				0,
				99,
				99
			);
			activity.addReview("Youstina", 4.6, "This was my first visit and I thought it was beautiful!");
			activity.addReview("Wilbur", 4.5, "The new Robert Houle exhibit was quite good");
			activity.addReview("Alexandra", 4.5, "Stunning gallery with lots of natural lighiting");
			activities.Add(activity);

			images = new string[]{
			"https://images.otstatic.com/prod/28088412/3/large.jpg",
			"https://noregretsjustlive.files.wordpress.com/2015/01/hot-house.jpg"
			};
			activity = new Item(
			 11,
			 "Hothouse",
			 "Toronto",
			 "Spacious standby for pizzas, pastas & assorted dishes, plus a Sunday buffet brunch with live jazz.",
			 "Food",
			 "35 Church St, Toronto, ON M5E 1T3",
			 43.64908,
			 -79.37375,
			 27.0,
			 images,
				0,
				99,
				99
			);
			activity.addReview("Nick", 4.0, "Lots of good brunch options here");
			activity.addReview("Phil", 3.0, "The food was just okay and the portions were too big");
			activity.addReview("Brianna", 4.2, "The staff were very polite and the food was good.");
			activities.Add(activity);

			images = new string[]{
			"https://i.guim.co.uk/img/media/fd30c6e90dcef2c2e6d5e65955423816eff62ad4/0_589_5587_3354/master/5587.jpg?width=1200&quality=85&auto=format&fit=max&s=d29d8ae1a9064946b9e0f2fd0555a92d",
			"data:image/jpeg;base64,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"
			};
			activity = new Item(
			 11,
			 "Shakespeare's Globe Theatre",
			 "London",
			 "Founded by the pioneering American actor and director Sam Wanamaker, Shakespeare's Globe is a unique international resource dedicated to the exploration of Shakespeare's work and the playhouse for which he wrote, through the connected means of performance and education.Together, the Globe Theatre Company, Shakespeare's Globe Exhibition and Globe Education seek to further the experience and international understanding of Shakespeare in performance.",
			 "Popular",
			 "Cardinal Cap Alley, London SE1 9JF, United Kingdom",
			 51.508198, 
			 -0.0971281,
			 29.0,
			 images,
				0,
				99,
				99
			);
			activity.addReview("Nick", 5.0, "I really liked watching Macbeth here");
			activity.addReview("Youstina", 4.0, "The guided tour was great - very informative and interesting");
			activity.addReview("Bella", 4.2, "The theatre is beautiful and it was a very unique experience.");
			activities.Add(activity);

			images = new string[]{
			"https://i1.sndcdn.com/artworks-2Qzzlytdxrkzk6x4-LVAQsA-t500x500.jpg",
			"https://pro-av.panasonic.net/en/casestudies/rsjc/img/fig.jpg"
			};
			activity = new Item(
			 12,
			 "Ronnie Scott's Jazz Club",
			 "London",
			 "European vanguard for jazz and blues from world's top musicians, in basement club with late bar.",
			 "Popular",
			 "47 Frith St, London W1D 4HT, United Kingdom",
			 51.51358,
			 -0.13156,
			 40.0,
			 images,
				0,
				99,
				99
			);
			activity.addReview("George", 4.5, "The food is good here, good staff, good music");
			activity.addReview("Yanessa", 5.0, "I had a very entertaining evening here and the band was outstanding");
			activity.addReview("Sam", 3.2, "It's pretty touristy here, too crowded and hot.");
			activities.Add(activity);

			images = new string[]{
			"https://media.tacdn.com/media/attractions-splice-spp-674x446/0b/ee/fb/5d.jpg",
			"https://static.independent.co.uk/2020/10/23/12/newFile-1.jpg?quality=75&width=982&height=726&auto=webp"
			};
			activity = new Item(
			 13,
			 "The Dubai Fountain",
			 "Dubai",
			 "Choreographed to music, the Dubai Fountain shoots water as high as 500 feet –that’s as high as a 50-story building. Designed by creators of the Fountains of Bellagio in Vegas, Dubai Fountain Performances occur daily on the 30-acre Burj Khalifa Lake.",
			 "Landmark",
			 "Sheikh Mohammed bin Rashid Blvd - Downtown Dubai - Dubai - United Arab Emirates",
			 25.195261, 
			 55.275161,
			 0.0,
			 images,
				0,
				99,
				99
			);
			activity.addReview("Youstina", 4.5, "A good view to have when my friends and I were eating dinner");
			activity.addReview("Roger", 3.0, "The show was great but only lasted two minutes");
			activity.addReview("Clara", 4.2, "Impressive fountain witha  good display.");
			activities.Add(activity);

			images = new string[]{
			"https://b.zmtcdn.com/data/pictures/8/201258/da3cf459c4cff1ab0fb19d6a05e3fd8d.jpg",
			"data:image/jpeg;base64,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"
			};
			activity = new Item(
			 14,
			 "Bussola",
			 "Dubai",
			 "Seaview Italian restaurant with pizzeria on top deck, romantic fine dining below, plus terrace bar.",			
			 "Food",
			 "The Westin Dubai Mina Seyahi Beach Resort & Marina - Dubai - United Arab Emirates",
			 25.09398,
			 55.14818,
			 40.0,
			 images,
				0,
				99,
				99
			);
			activity.addReview("Thompson", 4.5, "A good view to have when my friends and I were eating dinner");
			activity.addReview("Clay", 3.0, "The show was great but only lasted two minutes");
			activity.addReview("Vi", 4.2, "Impressive fountain witha  good display.");
			activities.Add(activity);

			User user = new User("Zeyad", "Omran", "zeyad@bookie.com", "1234");
			user.addTrip("London 2020", new DateTime(2020, 1, 1), new DateTime(2020, 1, 12), "London");
			user.addTrip("Tokyo 2021", new DateTime(2021, 1, 1), new DateTime(2021, 1, 12), "Tokyo");
			user.addToItinerary(activities[0], new DateTime(2021, 1, 2), "14:00", new DateTime(2021, 1, 2), "15:00");
			user.addToItinerary(activities[1], new DateTime(2021, 1, 5), "14:00", new DateTime(2021, 1, 5), "15:00");
			users.Add(user);
			users.Add(new User("Briana", "Hoang", "briana@bookie.com", "1234"));
			users.Add(new User("Youstina", "Attia", "youstina@bookie.com", "1234"));
			users.Add(new User("Vi", "Tsang", "vi@bookie.com", "1234"));
			users.Add(new User("Yanessa", "Lacsamana", "yanessa@bookie.com", "1234"));
		}
	}
}