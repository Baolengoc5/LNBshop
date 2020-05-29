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

        public List<Product> ListNewProduct(int top)
        {
            return db.Products.OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        public List<Product> ListHotProduct(int top)
        {
            return db.Products.Where(x=>x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }
    }
}
