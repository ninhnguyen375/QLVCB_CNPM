using webapi.core.Domain.Entities;
using webapi.core.Interfaces;

namespace webapi.infrastructure.Persistance.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
      public CustomerRepository(AppDbContext context) : base(context)
      {
      }
      protected AppDbContext AppDbContext {
        get { return Context as AppDbContext; }
      }
    }
}