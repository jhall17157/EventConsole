using System;
using NLog.Web;
using System.IO;
using System.Linq;
using System.Collections.Generic;


namespace EventConsole
{
    class Program
       {
           private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
           
            logger.Info("Program started");
        var db = new EventContext();

            // generate locations if they do not already exist
            int locations_count = db.Locations.Count();
            if (locations_count == 0){
                List<Location> locations = new List<Location>(){
                    new Location { Name = "Front Door"},
                    new Location { Name = "Back Door"},
                    new Location { Name = "Office"},

                };
                // save locations to database
                db.AddLocations(locations);
                logger.Info("{0} locations saved to database", locations.Count() );
            } else {
                logger.Info("Database already contains {0} locations", locations_count );
            }
            Location[] LocationsArray = db.Locations.ToArray();
            
            // create date object containing current date/time
            DateTime localDate = DateTime.Now;
            // subtract 6 months from date
            DateTime eventDate = localDate.AddMonths(-6);
            // instantiate Random class
            Random rnd = new Random();
            // create list to store events
            List<Event> events = new List<Event>();
            // loop for each day in the range from 6 months ago to today
            while (eventDate < localDate)
            {
                // random between 0 and 5 determines the number of events to occur on a given day
                int num = rnd.Next(0, 6);
                // a sorted list will be used to store daily events sorted by date/time - each time an event is added, the list is re-sorted
                SortedList<DateTime, Event> dailyEvents = new SortedList<DateTime, Event>();
                // for loop to generate times for each event
                for (int i = 0; i < num; i++)
                {
                    // random between 0 and 23 for hour of the day
                    int hour = rnd.Next(0, 24);
                    // random between 0 and 59 for minute of the day
                    int minute = rnd.Next(0, 60);
                    // random between 0 and 59 for seconds of the day
                    int second = rnd.Next(0, 60);
                    // random location (use Locations)
                    int loc = rnd.Next(0, LocationsArray.Count());
                    // create date/time for event
                    DateTime d = new DateTime(eventDate.Year, eventDate.Month, eventDate.Day, hour, minute, second);
                    // create event from date/time and location
                    Event e = new Event { Flagged = false, Location = LocationsArray[loc], LocationId = LocationsArray[loc].LocationId, TimeStamp = d };
                    // add daily event to sorted list
                    dailyEvents.Add(d, e);
                }

                // loop thru sorted list for daily events
                foreach (var de in dailyEvents)
                {
                    // add daily event to Events
                    events.Add(de.Value);
                }
                // add 1 day to eventDate
                eventDate = eventDate.AddDays(1);
            }
            // save events to db
            // foreach(Event e in events)
            // {
            //     db.AddEvent(e);
            // }
            db.AddEvents(events);

            logger.Info("Program ended");

        }     
    }
}