using Model.EF;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace Model.Dao
{
    public class OrderDao
    {
        private PhuKienDbContext db = null;

        public OrderDao()
        {
            db = new PhuKienDbContext();
        }

        public int Insert(Order entity)
        {
            try
            {
                db.Orders.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                return 0;
            }
        }
    }
}