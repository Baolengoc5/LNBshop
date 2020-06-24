using LNBshop.Areas.Admin.Models;
using LNBshop.Common;
using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LNBshop.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if(ModelState.IsValid)//Kiểm tra form có nhập đầy đủ chưa
            {
                var dao = new UserDao();// tạo một biến để dùng object UserDao
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.PassWord));//mã hóa bằng MD5
                if (result == 3)//kiểm tra username password nếu true
                {
                    var User = dao.GetByUserName(model.UserName);//tạo biến lấy dữ liệu từ GetByUserName bên UserDao
                    var userSession = new UserLogin();
                    //Lưu username với ID vào biến được tạo
                    userSession.UserName = User.UserName;
                    userSession.UserID = User.ID;
                    userSession.GroupID = User.GroupID;


                    var listCredentials = dao.GetListCredential(model.UserName);

                    //Lưu SESSION
                    Session.Add(CommonConstants.SESSION_CREDENTIALS, listCredentials);
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    //Chuyển trang tới nơi yêu cầu
                    return RedirectToAction("Index","Home");
                }
                else if (result == 1)
                {
                    ModelState.AddModelError("", "Bạn không đủ quyền hạn để truy cập khu vực này !");
                }else if(result == 0)
                {
                    ModelState.AddModelError("","Tài khoản không tồn tại !");
                }else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản này đang bị khóa !");
                }else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng !");
                }
            }
            //Còn lại không làm gì cả
            return View("Index");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "Login", new { area = "Admin" });
        }
    }
}