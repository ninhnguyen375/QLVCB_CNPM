using System.ComponentModel.DataAnnotations;
using webapi.core.Interfaces;

namespace webapi.core.DTOs
{
    public class LuggageDTO
    {
      public int Id { get; set; }

      public int LuggageWeight { get; set; }

      public decimal Price { get; set; }
    }
}