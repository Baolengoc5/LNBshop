using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LNBshop.Areas.Admin.Controllers
{
    public class ContentController : Controller
    {
        // GET: Admin/Content
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase Image,Content content)
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

                        if (Image != null)
                        {
                            //, Path.GetFileName(Image.FileName)
                            string path = Path.Combine(Server.MapPath("~/Data/Image/Content/"));
                            string strExtexsion = Path.GetExtension(Path.GetFileName(Image.FileName)).Trim();
                            content.CreatedDate = DateTime.Now;
                            

                            long id = dao.Insert(content);
                            content.Image = "/Data/Image/Content/" + content.ID + strExtexsion;
                            dao.Update(content);
                            if (id > 0)
                            {
                                Image.SaveAs(path + content.ID + strExtexsion);

                                return RedirectToAction("Index", "Content");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Thêm tin tức không thành công");
                            }
                            
                        }
                        
                    }
                    catch (Exception)
                    {

                        ViewBag.FileStatus = "Error while file uploading.";
                    }
                }
            }
            return View("Create");
        }
    }
}