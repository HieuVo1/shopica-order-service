using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.ViewModels.Carts
{
    public class UpdateProductDetailQuantityRequest
    {
        public int ProductDetailID { set; get; }
        public int Quantity { set; get; }
        public decimal Price { set; get; }
    }
}
