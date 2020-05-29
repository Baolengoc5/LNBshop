using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO.Client
{
    public class ContentDao
    {
        LNBshopDbContext db = null;
        public ContentDao()
        {
            db = new LNBshopDbContext();
        }

        public List<Content> ListNewContent(int top)
        {
            return db.Contents.OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }
    }
}
