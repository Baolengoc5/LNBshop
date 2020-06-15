using BotDetect.Web.Mvc;
using Common;
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
        // GET: User
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [CaptchaValidation("CaptchaCode", "registerCapcha", "Mã xác nhận không đúng!")]
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
                    user.Phone = model.Phone;
                    user.Email = model.Email;
                    user.Address = model.Address;
                    user.CreatedDate = DateTime.Now;
                    user.Status = false;
                    user.GroupID = "MEMBER";
                    user.CodeConfirmEmail = cCodeConfirmEmail;


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
                        string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/client/template/NewAccount.html"));

                        content = content.Replace("{{CustomerName}}", model.Name);
                        content = content.Replace("{{UserName}}", model.UserName);
                        content = content.Replace("{{CallbackUrl}}", CallbackUrl);
                        var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                        new MailHelper().SendMail(email, "Thư xác nhận tài khoản từ LNBshop", content);
                        new MailHelper().SendMail(toEmail, "Thư xác nhận tài khoản từ LNBshop", content);
                        
                        ViewBag.Success = "Đăng ký thành công! hãy xác nhận thư trong hộp thư Email để mở khóa tài khoản của bạn";
                        model = new RegisterModel();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký không thành công.");
                    }
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
    }
}