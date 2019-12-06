using System.ComponentModel.DataAnnotations;
namespace webapi.core.DTOs
{
    public class AirportDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
    }
}