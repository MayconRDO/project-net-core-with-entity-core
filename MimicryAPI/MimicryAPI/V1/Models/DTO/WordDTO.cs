using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicryAPI.V1.Models.DTO
{
    public class WordDTO : BaseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Point { get; set; }
        public bool Active { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
