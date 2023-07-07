using System;
using System.Collections.Generic;

namespace Commerce.Domain.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string? UserEmail { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? HasPassword { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
