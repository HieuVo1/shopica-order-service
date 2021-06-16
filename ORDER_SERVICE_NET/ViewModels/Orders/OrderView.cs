using ORDER_SERVICE_NET.Models;
using ORDER_SERVICE_NET.ViewModels.Address;
using ORDER_SERVICE_NET.ViewModels.OrderDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.ViewModels.Orders
{
    public class OrderView
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public AddressInfo Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string State { get; set; }
        public string QrCode { get; set; }
        public string Notes { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal ShippingCost { get; set; }
        public DateTime? Created_at { get; set; }

        public List<OrderDetailView> OrderDetails { get; set; }
    }
}
