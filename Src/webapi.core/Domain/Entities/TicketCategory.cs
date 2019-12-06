using System.ComponentModel.DataAnnotations;
using webapi.core.Interfaces;
using System.Collections.Generic;

namespace webapi.core.Domain.Entities
{
    public class TicketCategory : IAggregateRoot
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<FlightTicketCategory> FlightTicketCategories { get; set; } 
        
    }
}