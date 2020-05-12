using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LNBshop.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Bạn cần nhập Username")]
        public string UserName { set; get; }
        [Required(ErrorMessage = "Bạn cần nhập Password")]
        public string PassWord { set; get; }
        public bool RememberMe { set; get; }
    }
}