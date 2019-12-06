using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.core.Domain.Entities;

namespace webapi.core.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<IEnumerable<Order>> GetOrdersByIdAsync(string id);
    }
}