using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO.Client
{
    public class ClientMenuDao
    {
        LNBshopDbContext db = null;// khai báo biến
        public ClientMenuDao()//tạo một constructor để tạo mới db
        {
            db = new LNBshopDbContext();
        }

        public List<Menu> ListbygroupID(int groupId)
        {
            return db.Menus.Where(x => x.TypeID == groupId && x.Status == true).ToList();
        }
    }
}
