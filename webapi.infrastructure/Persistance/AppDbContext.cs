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
    }
}