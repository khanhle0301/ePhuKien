using Common;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Model.Dao
{
    public class PostDao
    {
        private PhuKienDbContext db = null;

        public PostDao()
        {
            db = new PhuKienDbContext();
        }

        public bool ChangeStatus(int id)
        {
            var cate = db.Posts.Find(id);
            cate.Status = !cate.Status;
            db.SaveChanges();
            return cate.Status;
        }

        public void InsertTag(string id, string name)
        {
            var tag = new Tag();
            tag.ID = id;
            tag.Name = name;
            tag.Type = CommonConstants.PostTag;
            db.Tags.Add(tag);
            db.SaveChanges();
        }

        public void InsertPostTag(int postId, string tagId)
        {
            PostTag postTag = new PostTag();
            postTag.PostID = postId;
            postTag.TagID = tagId;
            db.PostTags.Add(postTag);
            db.SaveChanges();
        }

        public bool CheckTag(string id)
        {
            return db.Tags.Count(x => x.ID == id) > 0;
        }

        public int Insert(Post post)
        {
            try
            {
                db.Posts.Add(post);
                db.SaveChanges();
                if (!string.IsNullOrEmpty(post.Tags))
                {
                    string[] tags = post.Tags.Split(',');
                    foreach (var tag in tags)
                    {
                        var tagId = StringHelper.ToUnsignString(tag);
                        var existedTag = this.CheckTag(tagId);
                        //insert to to tag table
                        if (!existedTag)
                        {
                            this.InsertTag(tagId, tag);
                        }
                        //insert to product tag
                        this.InsertPostTag(post.ID, tagId);
                    }
                }
                return post.ID;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public bool Update(Post post)
        {
            try
            {
                var model = db.Posts.Find(post.ID);
                model.Name = post.Name;
                model.Alias = StringHelper.ToUnsignString(post.Name);
                model.CategoryID = post.CategoryID;
                model.MetaKeyword = post.MetaKeyword;
                model.Image = post.Image;
                model.Description = post.Description;
                model.MetaDescription = post.MetaDescription;
                model.Content = post.Content;
                model.Tags = post.Tags;              
                model.Status = post.Status;
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = post.UpdatedBy;
                db.SaveChanges();
                //Xử lý tag
                this.RemoveAllContentTag(post.ID);
                if (!string.IsNullOrEmpty(post.Tags))
                {
                    string[] tags = post.Tags.Split(',');
                    foreach (var tag in tags)
                    {
                        var tagId = StringHelper.ToUnsignString(tag);
                        var existedTag = this.CheckTag(tagId);
                        //insert to to tag table
                        if (!existedTag)
                        {
                            this.InsertTag(tagId, tag);
                        }
                        //insert to product tag
                        this.InsertPostTag(post.ID, tagId);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void RemoveAllContentTag(int id)
        {
            db.PostTags.RemoveRange(db.PostTags.Where(x => x.PostID == id));
            db.SaveChanges();
        }


        public Post ViewDetail(int id)
        {
            return db.Posts.Find(id);
        }
      
        public void Delete(int id)
        {
            var cate = db.Posts.Find(id);
            db.Posts.Remove(cate);
            db.SaveChanges();
        }

        public IEnumerable<Post> ListAllPaging()
        {
            return db.Posts.OrderByDescending(x => x.CreatedDate);
        }

        public void IncreaseView(int id)
        {
            var post = db.Posts.Find(id);
            if (post.ViewCount.HasValue)
                post.ViewCount += 1;
            else
                post.ViewCount = 1;
            db.SaveChanges();
        }

        public IEnumerable<Post> GetReatedPosts(int id, int top)
        {
            var post = db.Posts.Find(id);
            return db.Posts.Include("PostCategory").Where(x => x.Status && x.ID != id && x.CategoryID == post.CategoryID).OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public IEnumerable<Post> GetReatedTakePosts(int id, int top)
        {
            var post = db.Posts.Find(id);
            return db.Posts.Include("PostCategory").Where(x => x.Status && x.ID != id && x.CategoryID == post.CategoryID).OrderByDescending(x => x.CreatedDate).Skip(2).Take(top);
        }

        public Post GetAllById(int id)
        {
            return db.Posts.Include("PostCategory").SingleOrDefault(x => x.ID == id);
        }

        public IEnumerable<Tag> GetListTagByPostId(int id)
        {
            return db.PostTags.Include("Tag").Where(x => x.PostID == id).Select(y => y.Tag); ;
        }

        public IEnumerable<Post> GetNew(int top)
        {
            return db.Posts.Include("PostCategory").Where(x => x.Status).OrderByDescending(x => x.CreatedDate).Take(top);
        }
        public IEnumerable<Post> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow)
        {
            var model = db.Posts.Include("PostCategory").Where(x => x.Status && x.CategoryID == categoryId).OrderByDescending(x => x.CreatedDate);
            totalRow = model.Count();
            return model.OrderByDescending(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<Post> GetAllByTagPaging(string tagId, int page, int pageSize, out int totalRow)
        {
            var query = from a in db.Posts
                        join b in db.PostTags
                        on a.ID equals b.PostID
                        join c in db.PostCategories
                        on a.CategoryID equals c.ID
                        where b.TagID == tagId && a.Status
                        orderby a.CreatedDate descending
                        select a;
            var model = query.Include("PostCategory");
            totalRow = model.Count();
            return model.OrderByDescending(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<Post> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            var model = db.Posts.Include("PostCategory").Where(x => x.Status).OrderByDescending(x => x.CreatedDate);
            totalRow = model.Count();
            return model.OrderByDescending(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}