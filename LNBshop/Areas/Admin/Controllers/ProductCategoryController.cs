using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LNBshop.Areas.Admin.Controllers
{
    public class ProductCategoryController : Controller
    {
        // GET: Admin/ProductCategory
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ProductCategoryDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);

            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductCategory producCategory)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductCategoryDao();
                if (dao.CountName(producCategory.Name) > 0)
                {
                    ModelState.AddModelError("", "Đã có danh mục này !");
                }
                else
                {
                    producCategory.CreatedDate = DateTime.Now;
                    producCategory.Status = true;

                    long id = dao.Insert(producCategory);

                    if (id > 0)
                    {
                        return RedirectToAction("Index", "ProductCategory");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm danh mục không thành công");
                    }
                }

            }
            return View("Create");
        }

        public ActionResult Edit(int id)
        {
            var productCategory = new ProductCategoryDao().ViewDetail(id);
            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductCategoryDao();

                var result = dao.Update(productCategory);
                if (result)
                {
                    return RedirectToAction("Index", "ProductCategory");
                }
                else
                {
                    ModelState.AddModelError("", "cập nhật không thành công");
                }
            }
            return View("Edit");
        }

        //change status

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new ProductCategoryDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        //xóa danh mục sản phẩm
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new ProductCategoryDao().Detele(id);
            return RedirectToAction("Index");
        }
    }
}