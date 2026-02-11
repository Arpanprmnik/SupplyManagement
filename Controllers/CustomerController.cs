using SupplyChain.Data;
using SupplyChain.Models;
using System.Web.Mvc;

namespace SupplyChain.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ProductRepository _productRepo = new ProductRepository();
        private readonly OrderRepository _orderRepo = new OrderRepository();


        public ActionResult Dashboard()
        {
            if (Session["CustomerId"] == null)
                return RedirectToAction("Login", "Account");

            return View(); 
        }

        public JsonResult GetDashboardData()
        {
            if (Session["CustomerId"] == null)
                return Json(null, JsonRequestBehavior.AllowGet);

            int customerId = (int)Session["CustomerId"];


            var model = new CustomerDashboardViewModel
            {
                Products = _productRepo.GetAllProducts(),
                Orders = _orderRepo.GetOrdersByCustomerId(customerId)
            };

            return Json(new
            {
                products = model.Products,
                orders = model.Orders

            }, JsonRequestBehavior.AllowGet);
        }
    }
}
