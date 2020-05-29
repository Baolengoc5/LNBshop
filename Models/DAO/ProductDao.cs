using Models.EF;
using Models.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class ProductDao
    {
        LNBshopDbContext db = null;
        public ProductDao()
        {
            db = new LNBshopDbContext();
        }

        public long Insert(Product entity)//tạo hàm chức năng Insert kiểu dữ liệu long vì trả về ID kiểu bigint
        {
            db.Products.Add(entity);//phương thức thêm trong entity
            db.SaveChanges();//Lưu thay đổi trong database
            return entity.ID;
        }

        //Sửa
        public bool Update(Product entity)
        {
            try
            {
                var product = db.Products.Find(entity.ID);//tìm ID
                if (!string.IsNullOrEmpty(entity.Detail))//kiểm tra nếu người dùng nhập Detail mới thực thi
                {
                    product.Detail = entity.Detail;
                }
                if (entity.PromotionPrice != null)
                {
                    product.PromotionPrice = entity.PromotionPrice;
                }
                if (entity.TopHot != null)
                {
                    product.TopHot = entity.TopHot;
                }

                product.Name = entity.Name;
                product.MetaTitle = entity.MetaTitle;
                product.Description = entity.Description;
                product.Price = entity.Price;
                product.Quantity = entity.Quantity;
                product.CategoryID = entity.CategoryID;
                product.Warranty = entity.Warranty;

                product.ModifiedBy = entity.ModifiedBy;
                product.ModifiedDate = DateTime.Now;
                product.MetaKeywords = entity.MetaKeywords;
                product.MetaDescriptions = entity.MetaDescriptions;
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
                var product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateImage(Product entityImage)
        {
            try
            {
                var content = db.Products.Find(entityImage.ID);//tìm ID
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
        public IEnumerable<Product> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Product> model = db.Products;
            if (!string.IsNullOrEmpty(searchString))//nếu searchString khác null
            {
                model = model.Where(x => x.Name.Contains(searchString));
                //OrderByDescending(x=>x.CreatedDate) là sắp xếp theo ngày tạo
                //.Contains(searchString) là tìm kiếm gần giống
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }



        public int CountProductName(string productName)//Kiểm tra tên đăng nhập có bị trùng lặp không
        {
            return db.Contents.Count(x => x.Name == productName);
        }

        public Product GetByID(long? id)//để lấy thông tin tin tức thông qua id 
        {
            return db.Products.Find(id);
            //SingleOrDefault : lấy một bảng ghi đơn thông qua id truyền vào
        }

        public bool ChangeStatus(long id)
        {
            var product = db.Products.Find(id);
            product.Status = !product.Status;
            db.SaveChanges();
            return product.Status;
        }
    }
}
