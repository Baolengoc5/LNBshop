using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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

        public ActionResult Index(int page = 1, int pageSize = 4)
        {
            var dao = new ContentDao();
            var model = dao.ListAllPaging(page, pageSize);
            return View(model);
        }
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        public ActionResult Edit( long id)
        {
            var dao = new ContentDao();
            var content = dao.GetByID(id);
            SetViewBag(content.CategoryID);
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(HttpPostedFileBase Image, Content content)
        {
            if (ModelState.IsValid)
            {
                
            }
            SetViewBag(content.CategoryID);
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
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
                            content.Status = true;

                            long id = dao.Insert(content);
                            content.Image = "/Data/Image/Content/" + content.ID + strExtexsion;
                            dao.UpdateImage(content);
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
            SetViewBag();
            return View("Create");
        }
        
        public void SetViewBag(long? selectedid = null)
        {
            var dao = new CategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(),"ID","Name",selectedid);
        }
    }
}