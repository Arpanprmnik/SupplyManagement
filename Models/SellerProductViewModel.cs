using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SupplyChain.Models
{
    public class SellerProductViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string SKU { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int SupplierId { get; set; }

        public int WarehouseId { get; set; }
        public int Quantity { get; set; }

        // For dropdown
        public IEnumerable<Warehouses> Warehouses { get; set; }

        public IEnumerable<Categories> Categories { get; set; }
        public IEnumerable<Suppliers> Suppliers { get; set; }
    }
}