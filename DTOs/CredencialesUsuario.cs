using System.ComponentModel.DataAnnotations;

namespace peliculasWebApi.DTOs
{
    public class CredencialesUsuario
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
