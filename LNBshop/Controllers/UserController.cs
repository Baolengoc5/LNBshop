using BotDetect.Web.Mvc;
using LNBshop.Common;
using LNBshop.Models;
using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LNBshop.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [CaptchaValidation("CaptchaCode", "registerCapcha", "Mã xác nhận không đúng!")]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CheckUserName(model.UserName))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                }
                else if (dao.CheckEmail(model.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    var user = new User();
                    user.UserName = model.UserName;
                    user.Name = model.Name;
                    user.Password = Encryptor.MD5Hash(model.Password);
                    user.Phone = model.Phone;
                    user.Email = model.Email;
                    user.Address = model.Address;
                    user.CreatedDate = DateTime.Now;
                    user.Status = false;
                    user.GroupID = "MEMBER";
                    //if (!string.IsNullOrEmpty(model.ProvinceID))
                    //{
                    //    user.ProvinceID = int.Parse(model.ProvinceID);
                    //}
                    //if (!string.IsNullOrEmpty(model.DistrictID))
                    //{
                    //    user.DistrictID = int.Parse(model.DistrictID);
                    //}

                    var result = dao.Insert(user);
                    if (result > 0)
                    {
                        ViewBag.Success = "Đăng ký thành công";
                        model = new RegisterModel();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký không thành công.");
                    }
                }
            }
            return View(model);
        }
    }
}