# CPSC481 Bookie App
Bookie is an app that helps people who are interested in traveling plan their trip. The  main goal is to create a convenient itinerary planner that shows current events at the destination and a convenient overview of a person’s plans and bookings for the duration of their trip.

## Team Members
Brianna Hoang <br>
Youstina Attia<br>
Vi Tsang<br>
Yanessa Lacsamana<br>
Zeyad Omran<br>


## Set up
1) Download and install [node.js](https://nodejs.org/en/) (We're using v16.14.0).
2) Download and install [dotnet 6.0](https://dotnet.microsoft.com/en-us/learn/aspnet/blazor-tutorial/install) (We're using v6.0.200).
3) cd into `CPSC481/CPSC481` (The sub dir not the root of the project).
4) run `npm install` in your terminal.
5) run `dotnet watch` in your terminal.


## System Walkthrough
### Walkthrough #1
**Step 1: Sign-up <br>**
Use the top right sign up button to create an account. Once all required data is submitted, you will be logged in and redirected to homescreen. . If any information is missing or incorrect you will be provided error messages.

**Step 2: Start Trip <br>**
Once on homescreen, click the "Start Your Trip" button on the top in order to set up an itinerary. Enter the following information:
- Trip Name: Toronto 2021
- City: Toronto
- From: Jan 1, 2021
- To: Jan 2, 2021
Once all data is entered, click "Start Trip". If any information is missing you will be provided error messages. Once submission is successful, you will be able to view all trips created in your account. If you click on your newly created trip, you will find the itinerary to be empty with a message to add activities.

**Step 3: Search Acitivites <br>**
You can now click on the Bookie logo on the top left to navigate to homescreen and begin your search. In the search bar, type "Toronto" and hit the search button. You will be directed to the activites screen with the city set to Toronto on the left panel.  

**Step 4: Filter & Select Acitivity <br>**
You can use the filter on the left pane to select the "popular" tag. Proceed to select "Centreville Amusement Park" by clicking the card to view infromation.

**Step 5: Add to trip <br>**
Once on activity page, you can view all the information required, including reviews, rating, price and address. You can add the trip to your trip. Click "Add to Trip" button and choose "Toronto 2021" trip. For date and time, enter Jan 1, 2021 in the date picker and update time value to 1:30 PM using arrows. Click "Next" and "Confirm". 

**Step 6: Search Acitivites <br>**
Once trip is added, hit the back bottom on the top left above the activity image to continue the search for activities with the last filter applied. 

**Step 7: Favourite <br>**
Now let's find a food location, hit the "food" category in the filters. You will find "Hothouse". Click the card and click "Favourite" to add later if you don't find other locations you like as much. 

**Step 8: Search <br>**
Next hit the back bottom on the top left above the activity image to continue the search for activities with the last filter applied. Now let's find a ladnmark location, click the "landmark" category. You will find "CN Tower". Click the card to view and follow above step 5 to add to trip on Jan 1, 2021 at 8:15pm.  

**Step 9: Go to Favourites <br>**
Once it's added, go to your favourites to add "Hothouse" as you think it would be the perfect spot with your current activities. Click the dropdown menu by your username on the top right of the screen. Click "Favourites" option and click "Hothouse". Follow step 5 to add to trip on Jan 1, 2021 at 6:00pm.  

**Step 10: Browse and Add More Activities to Trip <br>**
You can now click the back button and continue browsing using the different categories. Repeat steps 5-6 to add the following activities:
- LEGOLAND Discovery Centre (Jan 2, 2021 11:00am)
- iQ Food Co. (Jan 2, 2021 2:00pm)
- Art Gallery of Ontario (Jan 2, 2021 4:00pm)

**Step 11: View Itinerary Days <br>**
After all activities are added, you can view your final itinerary by using the drop down menu on the top right by your username. Click "Your Trips" and then on "Toronto 2021" trip click "View Itinerary". You will be able to view the itinerary of each day and the map of the locations.

**Step 12: Share <br>**
You may wish to share the trip with friends. This can be done in two ways. First option is to download the PDF of the trip by clicking the "Download" button. Another option is by clicking the "share" button redirects you to the URL of the trip without being signed-in to share with friends.

### Walthrough #2
**Step 1: Login <br>**
Click the login button on the top right corner of the screen. Provide your credentials from previous walkthrough and click "Login".

**Step 2: Go to Emergency Page <br>**
Go to "Emergecy Page" by using the drop down menu on the top right by your username. Click "Emergency". In the search bar, type city name and click search. You will be provided with all required emergency information. 

**Step 3: Sign Out <br>**
You can proceed to sign out by using the drop down menu on the top right by your username and clicking "Sign out".

### Walkthrough #3
**Step 1: Search Acitivites <br>**
You can now click on the Bookie logo on the top left to navigate to homescreen and begin your search. In the search bar, type "Toronto" and hit the search button. You will be directed to the activites screen with the city set to Toronto on the left panel.  

**Step 2: Select Acitivity <br>**
Proceed to select any activity by clicking the card to view infromation.

**Step 3: Log In Prompt <br>**
You will find a message requiring you to login in order to add the activity to favourites or trip. You can continue browsing without logging into the system if you wish by hitting the back button.

## Notes:
- DO NOT REFRESH THE PAGE AS THE SESSION IS NOT PERSISTENT AND SOME OF THE DATA YOU ENTERED MAY BE LOST.
- Categories in home page don't do anything
- The rating & people slider in the search results page don't do anything but the field next to them works as expected (rating is in the range 0-5)
- In the search result page, multiple categories may look like they are selected if you check multiple without unchecking the already checked ones. (It only uses the last checked category)
- Itinerary Activities may not be sorted by time. (We did not implement a sorting algorithm that sorts based on time when displaying)
- Google map integration defaults to toronto if there are no activities for that day in the itinerary).
- You cannot edit or remove activities from the itinerary (To implement this we would have to redo the entire booking system and that would take too long.)
- Share itinerary is hacky solution since we cannot persist sessions, so when you press share you are automatically logged out and directed to the resulting share page but you can not actually share the link as the data is not persistent.
- Download itinerary just shows what a sample looks like and does not have accurate data.
- Emergency page only shows toronto information.
