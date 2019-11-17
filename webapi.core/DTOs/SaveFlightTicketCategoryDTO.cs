namespace webapi.core.DTOs
{
    public class SaveFlightTicketCategoryDTO
    {
        public string FlightId { get; set; }
        public int TicketCategoryId { get; set; }
        public decimal Price { get; set; }
    }
}