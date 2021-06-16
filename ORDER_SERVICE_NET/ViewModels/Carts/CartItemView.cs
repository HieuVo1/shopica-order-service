using ORDER_SERVICE_NET.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.ViewModels.Carts
{
    public class CartItemView
    {
        public string ProductName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string BrandName { get; set; }
        public int Quantity { set; get; }
        public int Available { set; get; }
        public int ProductDetailId { set; get; }
        public int ProductId { set; get; }
        public string Image { get; set; }
        public int StoreId { get; set; }
        public decimal Price { set; get; }
    }
}
