using System.Linq;
using webapi.core.Domain.Entities;

namespace webapi.infrastructure.Persistance {
  public class SeedData {
    public static void Initialize (AppDbContext context) {
      context.Database.EnsureCreated ();

      if (context.Users.Any ()) {
        return;
      }

      context.Users.AddRange (
        new User {
          Email = "admin@admin.com",
            FullName = "admin",
            Identifier = "00000000",
            Password = "$2b$10$IZshIpJy3mRvjTGJJYD45OOccUcUNI8RrCUvURHcemPbdNfXR/q3i",
            Role = 1,
            Status = 1
        }
      );
      context.SaveChanges ();
    }
  }
}