using System.Collections.Generic;

namespace MimicryAPI.Models.DTO
{
    public abstract class BaseDTO
    {
        public List<LinkDTO> Links { get; set; }
    }
}
