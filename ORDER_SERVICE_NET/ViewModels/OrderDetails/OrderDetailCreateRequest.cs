using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.ViewModels.OrderDetails
{
    public class OrderDetailCreateRequest
    {
        public int Quantity { get; set; }
        public decimal TotalPriceProduct { get; set; }
        public int ProductDetailId { get; set; }
        public string ProductName { get; set; }
    }
}
