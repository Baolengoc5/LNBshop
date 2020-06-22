using BotDetect.Web.Mvc;
using Common;
using Facebook;
using LNBshop.Common;
using LNBshop.Models;
using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LNBshop.Controllers
{
    public class UserController : Controller
    {
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        // GET: User
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });


            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                // Get the user's information, like email, first name, middle name etc
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string userName = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;

                var user = new User();
                user.Email = email;
                user.UserName = userName;
                user.Status = true;
                user.Name = firstname + " " + middlename + " " + lastname;
                user.CreatedDate = DateTime.Now;
                var resultInsert = new UserDao().InsertForFacebook(user);
                if (resultInsert > 0)
                {
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                }
            }
            return Redirect("/");
        }

        [HttpPost]
        [CaptchaValidation("CaptchaCode", "registerCaptcha", "Mã xác nhận không đúng!")]
        public ActionResult Register(RegisterModel model, string email)
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
                    Random rnd = new Random();
                    string code = Convert.ToString(rnd.Next(0,9999));
                    string a = Convert.ToString((char)rnd.Next(97, 122));//97-122 là mã từ a-z
                    string cCodeConfirmEmail = a + code;
                    var CallbackUrl = Url.Action("ConfirmEmail","User",new{ userName = model.UserName,codeConfirmEmail = cCodeConfirmEmail },protocol: Request.Url.Scheme);

                    var user = new User();
                    user.UserName = model.UserName;
                    user.Name = model.Name;
                    user.Password = Encryptor.MD5Hash(model.Password);
                    user.Email = model.Email;
                    user.CreatedDate = DateTime.Now;
                    user.Status = false;
                    user.GroupID = "MEMBER";
                    user.CodeConfirmEmail = cCodeConfirmEmail;


                    var result = dao.Insert(user);
                    
                    if (result > 0)
                    {
                        string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/client/template/NewAccount.html"));

                        content = content.Replace("{{CustomerName}}", model.Name);
                        content = content.Replace("{{UserName}}", model.UserName);
                        content = content.Replace("{{CallbackUrl}}", CallbackUrl);
                        var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                        new MailHelper().SendMail(email, "Thư xác nhận tài khoản từ LNBshop", content);
                        new MailHelper().SendMail(toEmail, "Thư xác nhận tài khoản từ LNBshop", content);
                        
                        ViewBag.Success = "Đăng ký thành công! hãy xác nhận thư trong hộp thư Email để mở khóa tài khoản của bạn";
                        model = new RegisterModel();
                        MvcCaptcha.ResetCaptcha("registerCaptcha");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký không thành công.");
                        MvcCaptcha.ResetCaptcha("registerCaptcha");
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)//Kiểm tra form có nhập đầy đủ chưa
            {
                var dao = new UserDao();// tạo một biến để dùng object UserDao
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password));//mã hóa bằng MD5
                if (result == 1 || result == 3)//kiểm tra username password nếu true
                {
                    var User = dao.GetByUserName(model.UserName);//tạo biến lấy dữ liệu từ GetByUserName bên UserDao
                    var userSession = new UserLogin();
                    //Lưu username với ID vào biến được tạo
                    userSession.UserName = User.UserName;
                    userSession.UserID = User.ID;

                    //Lưu SESSION
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    //Chuyển trang tới nơi yêu cầu
                    return Redirect("/");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại !");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản này đang bị khóa !");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng !");
                }
            }
            return View(model);
        }

        public ActionResult ConfirmEmail(User user,string userName, string codeConfirmEmail)
        {
            user.UserName = userName;
            user.CodeConfirmEmail = codeConfirmEmail;
            var dao = new UserDao();
            var result = dao.ConfirmEmailAccount(user);
            if (result)
            {
                ViewBag.Success = "Xác nhận tài khoản thành công";
            }
            else
            {
                ViewBag.Error = "Liên kết không còn khả dụng";
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return Redirect("/");
        }
    }
}