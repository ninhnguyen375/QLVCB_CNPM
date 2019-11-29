using webapi.core.Domain.Entities;

namespace webapi.core.UseCases
{
    public class PassengerDF
    {
        public string Name { get; set; }
        // 1. Male, 2. Female, 3. Other
        public int Gender { get; set; }
        public int LuggageId { get; set; }
        public Luggage Luggage { get; set; }
        public int TicketCategoryId { get; set; }
        public TicketCategory TicketCategory { get; set; } 
    }
}