using Common;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Dao
{
    public class ColorDao
    {
        private PhuKienDbContext db = null;

        public ColorDao()
        {
            db = new PhuKienDbContext();
        }

        public IEnumerable<Color> GetListColorByProductId(int id)
        {
            return db.ProductColors.Include("Color").Where(x => x.ProductID == id).Select(y => y.Color);
        }

        public bool Delete(int id)
        {
            try
            {
                var cate = db.Colors.SingleOrDefault(x => x.ID == id);
                db.Colors.Remove(cate);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int Insert(Color entity)
        {
            if (db.Colors.Any(x => x.Name == entity.Name))
                return 0;
            db.Colors.Add(entity);
            db.SaveChanges();
            return 1;
        }

        public int Update(Color entity)
        {
            if (db.Colors.Any(x => x.Name == entity.Name && x.ID != entity.ID))
                return 0;
            var model = db.Colors.Find(entity.ID);           
            model.Background = entity.Background;
            db.SaveChanges();
            return 1;
        }

        public Color ViewDetail(int id)
        {
            return db.Colors.SingleOrDefault(x => x.ID == id);
        }

        public IEnumerable<Color> GetAll()
        {
            return db.Colors;
        }
    }
}