using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Dao
{
    public class FeedbackDao
    {
        private PhuKienDbContext db = null;

        public FeedbackDao()
        {
            db = new PhuKienDbContext();
        }

        public bool ChangeStatus(int id)
        {
            var user = db.Feedbacks.Find(id);
            user.Status = !user.Status;
            db.SaveChanges();
            return user.Status;
        }

        public bool Delete(int id)
        {
            try
            {
                var feedback = db.Feedbacks.Find(id);
                db.Feedbacks.Remove(feedback);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Feedback ViewDetail(int id)
        {
            return db.Feedbacks.Find(id);
        }

        public IEnumerable<Feedback> ListAll()
        {
            return db.Feedbacks.OrderByDescending(x => x.CreatedDate).ToList();
        }
    }
}