using System.ComponentModel.DataAnnotations;

namespace TasksAPI.Models
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
        public int PasswordConfirmation { get; set; }
    }
}
