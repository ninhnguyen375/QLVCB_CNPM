using System;

namespace webapi.core.UseCases
{
    public class AddOrder
    {
      // Property of Customer
      public string CustomerId { get; set; }
      public string FullName { get; set; }
      public string Phone { get; set; }

      // Property of Order
      public int TicketCount { get; set; }
      public decimal TotalPrice { get; set; }
    }
}