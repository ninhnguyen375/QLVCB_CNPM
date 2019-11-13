using System.Collections.Generic;

namespace webapi.core.UseCases
{
    public class TicketCategoriesOfSearchFlightFE {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
    public class SearchFlightFE
    {
        public string AirportFrom { get; set; }
        public string AirportTo { get; set; }
        public string DepartureDate { get; set; }
        public string ReturnDate { get; set; }
        public IList<TicketCategoriesOfSearchFlightFE> TicketCategories  { get; set; }
    }
}