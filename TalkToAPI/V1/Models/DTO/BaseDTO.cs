using System.Collections.Generic;

namespace TalkToAPI.V1.Models.DTO
{
    /// <summary>
    /// Classe base DTO
    /// </summary>
    public abstract class BaseDTO
    {
        /// <summary>
        /// Lista de link DTO
        /// </summary>
        public List<LinkDTO> Links { get; set; } = new List<LinkDTO>();
    }
}
