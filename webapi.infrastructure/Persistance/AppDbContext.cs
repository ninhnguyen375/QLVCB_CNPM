using Microsoft.EntityFrameworkCore;
using webapi.core.Domain.Entities;

namespace webapi.infrastructure.Persistance {
    public class AppDbContext : DbContext {
        public AppDbContext (DbContextOptions<AppDbContext> options) : base (options) {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<Luggage> Luggages { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Flight> Flights {get; set; }
        public DbSet<Date> Dates {get; set; }
        public DbSet<DateFlight> DateFlights {get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DateFlight>()
                .HasKey(df => new { df.DateId, df.FlightId });  

            modelBuilder.Entity<DateFlight>()
                .HasOne<Flight>(f => f.Flight)
                .WithMany(df => df.DateFlights)
                .HasForeignKey(f => f.FlightId);  

            modelBuilder.Entity<DateFlight>()
                .HasOne<Date>(d => d.Date)
                .WithMany(df => df.DateFlights)
                .HasForeignKey(d => d.DateId);


            base.OnModelCreating(modelBuilder);
        }

    }
}