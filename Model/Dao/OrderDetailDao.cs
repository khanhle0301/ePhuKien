using Model.EF;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace Model.Dao
{
    public class OrderDetailDao
    {
        private PhuKienDbContext db = null;

        public OrderDetailDao()
        {
            db = new PhuKienDbContext();
        }

        public int Insert(OrderDetail entity)
        {
            try
            {
                db.OrderDetails.Add(entity);
                db.SaveChanges();
                return entity.OrderID;
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