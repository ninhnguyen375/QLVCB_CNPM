using System.Collections.Generic;
using System.Linq;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;

namespace webapi.infrastructure.Persistance.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
      public OrderRepository(AppDbContext context) : base (context) 
      {
      }

      protected AppDbContext AppDbContext {
        get { return Context as AppDbContext; }
      }

      public IEnumerable<Ticket> GetTicketsById(string id) {
        return (
          Context.Tickets.Where(t =>
            t.OrderId.Equals(id))
        );
      }
    }
}