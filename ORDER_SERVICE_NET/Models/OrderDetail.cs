using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ORDER_SERVICE_NET.Models
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPriceProduct { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public int OrderId { get; set; }
        public int ProductDetailId { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }

        public virtual Orders Order { get; set; }
    }
}
