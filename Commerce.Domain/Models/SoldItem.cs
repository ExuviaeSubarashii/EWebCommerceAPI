using System;
using System.Collections.Generic;

namespace Commerce.Domain.Models
{
    public partial class SoldItem
    {
        public int SalesId { get; set; }
        public string? BuyerName { get; set; }
        public string? SoldItems { get; set; }
        public int? FullPrice { get; set; }
        public string? PriceTag { get; set; }
    }
}
