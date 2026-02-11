using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SupplyChain.Models
{
    public class Sellers
    {
        public int Id { get; set; }
        [DisplayName("Enter Username")]
        [Required(ErrorMessage = "Username is required")]
        public string SellerName { get; set; }
        [Required(ErrorMessage = "password please")]
        [DataType(DataType.Password)]
        public string Password {  get; set; }
    }
}