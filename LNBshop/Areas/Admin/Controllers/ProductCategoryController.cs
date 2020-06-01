using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            SetViewBag();
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
                    string stFormD = producCategory.Name.Normalize(NormalizationForm.FormD);
                    StringBuilder sb = new StringBuilder();
                    for (int ich = 0; ich < stFormD.Length; ich++)
                    {
                        System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                        if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                        {
                            sb.Append(stFormD[ich]);
                        }
                    }
                    sb = sb.Replace('Đ', 'D');
                    sb = sb.Replace('đ', 'd');
                    sb = sb.Replace(' ', '-');
                    producCategory.MetaTitle = sb.ToString().Normalize(NormalizationForm.FormD);
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
            SetViewBag();
            return View("Create");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "ProductCategory", new { area = "Admin" });
            }
            var productCategory = new ProductCategoryDao().ViewDetail(id);
            SetViewBag(productCategory.ParentID);
            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductCategoryDao();
                string stFormD = productCategory.Name.Normalize(NormalizationForm.FormD);
                StringBuilder sb = new StringBuilder();
                for (int ich = 0; ich < stFormD.Length; ich++)
                {
                    System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                    if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                    {
                        sb.Append(stFormD[ich]);
                    }
                }
                sb = sb.Replace('Đ', 'D');
                sb = sb.Replace('đ', 'd');
                sb = sb.Replace(' ', '-');
                productCategory.MetaTitle = sb.ToString().Normalize(NormalizationForm.FormD);
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

        public void SetViewBag(long? selectedId = null)
        {
            var dao = new ProductCategoryDao();
            ViewBag.ParentID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }
    }
}