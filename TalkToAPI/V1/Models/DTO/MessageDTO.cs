using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TalkToAPI.V1.Models.DTO
{
    /// <summary>
    /// Classe de Mensagem DTO
    /// </summary>
    public class MessageDTO : BaseDTO
    {
        /// <summary>
        /// Identificador do objeto
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Usuário remetente
        /// </summary>
        public ApplicationUser From { get; set; }

        /// <summary>
        /// Identificador do remetente
        /// </summary>
        public string FromId { get; set; }

        /// <summary>
        /// Usuário destion
        /// </summary>
        public ApplicationUser To { get; set; }

        /// <summary>
        /// Identificador do destinatário
        /// </summary>
        public string ToId { get; set; }

        /// <summary>
        /// Texto da mensagem
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Flag indicativa de exclusão
        /// </summary>
        public bool Exclude { get; set; }

        /// <summary>
        /// Data de criação
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Data de modificação
        /// </summary>
        public DateTime? ModifiedDate { get; set; }
    }
}
