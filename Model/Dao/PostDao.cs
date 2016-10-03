using Common;
using Model.EF;
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