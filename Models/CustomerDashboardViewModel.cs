using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupplyChain.Models
{
    public class CustomerDashboardViewModel
    {
        public IEnumerable<Products> Products { get; set; }
        public IEnumerable<Orders> Orders { get; set; }
    }
}