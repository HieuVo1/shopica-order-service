using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ORDER_SERVICE_NET.Models
{
    public partial class CartDetail
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int CartId { get; set; }
        public int ProductDetailId { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }

        public virtual Carts Cart { get; set; }
    }
}
