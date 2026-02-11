using SupplyChain.Data;
using SupplyChain.Models;
using SupplyChain.Utils;
using System.Web.Mvc;

namespace SupplyChain.Controllers
{
    public class AccountController : Controller
    {
        private readonly CustomerRepository _repo = new CustomerRepository();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();        
            Session.Abandon();     

            return RedirectToAction("Login");
        }
        [HttpPost]
        public JsonResult Login(LoginViewModel model, string returnUrl)
        {
            if(string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return Json(new {success= false, message = "Email and password are required" });
            }

            var customer = _repo.GetByEmail(model.Email);
            if (customer == null)
            {
                if (string.IsNullOrEmpty(model.FullName) || string.IsNullOrEmpty(model.Phone))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Full name and phone are required for new users"
                    });
                }
                int id = _repo.InsertCustomer(
                    model.FullName,
                    model.Email,
                    model.Phone,
                    PasswordHelper.HashPassword(model.Password)
                );
                Session["CustomerId"] = id;

                return Json(new
                {
                    success = true,
                    message = "Registration successful",
                    redirectUrl = Url.Action("Dashboard", "Customer")
                });
            }
            if (!PasswordHelper.VerifyPassword(model.Password, customer.PasswordHash))
            {
                return Json(new { success = false, message = "Invalid password" });
            }

            Session["CustomerId"] = customer.Id;

            return Json(new
            {
                success = true,
                message = "Login successful",
                redirectUrl = returnUrl ?? Url.Action("Dashboard", "Customer")
            });
        }

        
        public JsonResult Signup(LoginViewModel model,string returnUrl)
        {
            int id = _repo.InsertCustomer(
                model.FullName,
                model.Email,
                model.Phone,
                model.Password
            );

            Session["CustomerId"] = id;
            return Json(new
            {
                success = true,
                redirectUrl = returnUrl ?? Url.Action("Dashboard", "Customer")
            });
        }
        public ActionResult Common()
        {
            return View();
        }

    }
}