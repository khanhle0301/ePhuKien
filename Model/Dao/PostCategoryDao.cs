using Model.EF;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Model.Dao
{
    public class PostCategoryDao
    {
        private PhuKienDbContext db = null;

        public PostCategoryDao()
        {
            db = new PhuKienDbContext();
        }

        public PostCategory GetById(int id)
        {
            return db.PostCategories.Find(id);
        }

        public IEnumerable<PostCategory> GetPostCategroy()
        {
            return db.PostCategories.Where(x => x.Status).OrderBy(x => x.DisplayOrder);
        }

        public PostCategory GetByAlias(string alias)
        {
            return db.PostCategories.SingleOrDefault(x => x.Alias == alias);
        }
    }
}