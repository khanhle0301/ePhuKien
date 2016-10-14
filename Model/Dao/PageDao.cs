using Common;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Dao
{
    public class PageDao
    {
        private PhuKienDbContext db = null;

        public PageDao()
        {
            db = new PhuKienDbContext();
        }

        public bool ChangeStatus(int id)
        {
            var cate = db.Pages.Find(id);
            cate.Status = !cate.Status;
            db.SaveChanges();
            return cate.Status;
        }

        public bool Update(Page entity)
        {
            try
            {
                var page = db.Pages.Find(entity.ID);
                page.Name = entity.Name;
                page.Alias = StringHelper.ToUnsignString(entity.Name);             
                page.Content = entity.Content;
                page.UpdatedDate = DateTime.Now;
                page.UpdatedBy = entity.UpdatedBy;
                page.MetaDescription = entity.MetaDescription;
                page.MetaKeyword = entity.MetaKeyword;
                page.DisplayOrder = entity.DisplayOrder;
                page.Status = entity.Status;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Page ViewDetail(int id)
        {
            return db.Pages.Find(id);
        }

        public IEnumerable<Page> ListAll()
        {
            return db.Pages.OrderBy(x => x.DisplayOrder);
        }

        public IEnumerable<Page> GetPages()
        {
            return db.Pages.Where(x => x.Status).OrderBy(x => x.DisplayOrder);
        }

        public Page GetByAlias(string alias)
        {
            return db.Pages.SingleOrDefault(x => x.Alias == alias);
        }
    }
}