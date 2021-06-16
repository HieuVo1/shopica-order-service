using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.ViewModels.Commons.Pagging
{
    public class PaggingView<T>
    {
        public int TotalElements { get; set; }
        public int PageSize { get; set; }
        public int Pageindex { get; set; }
        public List<T> Content { get; set; }
    }
}
