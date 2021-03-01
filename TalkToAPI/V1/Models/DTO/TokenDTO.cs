using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TalkToAPI.V1.Models.DTO
{
    /// <summary>
    /// Classe o objeto token
    /// </summary>
    public class TokenDTO
    {
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Data de expiração do token
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// Token de expiração
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Data de expiração do token renovado
        /// </summary>
        public DateTime ExpirationRefreshToken { get; set; }

        /// <summary>
        /// Slogan
        /// </summary>
        public string Slogan { get; set; }
    }
}
