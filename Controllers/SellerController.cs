using SupplyChain.Data;
using SupplyChain.Models;
using System.Linq;
using System.Web.Mvc;

namespace SupplyChain.Controllers
{
    public class SellerController : Controller
    {
        private readonly SellerRepository _repo = new SellerRepository();


        // GET: /Seller/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: /Seller/Login
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var supplier = _repo.GetByEmail(email);

            if (supplier == null || supplier.PasswordHash != password)
            {
                ViewBag.Error = "Invalid email or password";
                return View();
            }

            Session["SupplierId"] = supplier.Id;
            Session["SupplierName"] = supplier.CompanyName;

            return RedirectToAction("Dashboard");
        }

        // GET: /Seller/Signup
        public ActionResult Signup()
        {
            return View();
        }

        // POST: /Seller/Signup
        [HttpPost]
        public ActionResult Signup(Suppliers model)
        {
            if (!ModelState.IsValid)
                return View(model);

            int supplierId = _repo.AddSupplier(model);

            Session["SupplierId"] = supplierId;
            Session["SupplierName"] = model.CompanyName;

            return RedirectToAction("Dashboard");
        }

        // GET: /Seller/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        public ActionResult Dashboard()
        {
            if (Session["SupplierId"] == null)
                return RedirectToAction("Login");

            int supplierId = (int)Session["SupplierId"];

            var model = _repo.GetSellerDashboard(supplierId);

            return View(model);
        }


        // GET
        public ActionResult AddProduct()
        {
            if (Session["SupplierId"] == null)
                return RedirectToAction("Login");

            var model = new SellerProductViewModel
            {
                Categories = _repo.GetCategories(),
                Warehouses = _repo.GetWarehouses()
            };

            return View(model);
        }

        // POST
        [HttpPost]
        public ActionResult AddProduct(SellerProductViewModel model)
        {
            if (Session["SupplierId"] == null)
                return RedirectToAction("Login");

            if (!ModelState.IsValid)
            {
                model.Categories = _repo.GetCategories();
                model.Warehouses = _repo.GetWarehouses();
                return View(model);
            }

            int supplierId = (int)Session["SupplierId"];

            _repo.AddProduct(model, supplierId);

            return RedirectToAction("Dashboard");
        }

        // GET
        public ActionResult Edit(int id)
        {
            if (Session["SupplierId"] == null)
                return RedirectToAction("Login");

            int supplierId = (int)Session["SupplierId"];

            var product = _repo.GetSupplierProducts(supplierId)
                               .FirstOrDefault(p => p.Id == id);

            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        // POST
        [HttpPost]
        public ActionResult Edit(Products model)
        {
            if (Session["SupplierId"] == null)
                return RedirectToAction("Login");

            int supplierId = (int)Session["SupplierId"];

            _repo.UpdateProduct(model, supplierId);

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public ActionResult UpdateInventory(int inventoryId, int quantity)
        {
            if (Session["SupplierId"] == null)
                return RedirectToAction("Login");

            int supplierId = (int)Session["SupplierId"];

            _repo.UpdateInventoryQuantity(inventoryId, quantity, supplierId);

            return RedirectToAction("Inventory");
        }
        public ActionResult Inventory()
        {
            if (Session["SupplierId"] == null)
                return RedirectToAction("Login");

            int supplierId = (int)Session["SupplierId"];
            var inventory = _repo.GetSellerInventory(supplierId);

            return View(inventory);
        }

    }
}
