using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class ContentDao //set public để project khác có thể gọi tới
    {
        LNBshopDbContext db = null;// khai báo biến
        public ContentDao()//tạo một constructor để tạo mới db
        {
            db = new LNBshopDbContext();
        }

        //Thêm
        public long Insert(Content entity)//tạo hàm chức năng Insert kiểu dữ liệu long vì trả về ID kiểu bigint
        {
            db.Contents.Add(entity);//phương thức thêm trong entity
            db.SaveChanges();//Lưu thay đổi trong database
            return entity.ID;
        }

        public int CountContentName(string contentName)//Kiểm tra tên đăng nhập có bị trùng lặp không
        {
            return db.Contents.Count(x => x.Name == contentName);
        }

        public bool Update(Content entity)
        {
            try
            {
                var content = db.Contents.Find(entity.ID);//tìm ID
                content.Image = entity.Image;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)//xử lý các trường hợp ngoại lệ
            {
                return false;
            }

        }
    }
}
