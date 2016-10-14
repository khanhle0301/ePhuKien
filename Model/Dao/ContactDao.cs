using Common;
using Model.EF;
using System;
using System.Collections;
using System.Collections.Generic;
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

        public IEnumerable ListAll()
        {
            return db.ContactDetails;
        }

        public ContactDetail ViewDetail(int id)
        {
            return db.ContactDetails.Find(id);
        }

        public ContactDetail GetContact()
        {
            return db.ContactDetails.SingleOrDefault(x => x.ID == CommonConstants.DefaultContactDetailId);
        }

        public bool Update(ContactDetail entity)
        {
            try
            {
                var Contact = db.ContactDetails.Find(entity.ID);
                Contact.Name = entity.Name;
                Contact.Phone = entity.Phone;
                Contact.Email = entity.Email;
                Contact.Website = entity.Website;
                Contact.Address = entity.Address;
                Contact.Other = entity.Other;
                Contact.Lat = entity.Lat;
                Contact.Lng = entity.Lng;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}