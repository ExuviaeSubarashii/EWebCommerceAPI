using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commerce.Domain.Models
{
    public partial class PaymentInformations
    {
        public int Id { get; set; }
        public string? CardNumber { get; set; }
        public string? ExpirationDate { get; set;}
        public string? SecurityCode { get; set;}
        public string? PostalCode { get; set; }
    }
}
