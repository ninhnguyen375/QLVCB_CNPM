using System.Collections.Generic;
using webapi.core.Domain.Entities;
using webapi.core.DTOs;
using webapi.core.UseCases;

namespace webapi.Interfaces
{
    public interface ICustomerService
    {
        IEnumerable<CustomerDTO> GetCustomers (Pagination pagination, SearchCustomer search);
        CustomerDTO GetCustomer(string id);
        DataResult PutCustomer(string id, SaveCustomerDTO saveCustomerDTO);
    }
}