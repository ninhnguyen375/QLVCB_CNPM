using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.core.Domain.Entities;
using webapi.core.UseCases;

namespace webapi.core.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Ticket>> GetTicketsByIdAsync(string id);
        Task<IEnumerable<PassengerDF>> GetTicketsByDateFlightAsync(int dateId, string flightId);
    }
}