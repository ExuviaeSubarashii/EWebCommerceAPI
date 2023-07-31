using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions;
namespace Commerce.Domain.Dtos
{
    public class NewItemRequest
    {
        public string? userName { get; set; }
        public string? itemName { get; set; }
        public int? itemPrice { get; set; }
        public int? itemStock { get; set; }
        //public IFormFile itemImage { get; set; }
        //install Microsoft.AspNetCore.Http.Features
        //public IFormFile itemImage { get; set; }
    }
}
