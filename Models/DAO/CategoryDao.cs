using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class CategoryDao
    {
        LNBshopDbContext db = null;// khai báo biến
        public CategoryDao()//tạo một constructor để tạo mới db
        {
            db = new LNBshopDbContext();
        }

        public IEnumerable<Category> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Category> model = db.Categories;
            if (!string.IsNullOrEmpty(searchString))//nếu searchString khác null
            {
                model = model.Where(x => x.Name.Contains(searchString));
                //OrderByDescending(x=>x.CreatedDate) là sắp xếp theo ngày tạo
                //.Contains(searchString) là tìm kiếm gần giống
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public bool ChangeStatus(long id)
        {
            var Category = db.Categories.Find(id);
            Category.Status = !Category.Status;
            db.SaveChanges();
            return Category.Status;
        }

        //Thêm
        public long Insert(Category entity)//tạo hàm chức năng Insert kiểu dữ liệu long vì trả về ID kiểu bigint
        {
            db.Categories.Add(entity);//phương thức thêm trong entity
            db.SaveChanges();//Lưu thay đổi trong database
            return entity.ID;
        }

        //Sửa
        public bool Update(Category entity)
        {
            try
            {
                var category = db.Categories.Find(entity.ID);//tìm ID

                category.Name = entity.Name;
                category.MetaTitle = entity.MetaTitle;
                category.ParentID = entity.ParentID;
                category.MetaKeywords = entity.MetaKeywords;
                category.MetaDescriptions = entity.MetaDescriptions;
                category.ShowOnHome = entity.ShowOnHome;
                category.DisplayOrder = entity.DisplayOrder;
                category.Language = entity.Language;
                category.ModifiedBy = entity.ModifiedBy;
                category.ModifiedDate = DateTime.Now;

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
                var category = db.Categories.Find(id);
                db.Categories.Remove(category);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Category ViewDetail(long? id)
        {
            return db.Categories.Find(id);
        }

        public int CountName(string Name)//Kiểm tra tên đăng nhập có bị trùng lặp không
        {
            return db.Categories.Count(x => x.Name == Name);
        }

        public List<Category> ListAll()
        {
            return db.Categories.Where(x => x.Status == true).ToList();
        }
    }
}
