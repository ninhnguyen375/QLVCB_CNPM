using System.ComponentModel.DataAnnotations;
using webapi.core.Interfaces;

namespace webapi.core.Domain.Entities
{
    public class Airline : IAggregateRoot
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}