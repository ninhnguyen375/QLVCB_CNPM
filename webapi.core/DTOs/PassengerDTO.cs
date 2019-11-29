using webapi.core.Domain.Entities;

namespace webapi.core.DTOs
{
    public class PassengerDTO
    {
        public string Name { get; set; }
        // 1. Male, 2. Female, 3. Other
        public int Gender { get; set; }
        public int LuggageId { get; set; }
        public LuggageDTO Luggage { get; set; }
        public int TicketCategoryId { get; set; }
        public TicketCategoryDTO TicketCategory { get; set; }
    }
}