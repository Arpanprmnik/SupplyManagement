using SupplyChain.Data;
using System.Web.Mvc;

namespace SupplyChain.Controllers
{
    public class ManagerController : Controller
    {
        private readonly OrderRepository _orderRepo = new OrderRepository();
        private readonly ManagerRepository _managerRepo = new ManagerRepository();

        // GET: /Manager/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: /Manager/Login
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var manager = _managerRepo.GetByEmail(email);

            if (manager == null || manager.Password != password)
            {
                ViewBag.Error = "Invalid login";
                return View();
            }

            Session["ManagerId"] = manager.Id;
            return RedirectToAction("Index");
        }

        // GET: /Manager
        public ActionResult Index()
        {
            if (Session["ManagerId"] == null)
                return RedirectToAction("Login");

            var orders = _orderRepo.GetAllOrders();
            return View(orders);
        }

        // POST: /Manager/Approve
        [HttpPost]
        public ActionResult UpdateStatus(int orderId, string status)
        {
            _orderRepo.UpdateOrderStatus(orderId, status);

            // Only reduce inventory if approved
            if (status == "Approved")
            {
                _orderRepo.UpdateInventory(orderId);
            }
            else if(status == "Rejected")
            {
                _orderRepo.IncreaseInventory(orderId);

            }

                return RedirectToAction("Index");
        }

        public ActionResult Inventory()
        {
            if (Session["ManagerId"] == null)
                return RedirectToAction("Login");

            var inventory = _managerRepo.GetInventory();
            return View(inventory);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}
