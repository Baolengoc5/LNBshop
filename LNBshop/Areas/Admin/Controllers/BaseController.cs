using LNBshop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Common;

namespace LNBshop.Areas.Admin.Controllers
{
    public class BaseController : Controller//Đây là Controller kiểm tra session
    {
        //Ghi đè phương thức OnActionExecuting vào controller
        //Tạm biết đây là một bộ lọc chuyển hướng
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //khai báo biến session và gán cho key Session USER_SESSION ép kiểu UserLogin
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];

            //Kiểm tra bộ lọc chuyển hướng
            if (session == null)//kiểm tra nếu không có session tức chưa đăng nhập sẽ chuyển sang trang Login
            {
                
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { Controller = "Login", Action = "Index", Areas = "Admin" })
                    );
            }
            else
            {
                if (session.GroupID == CommonConstantss.MEMBER_GROUP)
                {
                    Session[CommonConstants.USER_SESSION] = null;
                    filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { Controller = "Login", Action = "Index", Areas = "Admin" })
                    );
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}