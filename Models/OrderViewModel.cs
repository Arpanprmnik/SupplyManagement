using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupplyChain.Models
{
    public class OrderViewModel
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string ProductName { get; set; }
    }
}