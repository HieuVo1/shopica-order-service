using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.ViewModels.Orders
{
    public class OrderRequestByDate
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Top { get; set; }
        public string SortOrder { get; set; }
        public int StoreId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
