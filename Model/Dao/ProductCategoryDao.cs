using Common;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;

namespace Model.Dao
{
    public class ProductCategoryDao
    {
        private PhuKienDbContext db = null;

        public ProductCategoryDao()
        {
            db = new PhuKienDbContext();
        }

        public bool ChangeStatus(int id)
        {
            var cate = db.ProductCategories.Find(id);
            cate.Status = !cate.Status;
            db.SaveChanges();
            return cate.Status;
        }

        public bool Delete(int id)
        {
            try
            {
                var cate = db.ProductCategories.Find(id);
                db.ProductCategories.Remove(cate);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool NameExists(string name)
        {
            return db.ProductCategories.Any(x => x.Name == name);
        }

        public int Insert(ProductCategory entity)
        {
            if (db.ProductCategories.Any(x => x.Name == entity.Name))
                return 0;
            db.ProductCategories.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public int Update(ProductCategory entity)
        {
            if (db.ProductCategories.Any(x => x.Name == entity.Name && x.ID != entity.ID))
                return 0;
            var model = db.ProductCategories.Find(entity.ID);
            model.Name = entity.Name;
            model.Alias = StringHelper.ToUnsignString(entity.Name);
            model.ParentID = entity.ParentID;
            model.DisplayOrder = entity.DisplayOrder;
            model.MetaKeyword = entity.MetaKeyword;
            model.MetaDescription = entity.MetaDescription;         
            model.UpdatedDate = DateTime.Now;
            model.UpdatedBy = entity.UpdatedBy;
            model.Status = entity.Status;
            db.SaveChanges();
            return entity.ID;
        }
        public ProductCategory ViewDetail(int id)
        {
            return db.ProductCategories.Find(id);
        }

        public IEnumerable<ProductCategory> ListAllPaging()
        {
            return db.ProductCategories.OrderByDescending(x => x.CreatedDate);
        }

        public ProductCategory GetByAlias(string alias)
        {
            return db.ProductCategories.SingleOrDefault(x => x.Alias == alias);
        }

        public List<ProductCategory> ListByChild()
        {
            return db.ProductCategories.Where(x => x.ParentID != null).OrderBy(x => x.DisplayOrder).ToList();
        }

        public List<ProductCategory> GetCategory()
        {
            return db.ProductCategories.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }

        public List<ProductCategory> ListAllParent()
        {
            return db.ProductCategories.Where(x => x.ParentID == null).OrderBy(x => x.DisplayOrder).ToList();
        }

        public List<ProductCategory> ListByID(int parentId)
        {
            return db.ProductCategories.Where(x => x.ParentID == parentId & x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }
    }
}