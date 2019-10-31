using Microsoft.EntityFrameworkCore;

namespace webapi.infrastructure.Persistance {
    public class DbContextFactory : DesignTimeDbContextFactoryBase<AppDbContext> {
        protected override AppDbContext CreateNewInstance (DbContextOptions<AppDbContext> options) {
            return new AppDbContext (options);
        }
    }

}