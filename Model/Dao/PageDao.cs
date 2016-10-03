using Model.EF;
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