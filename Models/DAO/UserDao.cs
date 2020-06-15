using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class UserDao //set public để project khác có thể gọi tới
    {
        LNBshopDbContext db = null;// khai báo biến
        public UserDao()//tạo một constructor để tạo mới db
        {
            db = new LNBshopDbContext();
        }

        //Thêm
        public long Insert(User entity)//tạo hàm chức năng Insert kiểu dữ liệu long vì trả về ID kiểu bigint
        {
            db.Users.Add(entity);//phương thức thêm trong entity
            db.SaveChanges();//Lưu thay đổi trong database
            return entity.ID;
        }

        //Sửa
        public bool Update(User entity)
        {
            try
            {
                var user = db.Users.Find(entity.ID);//tìm ID
                if (!string.IsNullOrEmpty(entity.Password))//kiểm tra nếu người dùng nhập password mới thực thi
                {
                    user.Password = entity.Password;
                }
                user.Name = entity.Name;
                user.Address = entity.Address;
                user.Email = entity.Email;
                user.ProvinceID = entity.ProvinceID;
                user.DistrictID = entity.DistrictID;
                user.ModifiedBy = entity.ModifiedBy;
                user.ModifiedDate = DateTime.Now;
                user.Status = entity.Status;
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)//xử lý các trường hợp ngoại lệ
            {
                return false;
            }

        }

        public bool ChangeStatus(long id)
        {
            var user = db.Users.Find(id);
            user.Status = !user.Status;
            db.SaveChanges();
            return user.Status;
        }

        //Xóa
        public bool Detele(int id)
        {
            try
            {
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //Hiển thị danh sách
        public IEnumerable<User> ListAllPaging(string searchString,int page, int pageSize)
        {
            IQueryable<User> model = db.Users;
            if (!string.IsNullOrEmpty(searchString))//nếu searchString khác null
            {
                model = model.Where(x=>x.UserName.Contains(searchString) || x.Name.Contains(searchString));
                //OrderByDescending(x=>x.CreatedDate) là sắp xếp theo ngày tạo
                //.Contains(searchString) là tìm kiếm gần giống
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        //Đăng nhập
        public int Login(string userName, string passWord)//truyền vào 2 tham số
        {
            //Tìm Username và gọi ra bảng ghi của nó
            var result = db.Users.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.Status == false)
                {
                    return -1;
                }
                else
                {
                    if (result.Password == passWord)
                    {
                        return 1;
                    }
                    else
                    {
                        return -2;
                    }
                }
            }
        }

        // Lấy SESSION
        public User GetByUserName(string userName)//để lấy thông tin user thông qua userName để làm session 
        {
            return db.Users.SingleOrDefault(x => x.UserName == userName);
            //SingleOrDefault : lấy một bảng ghi đơn thông qua username truyền vào
        }

        public int CountUserName(string userName)//Kiểm tra tên đăng nhập có bị trùng lặp không
        {
            return db.Users.Count(x => x.UserName == userName);
        }

        public User ViewDetail(int? id)//Hàm này để tìm kiếm một bản ghi dựa trên khóa chính
        {
            return db.Users.Find(id);
            //Cách này cũng giống với hàm GetByUserName ở trên
            //Nhưng tìm khóa chính thì cách này ngắn hơn
        }

        //Register của user CLIENT
        public bool CheckUserName(string userName)
        {
            return db.Users.Count(x => x.UserName == userName) > 0;
        }
        public bool CheckEmail(string email)
        {
            return db.Users.Count(x => x.Email == email) > 0;
        }
    }
}
