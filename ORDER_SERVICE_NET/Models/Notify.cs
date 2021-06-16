using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ORDER_SERVICE_NET.Models
{
    public partial class Notify
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int StoreId { get; set; }
        public int OrderId { get; set; }
        public string Type { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        public int IsRead { get; set; }
    }
}
