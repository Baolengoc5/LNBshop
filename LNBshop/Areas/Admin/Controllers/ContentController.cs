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
using System.Web;
using System.Web.Mvc;

namespace LNBshop.Areas.Admin.Controllers
{
    public class ContentController : BaseController
    {
        // GET: Admin/Content

        public ActionResult Index(string searchString, int page = 1, int pageSize = 4)
        {
            var dao = new ContentDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);

            ViewBag.SearchString = searchString;

            return View(model);
        }

        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HttpGet]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Content", new { area = "Admin" });
            }
            var dao = new ContentDao();
            var content = dao.GetByID(id);

            Session["imgPath"] = content.Image;
            SetViewBag(content.CategoryID);

            return View(content);
        }

        [HttpPost]
        [ValidateInput(false)]

        public ActionResult Edit(HttpPostedFileBase Image, Content content)
        {
            if (ModelState.IsValid)
            {
                var dao = new ContentDao();

                try
                {
                    string stFormD = content.Name.Normalize(NormalizationForm.FormD);
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
                    content.MetaTitle = sb.ToString().Normalize(NormalizationForm.FormD);
                    if (Image != null)
                    {
                        //Lấy đuôi file để kiểm tra chỉ lấy hình ảnh
                        string extension = Path.GetExtension(Image.FileName);
                        //, Path.GetFileName(Image.FileName)
                        string path = Path.Combine(Server.MapPath("~/Data/Image/Content/"));
                        string strExtexsion = Path.GetExtension(Path.GetFileName(Image.FileName)).Trim();
                        content.CreatedDate = DateTime.Now;
                        content.Status = true;
                        //Kiểm tra đuôi file ảnh
                        if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                        {
                            if (Image.ContentLength <= 500000)// kiểm tra kích thước file nhỏ hơn hoặc bằng 1mb
                            {
                                string oldImagePath = Request.MapPath(Session["imgPath"].ToString());
                                var result = dao.Update(content);
                                content.Image = "/Data/Image/Content/" + content.ID + strExtexsion;
                                dao.UpdateImage(content);
                                if (result)
                                {
                                    if (System.IO.File.Exists(oldImagePath))
                                    {
                                        System.IO.File.Delete(oldImagePath);
                                    }
                                    Image.SaveAs(path + content.ID + strExtexsion);
                                    
                                    ModelState.Clear();

                                    return RedirectToAction("Index", "Content");
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
                        content.Image = Session["imgPath"].ToString();
                        var result = dao.Update(content);
                        return RedirectToAction("Index", "Content");
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

        [HttpDelete]
        public ActionResult Delete(int id, Content content)
        {
            new ContentDao().Detele(id);

                string currentImg = Request.MapPath(content.Image);
                if (System.IO.File.Exists(currentImg))
                {
                    System.IO.File.Delete(currentImg);
                }
                return RedirectToAction("Index", "Content");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(HttpPostedFileBase Image, Content content)
        {
            if (ModelState.IsValid)
            {
                var dao = new ContentDao();
                if (dao.CountContentName(content.Name) > 0)
                {
                    ModelState.AddModelError("", "Tiêu đề tin tức này đã có người sử dụng !");
                }
                else
                {
                    try
                    {
                        string stFormD = content.Name.Normalize(NormalizationForm.FormD);
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
                        content.MetaTitle = sb.ToString().Normalize(NormalizationForm.FormD);
                        if (Image != null)
                        {
                            //Lấy đuôi file để kiểm tra chỉ lấy hình ảnh
                            string extension = Path.GetExtension(Image.FileName);
                            //, Path.GetFileName(Image.FileName)
                            string path = Path.Combine(Server.MapPath("~/Data/Image/Content/"));
                            string strExtexsion = Path.GetExtension(Path.GetFileName(Image.FileName)).Trim();
                            content.CreatedDate = DateTime.Now;
                            content.Status = true;
                            //Kiểm tra đuôi file ảnh
                            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                            {
                                if (Image.ContentLength <= 500000)// kiểm tra kích thước file nhỏ hơn hoặc bằng 1mb
                                {
                                    long id = dao.Insert(content);
                                    content.Image = "/Data/Image/Content/" + content.ID + strExtexsion;
                                    dao.UpdateImage(content);
                                    if (id > 0)
                                    {
                                        Image.SaveAs(path + content.ID + strExtexsion);
                                        ModelState.Clear();

                                        return RedirectToAction("Index", "Content");
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

        public void SetViewBag(long? selectedid = null)
        {
            var dao = new CategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name", selectedid);
        }

        //change status

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new ContentDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}