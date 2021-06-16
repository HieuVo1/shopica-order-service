using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.ViewModels.Products
{
    public class ProductView
    {
        public int ProductId { get; set; }
        public int ProductDetailId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string BrandName { get; set; }
        public string SizeName { get; set; }
        public string ColorName { get; set; }
        public string Image { get; set; }
        public int StoreId { get; set; }

    }
}
