using System.Collections.Generic;
using webapi.core.Domain.Entities;

namespace webapi.core.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        IEnumerable<Order> GetOrdersById(string id);
    }
}