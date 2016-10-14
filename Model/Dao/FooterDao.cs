using Common;
using Model.EF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Model.Dao
{
    public class FooterDao
    {
        private PhuKienDbContext db = null;

        public FooterDao()
        {
            db = new PhuKienDbContext();
        }

        public bool Update(Footer entity)
        {
            try
            {
                var Footer = db.Footers.Find(entity.ID);
                Footer.Content = entity.Content;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<Footer> ListAll()
        {
            return db.Footers;
        }

        public Footer ViewDetail(int id)
        {
            return db.Footers.Find(id);
        }

        public Footer GetFooter()
        {
            return db.Footers.SingleOrDefault(x => x.ID == CommonConstants.DefaultFooterId);
        }
    }
}