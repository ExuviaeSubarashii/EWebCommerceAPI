using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commerce.Domain.Models
{
    public partial class Orders
    {
        public int OrderId { get; set; }
        public string? OrdererName { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? ItemName { get; set; }
        public string? ItemAmount { get; set; }
        public string? TotalPrice { get; set; }
    }
}
