using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commerce.Domain.Dtos
{
    public class NewItemRequest
    {
        public string? userName { get; set; }
        public string? itemName { get; set; }
        public int? itemPrice { get; set; }
        public int? itemStock { get; set; }
    }
}
