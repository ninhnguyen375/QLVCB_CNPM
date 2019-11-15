using System.ComponentModel.DataAnnotations;

namespace webapi.core.DTOs
{
    public class SaveAirlineDTO
    {
        [Required]
        [StringLength(2, MinimumLength = 2)]
        public string Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 4)]
        public string Name { get; set; }
    }
}