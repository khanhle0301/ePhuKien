using Model.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;

namespace Model.Dao
{
    public class OrderDao
    {
        private PhuKienDbContext db = null;

        public OrderDao()
        {
            db = new PhuKienDbContext();
        }

        public IEnumerable<OrderDetail> GetOrderDetailByID(int id)
        {
            return db.OrderDetails.Include("Product").Where(x => x.OrderID == id);
        }

        public bool Delete(int id)
        {
            try
            {
                var Order = db.Orders.Find(id);
                db.Orders.Remove(Order);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<Order> ListAll()
        {
            return db.Orders;
        }

        public Order ViewDetail(int id)
        {
            return db.Orders.Find(id);
        }

        public bool ChangeStatusPayment(int id)
        {
            var order = db.Orders.Find(id);
            order.PaymentStatus = !order.PaymentStatus;
            db.SaveChanges();
            return order.PaymentStatus;
        }

        public bool ChangeStatus(int id)
        {
            var order = db.Orders.Find(id);
            order.Status = !order.Status;
            db.SaveChanges();
            return order.Status;
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