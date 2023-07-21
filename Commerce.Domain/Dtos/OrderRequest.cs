using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commerce.Domain.Dtos
{
    public class OrderRequest
    {
        public string OrdererName { get; set; }
        public string ItemNames { get; set; }
        public string ItemAmounts { get; set; }
        public decimal TotalPrice { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public string PostalCode { get; set; }
    }
}
