using Models.DAO.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LNBshop.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            ViewBag.Slides = new SlideDao().ListAll();
            ViewBag.NewContent = new ContentDao().ListNewContent(3);
            var producDao = new ProductDao();
            ViewBag.NewProduct = producDao.ListNewProduct(8);
            ViewBag.HotProduct = producDao.ListHotProduct(6);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [ChildActionOnly]
        public ActionResult TopMenu()
        {
            var model = new ClientMenuDao().ListbygroupID(2);
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult MainMenu()
        {
            var model = new ClientMenuDao().ListbygroupID(1);
            return PartialView(model);
        }
    }
}