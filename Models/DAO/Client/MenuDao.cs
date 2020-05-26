using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO.Client
{
    public class MenuDao
    {
        LNBshopDbContext db = null;// khai báo biến
        public MenuDao()//tạo một constructor để tạo mới db
        {
            db = new LNBshopDbContext();
        }

        public List<Menu> ListbygroupID(int groupId)
        {
            return db.Menus.Where(x => x.TypeID == groupId).ToList();
        }
    }
}
