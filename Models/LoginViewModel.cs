using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupplyChain.Models
{
    public class LoginViewModel
    {
        public int ProductId { get; set; }

        public string FullName { get; set; }   // new user
        public string Email { get; set; }
        public string Phone { get; set; }

        // For future use (ignored now)
        public string Password { get; set; }
    }
}
