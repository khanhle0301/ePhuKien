using Common;
using Model.EF;
using System.Collections;
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


        public IEnumerable<Color> GetAll()
        {
            return db.Colors;
        }
    }
}
