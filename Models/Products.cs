using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupplyChain.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

        public int CategoryId { get; set; }
        public int SupplierId { get; set; }

        public Categories Categories { get; set; }
        public Suppliers Suppliers { get; set; }
    }
}