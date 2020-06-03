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
        public ActionResult Index(int page = 1, int pageSize = 6)
        {
            int totalRecord = 0;
            var allProduct = new ProductDao().ListAllProduct(ref totalRecord, page, pageSize);

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;

            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            ViewBag.totalPage = totalPage;

            ViewBag.maxPage = maxPage;
            ViewBag.first = 1;
            ViewBag.last = totalPage;
            ViewBag.next = page + 1;
            ViewBag.prev = page - 1;

            return View(allProduct);
        }

        [ChildActionOnly]
        public ActionResult MenuCategory()
        {
            var model = new ClientCategoryDao().ListbygroupID();
            return PartialView(model);
        }

        public ActionResult productCategory(long catId, int page = 1, int pageSize = 8)
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