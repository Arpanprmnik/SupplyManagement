using SupplyChain.Data;
using System.Web.Management;
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
        public JsonResult Login(string email,string password,string returnUrl)
        {
            var manager = _managerRepo.GetByEmail(email);
            if(manager == null || manager.Password != password)
            {
                return Json(new
                {
                    success = false,
                    message = "your password or email is not right"
                });
            }
            Session["ManagerId"] = manager.Id;
            return Json(new
            {
                success =true,
                redirectUrl = returnUrl ?? Url.Action("Index", "Manager")
            });
        }

        // GET: /Manager
        //public ActionResult Index()
        //{
        //    if (Session["ManagerId"] == null)
        //        return RedirectToAction("Login");

        //    var orders = _orderRepo.GetAllOrders();
        //    return View(orders);
        //}

        public ActionResult Index()
        {
            if (Session["ManagerId"] == null)
                return RedirectToAction("Login");

            return View();
        }

        public JsonResult GetOrders()
        {
            var orders = _orderRepo.GetAllOrders();

            return Json(new
            {
                data = orders
            }, JsonRequestBehavior.AllowGet);
        }
        // POST: /Manager/Approve
        [HttpPost]
        public JsonResult UpdateStatus(int orderId, string status)
        {
            _orderRepo.UpdateOrderStatus(orderId, status);

            if (status == "Approved")
                _orderRepo.UpdateInventory(orderId);
            else if (status == "Rejected")
                _orderRepo.IncreaseInventory(orderId);

            return Json(new { success = true });
        }

        public ActionResult Inventory()
        {
            if (Session["ManagerId"] == null)
                return RedirectToAction("Login");

            //var inventory = _managerRepo.GetInventory();
            return View();
        }
        public JsonResult GetInventory()
        {
            var inventory = _managerRepo.GetInventory();
            return Json(new
            {
                data =inventory
            },JsonRequestBehavior.AllowGet);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}
