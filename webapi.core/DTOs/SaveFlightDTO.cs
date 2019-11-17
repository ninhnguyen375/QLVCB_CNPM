using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using webapi.core.Domain.Entities;

namespace webapi.core.DTOs
{
    public class SaveFlightDTO
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Thời gian khởi hành không được để trống.")]
        // Thiếu RegEx 
        public int StartTime { get; set; }

        [Required(ErrorMessage = "Thời gian bay không được để trống.")]
        // Thiếu RegEx 
        public int FlightTime { get; set; }

        [Required(ErrorMessage = "Sân bay đi không được để trống.")]
        public string AirportFrom { get; set; }

        [Required(ErrorMessage = "Sân bay đến không được để trống.")]
        public string AirportTo { get; set; }

        // Thiếu Regex
        public int SeatsCount { get; set; }

        public int Status { get; set; }

        [Required(ErrorMessage = "Hãng hàng không không được để trống.")]
        public string AirlineId { get; set; }

        public virtual ICollection<FlightTicketCategory> FlightTicketCategories { get; set; } 

    }
}