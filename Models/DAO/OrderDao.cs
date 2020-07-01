using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class OrderDao
    {
        LNBshopDbContext db = null;// khai báo biến
        public OrderDao()//tạo một constructor để tạo mới db
        {
            db = new LNBshopDbContext();
        }

        public bool ChangeStatus(long id)
        {
            var order = db.Orders.Find(id);
            order.Status = !order.Status;
            db.SaveChanges();
            return order.Status;
        }

        //Xóa
        public bool Detele(int id)
        {
            try
            {
                var order = db.Orders.Find(id);
                db.Orders.Remove(order);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //Hiển thị danh sách
        public IEnumerable<Order> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Order> model = db.Orders;
            if (!string.IsNullOrEmpty(searchString))//nếu searchString khác null
            {
                model = model.Where(x => x.ShipName.Contains(searchString));
                //OrderByDescending(x=>x.CreatedDate) là sắp xếp theo ngày tạo
                //.Contains(searchString) là tìm kiếm gần giống
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
    }
}
