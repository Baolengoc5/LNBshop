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
        //public bool Update(Content entity)
        //{
            //try
            //{
            //    var content = db.Contents.Find(entity.ID);//tìm ID

            //    content.Name = entity.Name;
            //    content.MetaTitle = entity.MetaTitle;
            //    content.Description = entity.Description;
            //    content.Image =
            //    user.ModifiedBy = entity.ModifiedBy;
            //    user.ModifiedDate = DateTime.Now;
            //    user.Status = entity.Status;
            //    db.SaveChanges();
            //    return true;
            //}
            //catch (Exception ex)//xử lý các trường hợp ngoại lệ
            //{
            //    return false;
            //}
        //}



            public bool UpdateImage(Content entityImage)
            {
                try
                {
                    var content = db.Contents.Find(entityImage.ID);//tìm ID
                    content.Image = entityImage.Image;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)//xử lý các trường hợp ngoại lệ
                {
                    return false;
                }
            }

            //Hiển thị danh sách
            public IEnumerable<Content> ListAllPaging(int page, int pageSize)
            {
                //OrderByDescending(x=>x.CreatedDate) là sắp xếp theo ngày tạo
                return db.Contents.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
            }



            public int CountContentName(string contentName)//Kiểm tra tên đăng nhập có bị trùng lặp không
            {
                return db.Contents.Count(x => x.Name == contentName);
            }

            public Content GetByID(long id)//để lấy thông tin tin tức thông qua id 
            {
                return db.Contents.Find(id);
                //SingleOrDefault : lấy một bảng ghi đơn thông qua id truyền vào
            }
        }
    }
