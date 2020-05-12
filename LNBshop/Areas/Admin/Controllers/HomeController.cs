using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LNBshop.Areas.Admin.Controllers
{
    //cho HomeController kế thừa basecontroller để kiểm tra session
    //Và vì BaseController cũng kế thừa từ Controller nên điều này hợp lệ
    public class HomeController : BaseController
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}