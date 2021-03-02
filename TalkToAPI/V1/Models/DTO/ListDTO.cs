using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TalkToAPI.V1.Models.DTO
{
    /// <summary>
    /// Classe lista de objetos
    /// </summary>
    public class ListDTO<T> : BaseDTO
    {
        /// <summary>
        /// Lista
        /// </summary>
        public List<T> List { get; set; }
    }
}
