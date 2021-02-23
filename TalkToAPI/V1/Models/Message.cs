﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TalkToAPI.V1.Models
{
    /// <summary>
    /// Classe do objeto Mensagem
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Identificador do objeto
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Usuário remetente
        /// </summary>
        [ForeignKey("FromId")]
        public ApplicationUser From { get; set; }

        /// <summary>
        /// Identificador do remetente
        /// </summary>
        public string FromId { get; set; }

        /// <summary>
        /// Usuário destion
        /// </summary>
        [ForeignKey("ToId")]
        public ApplicationUser To { get; set; }

        /// <summary>
        /// Identificador do destinatário
        /// </summary>
        public string ToId { get; set; }

        /// <summary>
        /// Proprietário da mensagem
        /// </summary>

        public ApplicationUser Owner { get; set; }

        /// <summary>
        /// Texto da mensagem
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Data de criação
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}