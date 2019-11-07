using System.Collections.Generic;
using System;

namespace webapi.core.UseCases
{
    public class Passenger {
      public string PassengerName { get; set; }
      public int PassengerGender { get; set; }
      public IList<int> LuggageIds { get; set; }
      public int TicketCategoryId { get; set; }
    }

    public class AddOrder
    {
      // Property of Customer
      public string CustomerId { get; set; } // CMND
      public string FullName { get; set; }
      public string Phone { get; set; }

      // Property of Order
      public int TicketCount { get; set; }
      public decimal TotalPrice { get; set; }

      // Property of Ticket
      public IList<Passenger> Passengers { set; get; }
      public IList<string> FlightIds { set; get; }
    }
}