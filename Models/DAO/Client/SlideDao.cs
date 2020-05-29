using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO.Client
{
    public class SlideDao
    {
        LNBshopDbContext db = null;// khai báo biến
        public SlideDao()//tạo một constructor để tạo mới db
        {
            db = new LNBshopDbContext();
        }

        public List<Slide> ListAll()
        {
            return db.Slides.Where(x => x.Status == true).OrderBy(x=>x.DisplayOrder).ToList();
        }
    }
}
