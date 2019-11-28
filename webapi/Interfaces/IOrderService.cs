using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webapi.core.DTOs;
using webapi.core.UseCases;

namespace webapi.Interfaces
{
    public interface IOrderService
    {
      Task<ActionResult> GetOrdersAsync (Pagination pagination, SearchOrder search);
      Task<ActionResult> GetOrderAsync (string id);
      Task<ActionResult> AcceptOrderAsync (string id, int UserId);
      Task<ActionResult> RefuseOrderAsync (string id, int UserId);
      Task<ActionResult> AddOrderAsync (AddOrder values);
    }
}