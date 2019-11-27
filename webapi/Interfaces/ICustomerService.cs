using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.DTOs;
using webapi.core.UseCases;

namespace webapi.Interfaces
{
    public interface ICustomerService
    {
        Task<ActionResult> GetCustomersAsync (Pagination pagination, SearchCustomer search);
        Task<ActionResult> GetCustomerAsync(string id);
        Task<ActionResult> PutCustomerAsync(string id, SaveCustomerDTO saveCustomerDTO);
    }
}