using Models.DAO.Client;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace LNBshop.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index(int page = 1, int pageSize = 9)
        {
            var dao = new ProductDao();
            var model = dao.ListAllProduct(page, pageSize);
            return View(model);
        }

        public JsonResult ListName(string q)
        {
            var data = new ProductDao().ListName(q);
            return Json(new
            {
                data = data,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search(string keyword, int page = 1, int pageSize = 1)
        {
            int totalRecord = 0;
            var model = new ProductDao().Search(keyword, ref totalRecord, page, pageSize);

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;
            ViewBag.Keyword = keyword;
            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return View(model);
        }

        [ChildActionOnly]
        public ActionResult MenuCategory()
        {
            var model = new ClientCategoryDao().ListbygroupID();
            return PartialView(model);
        }

        public ActionResult productCategory(long catId, int page = 1, int pageSize = 9)
        {
            var category = new ClientCategoryDao().ViewDetail(catId);
            ViewBag.Category = category;
            int totalRecord = 0;
            var productCategory = new ProductDao().ListProductCategory(catId,ref totalRecord, page, pageSize);

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;

            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord/pageSize));
            ViewBag.totalPage = totalPage;

            ViewBag.maxPage = maxPage;
            ViewBag.first = 1;
            ViewBag.last = totalPage;
            ViewBag.next = page + 1;
            ViewBag.prev = page - 1;
            
            return View(productCategory);
        }

        [OutputCache (Duration = 3600 ,VaryByParam = "productId", Location = OutputCacheLocation.Server)]
        // Đặt OutputCache để hạn chế request server, VaryByParam để mỗi productId lưu cache 1 lần, Location để chỉ nơi lưu cache
        public ActionResult productDetail(long productId)
        {
            var product = new ProductDao().ViewDetail(productId);
            ViewBag.Category = new ClientCategoryDao().ViewDetail(product.CategoryID.Value);
            var producDao = new ProductDao();
            ViewBag.RelatedProduct = producDao.ListRelatedProduct(product.CategoryID.Value,8);
            return View(product);
        }
    }
}