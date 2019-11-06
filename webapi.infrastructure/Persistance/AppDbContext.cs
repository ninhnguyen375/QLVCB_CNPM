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
        public DbSet<TicketCategory> TicketCatogories {get; set; }
        public DbSet<FlightTicketCategory> FlightTicketCategories {get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // For table DateFlight
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

            // For table FlightTicketCategory
            modelBuilder.Entity<FlightTicketCategory>()
                .HasKey(ftc => new { ftc.TicketCategoryId, ftc.FlightId });  

            modelBuilder.Entity<FlightTicketCategory>()
                .HasOne<Flight>(f => f.Flight)
                .WithMany(ftc => ftc.FlightTicketCategories)
                .HasForeignKey(f => f.FlightId);  

            modelBuilder.Entity<FlightTicketCategory>()
                .HasOne<TicketCategory>(tc => tc.TicketCategory)
                .WithMany(ftc => ftc.FlightTicketCategories)
                .HasForeignKey(tc => tc.TicketCategoryId);


            base.OnModelCreating(modelBuilder);
        }

    }
}