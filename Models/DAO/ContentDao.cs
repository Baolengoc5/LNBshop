using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        //Sửa
        public bool Update(Content entity)
        {
            try
            {
                var content = db.Contents.Find(entity.ID);//tìm ID
                if (!string.IsNullOrEmpty(entity.Detail))//kiểm tra nếu người dùng nhập Detail mới thực thi
                {
                    content.Detail = entity.Detail;
                }

                content.Name = entity.Name;
                content.MetaTitle = entity.MetaTitle;
                content.Description = entity.Description;
                content.CategoryID = entity.CategoryID;

                content.Warranty = entity.Warranty;
                content.ModifiedBy = entity.ModifiedBy;
                content.ModifiedDate = DateTime.Now;
                content.MetaKeywords = entity.MetaKeywords;
                content.MetaDescriptions = entity.MetaDescriptions;

                content.Status = entity.Status;

                content.Tags = entity.Tags;
                content.Language = entity.Language;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)//xử lý các trường hợp ngoại lệ
            {
                return false;
            }
        }

        //Xóa
        public bool Detele(int id)
        {
            try
            {
                var content = db.Contents.Find(id);
                db.Contents.Remove(content);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

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
