using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TalkToAPI.V1.Models
{
    /// <summary>
    /// Classe do objeto de usário da aplicação
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Nome completo
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Slogan
        /// </summary>
        public string Slogan { get; set; }

        /// <summary>
        /// Lista de mensagens
        /// </summary>
        //[ForeignKey("From")]
        //public virtual ICollection<Message> Messages { get; set; }
    }
}
