using System.Collections.Generic;
using webapi.core.DTOs;
using webapi.core.UseCases;

namespace webapi.Interfaces
{
    public interface IOrderService
    {
      IEnumerable<OrderDTO> GetOrders (Pagination pagination, SearchOrder search);
      OrderDTO GetOrder (string id);
      DataResult AcceptOrder (string id, int UserId);
      DataResult RefuseOrder (string id, int UserId);
      DataResult PostOrder (AddOrder values);
    }
}