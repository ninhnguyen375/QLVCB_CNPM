using System.ComponentModel.DataAnnotations;
using webapi.core.Interfaces;

namespace webapi.core.Domain.Entities
{
    public class Luggage : IAggregateRoot
    {
      [Key]
      public int Id { get; set; }

      [Required]
      public int LuggageWeight { get; set; }

      [Required]
      public decimal Price { get; set; }
    }
}