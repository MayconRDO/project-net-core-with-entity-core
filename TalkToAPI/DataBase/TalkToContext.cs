using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalkToAPI.V1.Models;

namespace TalkToAPI.DataBase
{
    /// <summary>
    /// Classe de contexto
    /// </summary>
    public class TalkToContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="options"></param>
        public TalkToContext(DbContextOptions<TalkToContext> options) : base(options)
        {

        }

        /// <summary>
        /// Tabela de Mensagem
        /// </summary>
        public DbSet<Message> Messages { get; set; }

        /// <summary>
        /// Tabela de Token
        /// </summary>
        public DbSet<Token> Tokens { get; set; }

    }
}
