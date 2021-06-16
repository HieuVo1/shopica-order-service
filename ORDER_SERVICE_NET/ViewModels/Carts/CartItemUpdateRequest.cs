using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.ViewModels.Carts
{
    public class CartItemUpdateRequest
    {
        public int OldProductDetailID { set; get; }
        public int NewProductDetailID { set; get; }
        public int AccountId { set; get; }
        public int Quantity { set; get; }

        public decimal Price { set; get; }
    }
}
