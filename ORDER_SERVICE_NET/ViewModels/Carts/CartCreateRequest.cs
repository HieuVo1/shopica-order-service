using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.ViewModels.Carts
{
    public class CartCreateRequest
    {
        public int Id { set; get; }
        public decimal Price { set; get; }
        public string CreateAt { set; get; }
        public List<CartItemView> Items { set; get; }
    }
}
