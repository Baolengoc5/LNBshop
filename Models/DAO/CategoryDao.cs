using Models.EF;
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

        public List<Category> ListAll()
        {
            return db.Categories.Where(x => x.Status == true).ToList();
        }
    }
}
