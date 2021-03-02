using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TalkToAPI.V1.Models.DTO
{
    /// <summary>
    /// Classe Link DTO
    /// </summary>
    public class LinkDTO
    {
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="rel"></param>
        /// <param name="href"></param>
        /// <param name="method"></param>
        public LinkDTO(string rel, string href, string method)
        {
            Rel = rel;
            Href = href;
            Method = method;
        }

        /// <summary>
        /// Rel
        /// </summary>
        public string Rel { get; set; }

        /// <summary>
        /// Href
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        /// Method
        /// </summary>
        public string Method { get; set; }

    }
}
