using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicryAPI.Helpers
{
    public class WordUrlQuery
    {
        public DateTime? Date { get; set; }
        public int? PageNumber { get; set; }
        public int? RecordPerPage { get; set; }
    }
}
