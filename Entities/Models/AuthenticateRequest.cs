using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class AuthenticateRequest
    {
        [Required]
        public string Email { get; set; }
    }
}