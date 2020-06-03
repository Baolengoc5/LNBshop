using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO.Client
{
    public class ProductDao
    {
        LNBshopDbContext db = null;
        public ProductDao()
        {
            db = new LNBshopDbContext();
        }

        public List<Product> ListAllProduct(ref int totalRecord, int pageIndex = 1, int pageSize = 6)
        {
            totalRecord = db.Products.Where(x => x.Status == true).Count();
            var model = db.Products.Where(x => x.Status == true).OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return model;
        }

        public List<Product> ListNewProduct(int top)
        {
            return db.Products.Where(x => x.Status == true).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        public List<Product> ListHotProduct(int top)
        {
            return db.Products.Where( x=>x.Status == true && x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        public List<Product> ListProductCategory(long CatID,ref int totalRecord, int pageIndex = 1, int pageSize = 8)
        {
            totalRecord = db.Products.Where(x => x.Status == true && x.CategoryID == CatID).Count();
            var model = db.Products.Where(x => x.Status == true && x.CategoryID == CatID).OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return model;
        }

        public Product ViewDetail(long id)
        {
            return db.Products.Find(id);
        }

        public List<Product> ListRelatedProduct(long CatID,int top)
        {
            return db.Products.Where(x => x.Status == true && x.CategoryID == CatID).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }
    }
}
