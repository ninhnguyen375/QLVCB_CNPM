using System;

namespace webapi.core.UseCases
{
    public class SearchOrder
    {
        public int? TicketCount { get; set; }
        public decimal? TotalPrice  { get; set; }
        public int? Status { get; set; }
        public string DateFrom { get; set; } = "";
        public string DateTo { get; set; } = "";
        public string DateCreateFrom { get; set; } = "";
        public string DateCreateTo { get; set; } = "";
        public string CustomerId { get; set; } = "";
        public int? UserId { get; set; }
        public string sortAsc { get; set; } = "";
        public string sortDesc { get; set; } = "";
    }
}