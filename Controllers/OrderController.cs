using System.Web.Mvc;
using SupplyChain.Data;

namespace SupplyChain.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderRepository _repo = new OrderRepository();
        // GET: Order
        public ActionResult Create(int productId)
        {
            if (Session["CustomerId"] == null)
            {
                return RedirectToAction("Login", "Account", new { productId });
            }
            ViewBag.ProductId = productId;
            return View();
        }
    [HttpPost]
    public ActionResult Create(int productId,int quantity)
    {
            int customerId = (int)Session["CustomerId"];
            int orderId = _repo.CreateOrder(customerId);
            _repo.AddOrderItem(orderId, productId, quantity);
            return RedirectToAction("Success", new { orderId });
    }
    public ActionResult Success(int orderId)
        {
            ViewBag.OrderId = orderId;
            ViewBag.CustomerId = Session["CustomerId"];
            return View();
        }
    }

}