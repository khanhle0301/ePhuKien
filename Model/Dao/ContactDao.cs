using Common;
using Model.EF;
using System.Linq;

namespace Model.Dao
{
    public class ContactDao
    {
        private PhuKienDbContext db = null;

        public ContactDao()
        {
            db = new PhuKienDbContext();
        }

        public ContactDetail GetContact()
        {
            return db.ContactDetails.SingleOrDefault(x => x.ID == CommonConstants.DefaultContactDetailId);
        }
    }
}