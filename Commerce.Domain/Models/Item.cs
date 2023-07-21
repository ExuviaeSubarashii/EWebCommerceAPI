using System;
using System.Collections.Generic;

namespace Commerce.Domain.Models
{
    public partial class Item
    {
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public int? ItemPrice { get; set; }
        public string? ItemPriceTag { get; set; }
        public string? ItemImage { get; set; }
        public int? ItemStock { get; set; }
    }
}
