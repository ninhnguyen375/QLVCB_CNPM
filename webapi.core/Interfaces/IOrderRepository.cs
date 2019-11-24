using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.core.Domain.Entities;

namespace webapi.core.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Ticket>> GetTicketsByIdAsync(string id);
    }
}