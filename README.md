# CPSC481
Repository to store UI code for CPSC481 term project

## Team Members
Brianna Hoang <br>
Youstina Attia<br>
Vi Tsang<br>
Yanessa Lacsamana<br>
Zeyad Omran<br>


## Set up
- 1) Download and install [node.js](https://nodejs.org/en/) (We're using v16.14.0).
- 2) Download and install [dotnet 6.0](https://dotnet.microsoft.com/en-us/learn/aspnet/blazor-tutorial/install) (We're using v6.0.200).
- 3) cd into `CPSC481/CPSC481` (The sub dir not the root of the project).
- 4) run `npm install` in your terminal.
- 5) run `dotnet watch` in your terminal.

## Notes when using the system.
- DO NOT REFRESH THE PAGE AS THE SESSION IS NOT PERSISTENT AND SOME OF THE DATA YOU ENTERED MAY BE LOST.
- Categories in home page don't do anything
- The rating & people slider in the search results page don't do anything but the field next to them works as expected (rating is in the range 0-5)
- In the search result page, multiple categories may look like they are selected if you check multiple without unchecking the already checked ones. (It only uses the last checked category)
- Itinerary Activities may not be sorted by time. (We did not implement a sorting algorithm that sorts based on time when displaying)
- google map integration defaults to toronto if there are no activities for that day in the itinerary).
- You cannot edit or remove activities from the itinerary (To implement this we would have to redo the entire booking system and that would take too long.)
- Share itinerary is hacky solution since we cannot persist sessions, so when you press share you are automatically logged out and directed to the resulting share page but you can not actually share the link as the data is not persistent.
- Download itinerary just shows what a sample looks like and does not have accurate data.
- Emergency page only shows toronto information.
## System Walkthrough

