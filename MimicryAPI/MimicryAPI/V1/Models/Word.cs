using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MimicryAPI.V1.Models
{
    public class Word
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Point { get; set; }
        [Required]
        public bool Active { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
