using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TalkToAPI.V1.Models.DTO
{
    /// <summary>
    /// Classe view Login
    /// </summary>
    public class LoginDTO
    {
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
    }
}
