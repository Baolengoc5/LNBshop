using Common;
using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class ContentDao //set public để project khác có thể gọi tới
    {
        LNBshopDbContext db = null;// khai báo biến
        public ContentDao()//tạo một constructor để tạo mới db
        {
            db = new LNBshopDbContext();
        }
        
        public long Insert(Content content)//tạo hàm chức năng Insert kiểu dữ liệu long vì trả về ID kiểu bigint
        {
            //Xử lý alias
            if (string.IsNullOrEmpty(content.MetaTitle))
            {
                content.MetaTitle = StringHelper.ToUnsignString(content.Name);
            }
            content.CreatedDate = DateTime.Now;
            content.ViewCount = 0;
            db.Contents.Add(content);
            db.SaveChanges();

            //Xử lý tag
            if (!string.IsNullOrEmpty(content.Tags))
            {
                string[] tags = content.Tags.Split(',');
                foreach (var tag in tags)
                {
                    var tagId = StringHelper.ToUnsignString(tag);
                    var existedTag = this.CheckTag(tagId);

                    //insert to to tag table
                    if (!existedTag)
                    {
                        this.InsertTag(tagId, tag);
                    }

                    //insert to content tag
                    this.InsertContentTag(content.ID, tagId);

                }
            }

            return content.ID;
        }

        public void InsertTag(string id, string name)
        {
            var tag = new Tag();
            tag.ID = id;
            tag.Name = name;
            db.Tags.Add(tag);
            db.SaveChanges();
        }

        public void InsertContentTag(long contentId, string tagId)
        {
            var contentTag = new ContentTag();
            contentTag.ContentID = contentId;
            contentTag.TagID = tagId;
            db.ContentTags.Add(contentTag);
            db.SaveChanges();
        }

        public bool CheckTag(string id)
        {
            return db.Tags.Count(x => x.ID == id) > 0;
        }

        //Sửa
        public bool Update(Content entity)
        {
            try
            {
                var content = db.Contents.Find(entity.ID);//tìm ID
                                                          //Xử lý alias
                if (string.IsNullOrEmpty(entity.MetaTitle))
                {
                    content.MetaTitle = StringHelper.ToUnsignString(content.Name);
                }
                if (!string.IsNullOrEmpty(entity.Detail))
                {
                    content.Detail = entity.Detail;
                }
                content.Name = entity.Name;
                content.Description = entity.Description;
                content.CategoryID = entity.CategoryID;

                content.Warranty = entity.Warranty;
                content.ModifiedBy = entity.ModifiedBy;
                content.ModifiedDate = DateTime.Now;
                content.MetaKeywords = entity.MetaKeywords;
                content.MetaDescriptions = entity.MetaDescriptions;

                content.Status = entity.Status;

                content.Tags = entity.Tags;
                content.Language = entity.Language;
                db.SaveChanges();

                //Xử lý tag
                if (!string.IsNullOrEmpty(entity.Tags))
                {
                    this.RemoveAllContentTag(entity.ID);
                    string[] tags = entity.Tags.Split(',');
                    foreach (var tag in tags)
                    {
                        var tagId = StringHelper.ToUnsignString(tag);
                        var existedTag = this.CheckTag(tagId);

                        //insert to to tag table
                        if (!existedTag)
                        {
                            this.InsertTag(tagId, tag);
                        }

                        //insert to content tag
                        this.InsertContentTag(entity.ID, tagId);

                    }
                }

                return true;
            }
            catch(Exception)
            {
                return false;
            }
            
        }

        public void RemoveAllContentTag(long contentId)
        {
            db.ContentTags.RemoveRange(db.ContentTags.Where(x => x.ContentID == contentId));
            db.SaveChanges();
        }

        //Xóa
        public bool Detele(int id)
        {
            try
            {
                var content = db.Contents.Find(id);
                db.Contents.Remove(content);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateImage(Content entityImage)
        {
            try
            {
                var content = db.Contents.Find(entityImage.ID);//tìm ID
                content.Image = entityImage.Image;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)//xử lý các trường hợp ngoại lệ
            {
                return false;
            }
        }

        //Hiển thị danh sách
        
        public IEnumerable<Content> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Content> model = db.Contents;
            if (!string.IsNullOrEmpty(searchString))//nếu searchString khác null
            {
                model = model.Where(x => x.Name.Contains(searchString));
                //OrderByDescending(x=>x.CreatedDate) là sắp xếp theo ngày tạo
                //.Contains(searchString) là tìm kiếm gần giống
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public int CountContentName(string contentName)//Kiểm tra tên đăng nhập có bị trùng lặp không
        {
            return db.Contents.Count(x => x.Name == contentName);
        }

        public Content GetByID(long? id)//để lấy thông tin tin tức thông qua id 
        {
            return db.Contents.Find(id);
            //SingleOrDefault : lấy một bảng ghi đơn thông qua id truyền vào
        }

        public bool ChangeStatus(long id)
        {
            var content = db.Contents.Find(id);
            content.Status = !content.Status;
            db.SaveChanges();
            return content.Status;
        }
    }
}
