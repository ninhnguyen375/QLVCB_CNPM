using System.ComponentModel.DataAnnotations;

namespace webapi.core.DTOs
{
    public class SaveAirportDTO
    {
        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 4)]
        public string Name { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 4)]
        public string Location { get; set; }
    }
}