using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupplyChain.Models
{
    public class Suppliers
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

    }
}