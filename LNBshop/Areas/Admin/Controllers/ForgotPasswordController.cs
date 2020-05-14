using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LNBshop.Areas.Admin.Controllers
{
    public class ForgotPasswordController : Controller
    {
        // GET: Admin/ForgotPassword
        public ActionResult Index()
        {
            return View();
        }
    }
}