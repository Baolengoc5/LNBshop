using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LNBshop.Common;
using Models.DAO;
using Models.EF;
using PagedList;

namespace LNBshop.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        // GET: Admin/User
        public ActionResult Index(string searchString, int page = 1, int pageSize = 4)
        {
            var dao = new UserDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);

            ViewBag.SearchString = searchString;

            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Edit(int id)
        {
            var user = new UserDao().ViewDetail(id);
            return View(user);
        }
        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CountUserName(user.UserName) > 0)
                {
                    ModelState.AddModelError("", "đã có người sử dụng tên đăng nhập này !");
                }
                else
                {
                    var EncryptedMD5Pas = Encryptor.MD5Hash(user.Password);//Mã hóa password người dùng nhập vào
                    user.Password = EncryptedMD5Pas;
                    //có thể thêm một số dữ liệu không cho người dùng nhập trong View Create ở đây
                    user.CreatedDate = DateTime.Now;

                    long id = dao.Insert(user);

                    if (id > 0)
                    {
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm user không thành công");
                    }
                }
                
            }
            return View("Create");

        }
        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (!string.IsNullOrEmpty(user.Password))//kiểm tra nếu người dùng nhập password mới thực thi
                {
                    var EncryptedMD5Pas = Encryptor.MD5Hash(user.Password);//Mã hóa password người dùng nhập vào
                    user.Password = EncryptedMD5Pas;
                }

                var result = dao.Update(user);
                if (result)
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "cập nhật không thành công");
                }
            }
            return View("Edit");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new UserDao().Detele(id);
            return RedirectToAction("Index");
        }
    }
}