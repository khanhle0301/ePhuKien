using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Model.Dao
{    
    public class TagDao
    {
        private PhuKienDbContext db = null;

        public TagDao()
        {
            db = new PhuKienDbContext();
        }

        public Tag GetById(string tagId)
        {
            return db.Tags.SingleOrDefault(x => x.ID == tagId);
        }

        public IEnumerable<Tag> GetListTagByProductId(int id)
        {
            return db.ProductTags.Include("Tag").Where(x => x.ProductID == id).Select(y => y.Tag);
        }

        public IEnumerable<Tag> GetPostTags()
        {
            return db.Tags.Where(x => x.Type == "post");
        }
    }
}
