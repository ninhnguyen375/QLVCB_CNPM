using System.ComponentModel.DataAnnotations;
using webapi.core.Interfaces;

namespace webapi.core.DTOs
{
    public class CustomerDTO
    {
        public string Id { get; set; }
        
        public string FullName { get; set; }

        public string Phone { get; set; }

        public int BookingCount { get; set; } = 1;
    }
}