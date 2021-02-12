using System.ComponentModel.DataAnnotations;

namespace TasksAPI.API.Models
{
    public class UserDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "O e-mail é obrigatório")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string PasswordConfirmation { get; set; }
    }
}
