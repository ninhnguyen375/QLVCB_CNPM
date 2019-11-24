using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.core.DTOs;
using webapi.core.UseCases;

namespace webapi.Interfaces
{
    public interface IOrderService
    {
      Task<IEnumerable<OrderDTO>> GetOrdersAsync (Pagination pagination, SearchOrder search);
      Task<OrderDTO> GetOrderAsync (string id);
      Task<DataResult> AcceptOrderAsync (string id, int UserId);
      Task<DataResult> RefuseOrderAsync (string id, int UserId);
      Task<DataResult> PostOrderAsync (AddOrder values);
    }
}