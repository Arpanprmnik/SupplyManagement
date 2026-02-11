using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupplyChain.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public int WarehouseID { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}