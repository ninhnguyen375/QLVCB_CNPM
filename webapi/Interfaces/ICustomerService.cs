using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.core.Domain.Entities;
using webapi.core.DTOs;
using webapi.core.UseCases;

namespace webapi.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDTO>> GetCustomersAsync (Pagination pagination, SearchCustomer search);
        Task<CustomerDTO> GetCustomerAsync(string id);
        Task<DataResult> PutCustomerAsync(string id, SaveCustomerDTO saveCustomerDTO);
    }
}