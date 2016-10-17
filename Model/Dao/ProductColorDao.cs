using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ProductColorDao
    {
        private PhuKienDbContext db = null;

        public ProductColorDao()
        {
            db = new PhuKienDbContext();
        }        

        public void Insert(ProductColor productColor)
        {
            db.ProductColors.Add(productColor);
            db.SaveChanges();
        }


        public void Create(int productId, string color)
        {
            if (!string.IsNullOrEmpty(color))
            {
                foreach (var _color in color.Split(','))
                {
                    this.Insert(new ProductColor()
                    {
                        ProductID = productId,
                        ColorID = int.Parse(_color)
                    });
                }
            }

        }

        public void Update(int productId, string color)
        {
            this.RemoveAllProductColor(productId);
            if (!string.IsNullOrEmpty(color))
            {
                foreach (var _color in color.Split(','))
                {
                    this.Insert(new ProductColor()
                    {
                        ProductID = productId,
                        ColorID = int.Parse(_color)
                    });
                }
            }
        }

        public void RemoveAllProductColor(int id)
        {
            db.ProductColors.RemoveRange(db.ProductColors.Where(x => x.ProductID == id));
            db.SaveChanges();
        }

    }
}
