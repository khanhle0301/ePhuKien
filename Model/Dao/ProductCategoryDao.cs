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
            return db.ProductCategories.Where(x => x.Status == true && x.ParentID == null).OrderBy(x => x.DisplayOrder).ToList();
        }

        public List<ProductCategory> ListByID(int parentId)
        {
            return db.ProductCategories.Where(x => x.ParentID == parentId & x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }      
    }
}