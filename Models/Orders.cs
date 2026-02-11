using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupplyChain.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }
        public string Status { get; set; }

    }
}