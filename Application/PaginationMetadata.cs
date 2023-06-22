using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class PaginationMetadata
    {
        public int ItemCount { get; set; }
        public int PageCount { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}
