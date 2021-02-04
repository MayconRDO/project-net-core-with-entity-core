using System.Collections.Generic;

namespace MimicryAPI.Helpers
{
    public class PaginationList<T> : List<T>
    {
        public Pagination Pagination { get; set; }
    }
}