using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LNBshop.Common
{
    [Serializable] //"trình tự hóa dữ liệu" từ object về dạng trung gian để lưu trữ (ở đây là session)
    //phải có mới lưu thành session được
    public class UserLogin
    {
        //2 trường để lưu session
        public long UserID { set; get; }
        public string UserName { set; get; }
    }
}