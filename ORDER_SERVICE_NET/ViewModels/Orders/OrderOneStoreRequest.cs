using ORDER_SERVICE_NET.ViewModels.OrderDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.ViewModels.Orders
{
    public class OrderOneStoreRequest
    {
        public string Notes { get; set; }
        public string QrCode { set; get; }
        public decimal Total { set; get; }
        public int PromotionId { set; get; }
        public int StoreId { set; get; }
        public int Discount { set; get; }
        public int ShippingCost { set; get; }
        public List<OrderDetailCreateRequest> OrderDetails { set; get; }
    }
}
