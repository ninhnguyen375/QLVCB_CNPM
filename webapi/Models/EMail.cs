using System.ComponentModel.DataAnnotations;
namespace webapi.Models
{
  public class Email
  {
    [Required]
    public string ToName { get; set; }
    [Required, EmailAddress]
    public string ToEmail { get; set; }
    [Required]
    public string Subject { get; set; }
    [Required]
    public string Message { get; set; }
  }
}