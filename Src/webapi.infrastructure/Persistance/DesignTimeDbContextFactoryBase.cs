using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace webapi.infrastructure.Persistance {
    public abstract class DesignTimeDbContextFactoryBase<TContext>:
        IDesignTimeDbContextFactory<TContext> where TContext : DbContext {
            public TContext CreateDbContext (string[] args) {
                return Create (
                    Directory.GetCurrentDirectory (),
                    Environment.GetEnvironmentVariable ("ASPNETCORE_ENVIRONMENT"));
            }
            protected abstract TContext CreateNewInstance (
                DbContextOptions<TContext> options);

            public TContext Create () {
                var environmentName =
                    Environment.GetEnvironmentVariable (
                        "ASPNETCORE_ENVIRONMENT");

                var basePath = AppContext.BaseDirectory;

                return Create (basePath, environmentName);
            }

            private TContext Create (string basePath, string environmentName) {
                var builder = new ConfigurationBuilder ();

                var config = builder.Build ();

                // var connstr = config.GetConnectionString ("DefaultConnection");
                var connstr = "Data Source=App.db";
                // var connstr = "Server=localhost; Database=cnpm;User=root;Password=123456;";

                if (string.IsNullOrWhiteSpace (connstr)) {
                    throw new InvalidOperationException (
                        "Could not find a connection string named 'Default'.");
                }
                return Create (connstr);
            }

            private TContext Create (string connectionString) {
                if (string.IsNullOrEmpty (connectionString))
                    throw new ArgumentException (
                        $"{nameof(connectionString)} is null or empty.",
                        nameof (connectionString));

                var optionsBuilder = new DbContextOptionsBuilder<TContext> ();

                Console.WriteLine ("DesignTimeDbContextFactory.Create(string): Connection string: {0}", connectionString);

                optionsBuilder.UseSqlite (connectionString);
                // optionsBuilder.UseMySql (connectionString);

                var options = optionsBuilder.Options;
                return CreateNewInstance (options);
            }
        }

}