using Common;
using Model.EF;
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

        public Footer GetFooter()
        {
            return db.Footers.SingleOrDefault(x => x.ID == CommonConstants.DefaultFooterId);
        }
    }
}