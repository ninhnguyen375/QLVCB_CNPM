using System.Collections.Generic;
using webapi.core.Domain.Entities;

namespace webapi.core.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        IEnumerable<Ticket> GetTicketsById(string id);
    }
}