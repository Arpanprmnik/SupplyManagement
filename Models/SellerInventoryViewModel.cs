using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupplyChain.Models
{
    public class SellerInventoryViewModel
    {
        public int InventoryId { get; set; }
        public string ProductName { get; set; }
        public string WarehouseName { get; set; }
        public int Quantity { get; set; }
    }
}