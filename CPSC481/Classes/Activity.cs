using System;
using System.Collections.Generic;
namespace CPSC481.Classes
{
	public class Item
	{
		public int id { get; set; }
		public string name;
		public string city;
		public string description;
		public string category;
		public string address;
		public string longitude;
		public string latitude;
		public double price;
		public double rating;
		private double fullRating;
		public List<ItemReview> reviews = new List<ItemReview>();
		public string[] images;
		public int minAge;
		public int maxAge;
		public int people;


		public Item(int id, string name, string city, string description, string category, string address, string longitude, string latitude, double price, string[] images, int minAge, int maxAge, int people)
		{
			this.id = id;
			this.name = name;
			this.city = city;
			this.description = description;
			this.category = category;
			this.address = address;
			this.longitude = longitude;
			this.latitude = latitude;
			this.price = price;
			this.images = images;
			this.rating = 0;
			this.fullRating = 0;
			this.minAge = minAge;
			this.maxAge = maxAge;
			this.people = people;
		}

		public void addReview(string name, double rating, string comment)
		{
			reviews.Add(new ItemReview(reviews.Count, name, rating, comment));
			this.fullRating += rating;
			this.rating = this.fullRating / this.reviews.Count;
		}
	}
}