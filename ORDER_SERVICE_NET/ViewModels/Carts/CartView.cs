using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.ViewModels.Carts
{
    public class CartView
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
        public List<CartItemView> CartItems { get; set; }
    }
}
