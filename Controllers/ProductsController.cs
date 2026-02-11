using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SupplyChain.Data;

namespace SupplyChain.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductRepository _repo = new ProductRepository();
        public ActionResult Index()
        {
            var products = _repo.GetAllProducts();
            return View(products);

        }
        public ActionResult Details(int id)
        {
            var detail = _repo.GetProductsbyId(id);
            if (detail == null)
            {
                return HttpNotFound();
            }
            return PartialView("_details",detail);
        }
        
    }
}