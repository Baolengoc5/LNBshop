using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class ProductCategoryDao
    {
        LNBshopDbContext db = null;
        public ProductCategoryDao()
        {
            db = new LNBshopDbContext();
        }

        public IEnumerable<ProductCategory> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<ProductCategory> model = db.ProductCategories;
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
            var productCategory = db.ProductCategories.Find(id);
            productCategory.Status = !productCategory.Status;
            db.SaveChanges();
            return productCategory.Status;
        }

        //Thêm
        public long Insert(ProductCategory entity)//tạo hàm chức năng Insert kiểu dữ liệu long vì trả về ID kiểu bigint
        {
            db.ProductCategories.Add(entity);//phương thức thêm trong entity
            db.SaveChanges();//Lưu thay đổi trong database
            return entity.ID;
        }

        //Sửa
        public bool Update(ProductCategory entity)
        {
            try
            {
                var productCategory = db.ProductCategories.Find(entity.ID);//tìm ID

                if (entity.ParentID != null)
                {
                    productCategory.ParentID = entity.ParentID;
                }

                productCategory.Name = entity.Name;
                productCategory.MetaTitle = entity.MetaTitle;
                productCategory.SeoTitle = entity.SeoTitle;
                productCategory.MetaKeywords = entity.MetaKeywords;
                productCategory.MetaDescriptions = entity.MetaDescriptions;
                productCategory.ShowOnHome = entity.ShowOnHome;
                productCategory.DisplayOrder = entity.DisplayOrder;
                productCategory.ModifiedBy = entity.ModifiedBy;
                productCategory.ModifiedDate = DateTime.Now;

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
                var productCategory = db.ProductCategories.Find(id);
                db.ProductCategories.Remove(productCategory);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<ProductCategory> ListAll()
        {
            return db.ProductCategories.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }

        public ProductCategory ViewDetail(long? id)
        {
            return db.ProductCategories.Find(id);
        }

        public int CountName(string Name)//Kiểm tra tên đăng nhập có bị trùng lặp không
        {
            return db.ProductCategories.Count(x => x.Name == Name);
        }
    }
}
