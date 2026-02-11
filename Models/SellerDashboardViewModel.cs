using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


    namespace SupplyChain.Models
    {
        public class SellerDashboardViewModel
        {
            public int ProductId { get; set; }
            public int InventoryId { get; set; }

            public string Name { get; set; }
            public decimal Price { get; set; }

            public int Quantity { get; set; }
        }
    }

