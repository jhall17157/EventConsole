using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;


namespace EventConsole
{
    public class EventContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Location> Locations { get; set; }

        public void AddLocation(Location location)
        {
            this.Locations.Add(location);
            this.SaveChanges();
        }
          public void AddLocations(List<Location> locations)
        {
            this.Locations.AddRange(locations);
            this.SaveChanges();
        }

        public void AddEvents(List<Event> events) 
        {
            this.Events.AddRange(events);
            this.SaveChanges();
        }
         public void AddEvent(Event events) 
        {
            this.Events.Add(events);
            this.SaveChanges();
        }
      

      

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            optionsBuilder.UseSqlServer(@config["EventContext:ConnectionString"]);
        }
    }
}