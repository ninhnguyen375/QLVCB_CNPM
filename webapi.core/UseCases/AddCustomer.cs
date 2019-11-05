using System;

namespace webapi.core.UseCases
{
    public class AddCustomer
    {
      // Property of Customer
      public string CustomerId { get; set; }
      public string FullName { get; set; }
      public string Phone { get; set; }
      public int BookingCount { get; set; } = 1;

      // Property of Order
      public int TicketCount { get; set; }
      public decimal TotalPrice { get; set; }
      public DateTime CreateAt { get; set; }
      public int Status { get; set; } = 0;
    }
}