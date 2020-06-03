using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO.Client
{
    public class ClientCategoryDao
    {
        LNBshopDbContext db = null;// khai báo biến
        public ClientCategoryDao()//tạo một constructor để tạo mới db
        {
            db = new LNBshopDbContext();
        }

        public List<ProductCategory> ListbygroupID()
        {
            return db.ProductCategories.Where(x =>x.Status == true).ToList();
        }

        public ProductCategory ViewDetail(long id)
        {
            return db.ProductCategories.Find(id);
        }
    }
}
