using ORDER_SERVICE_NET.ViewModels.OrderDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.ViewModels.Orders
{
    public class BestSeller
    {
        public List<ProductDetailQuantity> ProductDetails { get; set; }
        public List<OrderState> OrderStates { get; set; }
    }
}
