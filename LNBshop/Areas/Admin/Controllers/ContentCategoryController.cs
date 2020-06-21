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
    public class ContentCategoryController : BaseController
    {
        // GET: Admin/ContentCategory
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new CategoryDao();
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
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                var dao = new CategoryDao();
                if (dao.CountName(category.Name) > 0)
                {
                    ModelState.AddModelError("", "đã có danh mục này !");
                }
                else
                {
                    string stFormD = category.Name.Normalize(NormalizationForm.FormD);
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
                    category.MetaTitle = sb.ToString().Normalize(NormalizationForm.FormD);
                    category.CreatedDate = DateTime.Now;
                    category.Status = true;

                    long id = dao.Insert(category);

                    if (id > 0)
                    {
                        return RedirectToAction("Index", "ContentCategory");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm danh mục không thành công");
                    }
                }

            }
            return View("Create");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "ContentCategory", new { area = "Admin" });
            }
            var category = new CategoryDao().ViewDetail(id);
            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(Category category)
        {

            if (ModelState.IsValid)
            {
                var dao = new CategoryDao();
                string stFormD = category.Name.Normalize(NormalizationForm.FormD);
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
                category.MetaTitle = sb.ToString().Normalize(NormalizationForm.FormD);
                var result = dao.Update(category);
                if (result)
                {
                    return RedirectToAction("Index", "ContentCategory");
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
            var result = new CategoryDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        //xóa danh mục tin tức
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new CategoryDao().Detele(id);
            return RedirectToAction("Index");
        }
    }
}