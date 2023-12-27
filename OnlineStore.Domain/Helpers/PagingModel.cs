using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Domain.Helpers
{
    public class PagingModel
    {
        public int PageNumber { get; set; }
        public int CountPage { get; set; }

        public Func<int?, string> PageUrl { get; set; }
    }
}
