using Models.DAO.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LNBshop.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult MenuCategory()
        {
            var model = new ClientCategoryDao().ListbygroupID(true);
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult MenuCategoryHiden()
        {
            var model = new ClientCategoryDao().ListbygroupID(false);
            return PartialView(model);
        }

        public ActionResult productCategory(long catId)
        {
            var productCategory = new ProductDao().ListProductCategory(catId);
            return View(productCategory);
        }
    }
}