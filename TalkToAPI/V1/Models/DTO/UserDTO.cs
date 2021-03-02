using System.ComponentModel.DataAnnotations;

namespace TalkToAPI.V1.Models.DTO
{
    /// <summary>
    /// Classe DTO do usuário
    /// </summary>
    public class UserDTO : BaseDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "O e-mail é obrigatório")]
        public string Email { get; set; }

        /// <summary>
        /// Senha
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Confirmação de senha
        /// </summary>
        [Required]
        [Compare("Password")]
        public string PasswordConfirmation { get; set; }

        /// <summary>
        /// Slogan
        /// </summary>
        public string Slogan { get; set; }
    }
}
