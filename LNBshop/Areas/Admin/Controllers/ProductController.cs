using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace LNBshop.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        // GET: Admin/Product
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ProductDao();
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
        [ValidateInput(false)]
        public ActionResult Create(HttpPostedFileBase Image, Product product)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductDao();
                if (dao.CountProductName(product.Name) > 0)
                {
                    ModelState.AddModelError("", "Sản phẩm này đã có !");
                }
                else
                {
                    try
                    {
                        string stFormD = product.Name.Normalize(NormalizationForm.FormD);
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
                        sb = sb.Replace(' ','-');
                        product.MetaTitle = sb.ToString().Normalize(NormalizationForm.FormD);

                        if (Image != null)
                        {
                            //Lấy đuôi file để kiểm tra chỉ lấy hình ảnh
                            string extension = Path.GetExtension(Image.FileName);
                            //, Path.GetFileName(Image.FileName)
                            string path = Path.Combine(Server.MapPath("~/Data/Image/Product/"));
                            string strExtexsion = Path.GetExtension(Path.GetFileName(Image.FileName)).Trim();
                            product.CreatedDate = DateTime.Now;
                            product.Status = true;
                            //Kiểm tra đuôi file ảnh
                            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                            {
                                if (Image.ContentLength <= 500000)// kiểm tra kích thước file nhỏ hơn hoặc bằng 1mb
                                {
                                    long id = dao.Insert(product);
                                    product.Image = "/Data/Image/Product/" + product.ID + strExtexsion;
                                    dao.UpdateImage(product);
                                    if (id > 0)
                                    {
                                        Image.SaveAs(path + product.ID + strExtexsion);
                                        ModelState.Clear();

                                        return RedirectToAction("Index", "Product");
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("", "Thêm tin tức không thành công");
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError("", "Kích thước file phải nhỏ hơn hoặc bằng 5mb");
                                }

                            }
                            else
                            {
                                ModelState.AddModelError("", "Bạn phải chọn hình ảnh, các đuôi .jpg/.jpeg/.png");
                            }


                        }

                    }
                    catch (Exception)
                    {

                        ViewBag.FileStatus = "Error while file uploading.";
                    }
                }
            }
            SetViewBag();
            return View("Create");
        }

        [HttpGet]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Product", new { area = "Admin" });
            }
            var dao = new ProductDao();
            var product = dao.GetByID(id);

            Session["topHot"] = product.TopHot;
            Session["imgPath"] = product.Image;
            SetViewBag(product.CategoryID);

            return View(product);
        }

        [HttpPost]
        [ValidateInput(false)]

        public ActionResult Edit(HttpPostedFileBase Image, Product product, DateTime TopHot)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductDao();

                try
                {
                    string stFormD = product.Name.Normalize(NormalizationForm.FormD);
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
                    product.MetaTitle = sb.ToString().Normalize(NormalizationForm.FormD);
                    if (TopHot != null)
                    {
                        product.TopHot = TopHot;
                    }
                    if (Image != null)
                    {
                        //Lấy đuôi file để kiểm tra chỉ lấy hình ảnh
                        string extension = Path.GetExtension(Image.FileName);
                        //, Path.GetFileName(Image.FileName)
                        string path = Path.Combine(Server.MapPath("~/Data/Image/Product/"));
                        string strExtexsion = Path.GetExtension(Path.GetFileName(Image.FileName)).Trim();
                        product.CreatedDate = DateTime.Now;
                        //Kiểm tra đuôi file ảnh
                        if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                        {
                            if (Image.ContentLength <= 500000)// kiểm tra kích thước file nhỏ hơn hoặc bằng 1mb
                            {
                                string oldImagePath = Request.MapPath(Session["imgPath"].ToString());
                                var result = dao.Update(product);
                                product.Image = "/Data/Image/Product/" + product.ID + strExtexsion;
                                dao.UpdateImage(product);
                                if (result)
                                {
                                    if (System.IO.File.Exists(oldImagePath))
                                    {
                                        System.IO.File.Delete(oldImagePath);
                                    }
                                    Image.SaveAs(path + product.ID + strExtexsion);

                                    ModelState.Clear();

                                    return RedirectToAction("Index", "Product");
                                }
                                else
                                {
                                    ModelState.AddModelError("", "Thêm tin tức không thành công");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "Kích thước file phải nhỏ hơn hoặc bằng 5mb");
                            }

                        }
                        else
                        {
                            ModelState.AddModelError("", "Bạn phải chọn hình ảnh, các đuôi .jpg/.jpeg/.png");
                        }


                    }
                    else
                    {
                        product.Image = Session["imgPath"].ToString();
                        var result = dao.Update(product);
                        return RedirectToAction("Index", "Product");
                    }

                }
                catch (Exception)
                {

                    ViewBag.FileStatus = "Error while file uploading.";
                }

            }
            SetViewBag();
            return View("Edit");
        }

        //xóa sản phẩm
        [HttpDelete]
        public ActionResult Delete(int id, Product product)
        {
            new ProductDao().Detele(id);

            string currentImg = Request.MapPath(product.Image);
            if (System.IO.File.Exists(currentImg))
            {
                System.IO.File.Delete(currentImg);
            }
            return RedirectToAction("Index", "Product");
        }

        public void SetViewBag(long? selectedId = null)
        {
            var dao = new ProductCategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }

        //change status

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new ProductDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}