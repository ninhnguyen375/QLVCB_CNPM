using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

      public async Task<IEnumerable<Order>> GetOrdersByIdAsync(string id) {
        return (
          await Context.Orders.Where(o =>
            o.CustomerId.Equals(id)).ToListAsync()
        );
      }
    }
}