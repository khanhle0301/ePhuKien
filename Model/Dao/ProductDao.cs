using Common;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Xml.Linq;

namespace Model.Dao
{
    public class ProductDao
    {
        PhuKienDbContext db = null;

        public ProductDao()
        {
            db = new PhuKienDbContext();
        }

        public bool ChangeStatus(int id)
        {
            var cate = db.Products.Find(id);
            cate.Status = !cate.Status;
            db.SaveChanges();
            return cate.Status;
        }

        public void UpdateImages(long productId, string images)
        {
            var product = db.Products.Find(productId);
            product.MoreImages = images;
            db.SaveChanges();
        }

        public bool Delete(int id)
        {
            try
            {
                var cate = db.Products.Find(id);
                db.Products.Remove(cate);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Product ViewDetail(int id)
        {
            return db.Products.Find(id);
        }
        public IEnumerable<Product> ListAllPaging()
        {
            return db.Products.OrderByDescending(x => x.CreatedDate);
        }

        public IEnumerable<string> GetListProductByName(string name)
        {
            return db.Products.Include("ProductCategory").Where(x => x.Status && x.Name.Contains(name)).Select(y => y.Name);
        }

        public IEnumerable<Product> GetReatedProducts(int id, int top)
        {
            var product = db.Products.Find(id);
            return db.Products.Include("ProductCategory").Where(x => x.Status && x.ID != id && x.CategoryID == product.CategoryID).OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public void IncreaseView(int id)
        {
            var post = db.Products.Find(id);
            if (post.ViewCount.HasValue)
                post.ViewCount += 1;
            else
                post.ViewCount = 1;
            db.SaveChanges();
        }

        public Post GetByAlias(string alias)
        {
            return db.Posts.SingleOrDefault(x => x.Alias == alias);
        }

        public Product GetAllById(int id)
        {
            return db.Products.Include("ProductCategory").SingleOrDefault(x => x.ID == id);
        }

        public IEnumerable<Product> GetHomeLastest(int top)
        {
            return db.Products.Include("ProductCategory").Where(x => x.Status && x.HomeFlag == true)
                                .OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public IEnumerable<Product> GetHomeHotSaleProduct(int top)
        {
            return db.Products.Include("ProductCategory").Where(x => x.Status && x.HomeFlag == true && x.QuantitySold.HasValue)
                                .OrderByDescending(x => x.QuantitySold).Take(top);
        }

        public IEnumerable<Product> GetHomeSale(int top)
        {
            return db.Products.Include("ProductCategory").Where(x => x.Status && x.HomeFlag == true && x.PromotionPrice.HasValue)
                                .OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public IEnumerable<Product> GetHomeHotProduct()
        {
            return db.Products.Include("ProductCategory").Where(x => x.Status && x.HomeFlag == true && x.HotFlag == true)
                                .OrderByDescending(x => x.CreatedDate);
        }

        public IEnumerable<Product> GetViewCount(int top)
        {
            return db.Products.Include("ProductCategory").Where(x => x.Status).OrderByDescending(x => x.ViewCount).Take(top);
        }

        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }

        public IEnumerable<Product> GetProductColor(int id)
        {
            var product = db.ProductColors.Include("Product").Where(x => x.ColorID == id).Select(y => y.Product);
            return product.Include("ProductCategory");
        }

        public IEnumerable<Product> GetPopularProductPaging(string sort, string price, string color)
        {
            var query = db.Products.Include("ProductCategory").Where(x => x.Status && x.ViewCount.HasValue).Take(20);

            IEnumerable<Product> resultColor = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(color))
            {
                var colorArr = color.Split(',');
                foreach (var item in colorArr)
                {
                    resultColor = resultColor.Concat(this.GetProductColor(int.Parse(item)));
                }
            }
            else
            {
                resultColor = resultColor.Concat(query);
            }

            resultColor = resultColor.Distinct();

            IEnumerable<Product> resultPrice = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(price))
            {
                var priceArr = price.Split(',');
                for (int i = 0; i < priceArr.Length; i++)
                {
                    if (priceArr[i] == "-100")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price < 100000));
                    else if (priceArr[i] == "100-300")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price >= 100000 && x.Price <= 300000));
                    else if (priceArr[i] == "300-500")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price >= 300000 && x.Price <= 500000));
                    else if (priceArr[i] == "500-1000")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price >= 500000 && x.Price <= 1000000));
                    else if (priceArr[i] == "1000")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price > 1000000));
                }
            }
            else
            {
                resultPrice = resultPrice.Concat(resultColor); ;
            }

            var result = resultPrice.Distinct();
            switch (sort)
            {
                case "manual":
                    result = result.OrderByDescending(x => x.HotFlag);
                    break;
                case "price_asc":
                    result = result.OrderBy(x => x.Price);
                    break;
                case "price_desc":
                    result = result.OrderByDescending(x => x.Price);
                    break;
                case "name_asc":
                    result = result.OrderBy(x => x.Name);
                    break;
                case "name_desc":
                    result = result.OrderByDescending(x => x.Name);
                    break;
                case "updated_asc":
                    result = result.OrderBy(x => x.UpdatedDate);
                    break;
                case "updated_desc":
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
                case "sold_quantity":
                    result = result.OrderByDescending(x => x.QuantitySold);
                    break;
                default:
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
            }
            return result;
        }

        public IEnumerable<Product> GetAllProductPaging(string sort, string price, string color)
        {
            var query = db.Products.Include("ProductCategory").Where(x => x.Status);

            IEnumerable<Product> resultColor = Enumerable.Empty<Product>();

            IEnumerable<Product> resultPrice = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(color))
            {
                var colorArr = color.Split(',');
                foreach (var item in colorArr)
                {
                    resultColor = resultColor.Concat(this.GetProductColor(int.Parse(item)));
                }
            }
            else
            {
                resultColor = resultColor.Concat(query);
            }

            resultColor = resultColor.Distinct();

            if (!string.IsNullOrEmpty(price))
            {
                var priceArr = price.Split(',');
                for (int i = 0; i < priceArr.Length; i++)
                {
                    if (priceArr[i] == "-100")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price < 100000));
                    else if (priceArr[i] == "100-300")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price >= 100000 && x.Price <= 300000));
                    else if (priceArr[i] == "300-500")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price >= 300000 && x.Price <= 500000));
                    else if (priceArr[i] == "500-1000")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price >= 500000 && x.Price <= 1000000));
                    else if (priceArr[i] == "1000")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price > 1000000));
                }
            }
            else
            {
                resultPrice = resultPrice.Concat(resultColor); ;
            }

            var result = resultPrice.Distinct();
            switch (sort)
            {
                case "manual":
                    result = result.OrderByDescending(x => x.HotFlag);
                    break;
                case "price_asc":
                    result = result.OrderBy(x => x.Price);
                    break;
                case "price_desc":
                    result = result.OrderByDescending(x => x.Price);
                    break;
                case "name_asc":
                    result = result.OrderBy(x => x.Name);
                    break;
                case "name_desc":
                    result = result.OrderByDescending(x => x.Name);
                    break;
                case "updated_asc":
                    result = result.OrderBy(x => x.UpdatedDate);
                    break;
                case "updated_desc":
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
                case "sold_quantity":
                    result = result.OrderByDescending(x => x.QuantitySold);
                    break;
                default:
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
            }
            return result;
        }

        public IEnumerable<Product> GetAllBySearch(string keyword, string filter, string sort, string price, string color)
        {
            var query = db.Products.Include("ProductCategory").Where(x => x.Status && x.Name.Contains(keyword));

            if (!string.IsNullOrEmpty(filter))
            {
                switch (filter)
                {
                    case "onsale":
                        query = query.Where(x => x.PromotionPrice.HasValue);
                        break;
                    case "hot":
                        query = query.Where(x => x.HotFlag.HasValue);
                        break;
                    case "ban-chay":
                        query = query.Where(x => x.QuantitySold.HasValue);
                        break;
                    case "news":
                        query = query.OrderByDescending(x => x.UpdatedDate);
                        break;
                    default:
                        query = query.OrderByDescending(x => x.UpdatedDate);
                        break;
                }
            }
            else
            {
                query = query.Concat(query);
            }


            IEnumerable<Product> resultColor = Enumerable.Empty<Product>();

            IEnumerable<Product> resultPrice = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(color))
            {
                var colorArr = color.Split(',');
                foreach (var item in colorArr)
                {
                    resultColor = resultColor.Concat(this.GetProductColor(int.Parse(item)));
                }
            }
            else
            {
                resultColor = resultColor.Concat(query);
            }

            resultColor = resultColor.Distinct();

            if (!string.IsNullOrEmpty(price))
            {
                var priceArr = price.Split(',');
                for (int i = 0; i < priceArr.Length; i++)
                {
                    if (priceArr[i] == "-100")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price < 100000));
                    else if (priceArr[i] == "100-300")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price >= 100000 && x.Price <= 300000));
                    else if (priceArr[i] == "300-500")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price >= 300000 && x.Price <= 500000));
                    else if (priceArr[i] == "500-1000")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price >= 500000 && x.Price <= 1000000));
                    else if (priceArr[i] == "1000")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price > 1000000));
                }
            }
            else
            {
                resultPrice = resultPrice.Concat(resultColor); ;
            }

            var result = resultPrice.Distinct();
            switch (sort)
            {
                case "manual":
                    result = result.OrderByDescending(x => x.HotFlag);
                    break;
                case "price_asc":
                    result = result.OrderBy(x => x.Price);
                    break;
                case "price_desc":
                    result = result.OrderByDescending(x => x.Price);
                    break;
                case "name_asc":
                    result = result.OrderBy(x => x.Name);
                    break;
                case "name_desc":
                    result = result.OrderByDescending(x => x.Name);
                    break;
                case "updated_asc":
                    result = result.OrderBy(x => x.UpdatedDate);
                    break;
                case "updated_desc":
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
                case "sold_quantity":
                    result = result.OrderByDescending(x => x.QuantitySold);
                    break;
                default:
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
            }
            return result;
        }

        public IEnumerable<Product> GetSaleProductPaging(string sort, string price, string color)
        {
            var query = db.Products.Include("ProductCategory").Where(x => x.PromotionPrice.HasValue && x.Status);

            IEnumerable<Product> resultColor = Enumerable.Empty<Product>();

            IEnumerable<Product> resultPrice = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(color))
            {
                var colorArr = color.Split(',');
                foreach (var item in colorArr)
                {
                    resultColor = resultColor.Concat(this.GetProductColor(int.Parse(item)));
                }
            }
            else
            {
                resultColor = resultColor.Concat(query);
            }

            resultColor = resultColor.Distinct();

            if (!string.IsNullOrEmpty(price))
            {
                var priceArr = price.Split(',');
                for (int i = 0; i < priceArr.Length; i++)
                {
                    if (priceArr[i] == "-100")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price < 100000));
                    else if (priceArr[i] == "100-300")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price >= 100000 && x.Price <= 300000));
                    else if (priceArr[i] == "300-500")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price >= 300000 && x.Price <= 500000));
                    else if (priceArr[i] == "500-1000")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price >= 500000 && x.Price <= 1000000));
                    else if (priceArr[i] == "1000")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price > 1000000));
                }
            }
            else
            {
                resultPrice = resultPrice.Concat(resultColor); ;
            }

            var result = resultPrice.Distinct();
            switch (sort)
            {
                case "manual":
                    result = result.OrderByDescending(x => x.HotFlag);
                    break;
                case "price_asc":
                    result = result.OrderBy(x => x.Price);
                    break;
                case "price_desc":
                    result = result.OrderByDescending(x => x.Price);
                    break;
                case "name_asc":
                    result = result.OrderBy(x => x.Name);
                    break;
                case "name_desc":
                    result = result.OrderByDescending(x => x.Name);
                    break;
                case "updated_asc":
                    result = result.OrderBy(x => x.UpdatedDate);
                    break;
                case "updated_desc":
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
                case "sold_quantity":
                    result = result.OrderByDescending(x => x.QuantitySold);
                    break;
                default:
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
            }
            return result;
        }

        public IEnumerable<Product> GetHotProductPaging(string sort, string price, string color)
        {
            var query = db.Products.Include("ProductCategory").Where(x => x.HotFlag == true && x.Status);

            IEnumerable<Product> resultColor = Enumerable.Empty<Product>();

            IEnumerable<Product> resultPrice = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(color))
            {
                var colorArr = color.Split(',');
                foreach (var item in colorArr)
                {
                    resultColor = resultColor.Concat(this.GetProductColor(int.Parse(item)));
                }
            }
            else
            {
                resultColor = resultColor.Concat(query);
            }
            resultColor = resultColor.Distinct();

            if (!string.IsNullOrEmpty(price))
            {
                var priceArr = price.Split(',');
                for (int i = 0; i < priceArr.Length; i++)
                {
                    if (priceArr[i] == "-100")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price < 100000));
                    else if (priceArr[i] == "100-300")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price >= 100000 && x.Price <= 300000));
                    else if (priceArr[i] == "300-500")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price >= 300000 && x.Price <= 500000));
                    else if (priceArr[i] == "500-1000")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price >= 500000 && x.Price <= 1000000));
                    else if (priceArr[i] == "1000")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price > 1000000));
                }
            }
            else
            {
                resultPrice = resultPrice.Concat(resultColor); ;
            }

            var result = resultPrice.Distinct();
            switch (sort)
            {
                case "manual":
                    result = result.OrderByDescending(x => x.HotFlag);
                    break;
                case "price_asc":
                    result = result.OrderBy(x => x.Price);
                    break;
                case "price_desc":
                    result = result.OrderByDescending(x => x.Price);
                    break;
                case "name_asc":
                    result = result.OrderBy(x => x.Name);
                    break;
                case "name_desc":
                    result = result.OrderByDescending(x => x.Name);
                    break;
                case "updated_asc":
                    result = result.OrderBy(x => x.UpdatedDate);
                    break;
                case "updated_desc":
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
                case "sold_quantity":
                    result = result.OrderByDescending(x => x.QuantitySold);
                    break;
                default:
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
            }
            return result;
        }

        public IEnumerable<Product> GetAllByTagPaging(string tagId, string sort, string price, string color)
        {
            var query = from a in db.Products
                        join b in db.ProductTags
                        on a.ID equals b.ProductID
                        where b.TagID == tagId && a.Status
                        orderby a.CreatedDate descending
                        select a;
            query = query.Include("ProductCategory");

            IEnumerable<Product> resultColor = Enumerable.Empty<Product>();

            IEnumerable<Product> resultPrice = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(color))
            {
                var colorArr = color.Split(',');
                foreach (var item in colorArr)
                {
                    resultColor = resultColor.Concat(this.GetProductColor(int.Parse(item)));
                }
            }
            else
            {
                resultColor = resultColor.Concat(query);
            }

            resultColor = resultColor.Distinct();

            if (!string.IsNullOrEmpty(price))
            {
                var priceArr = price.Split(',');
                for (int i = 0; i < priceArr.Length; i++)
                {
                    if (priceArr[i] == "-100")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price < 100000));
                    else if (priceArr[i] == "100-300")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price >= 100000 && x.Price <= 300000));
                    else if (priceArr[i] == "300-500")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price >= 300000 && x.Price <= 500000));
                    else if (priceArr[i] == "500-1000")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price >= 500000 && x.Price <= 1000000));
                    else if (priceArr[i] == "1000")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price > 1000000));
                }
            }
            else
            {
                resultPrice = resultPrice.Concat(resultColor); ;
            }

            var result = resultPrice.Distinct();
            switch (sort)
            {
                case "manual":
                    result = result.OrderByDescending(x => x.HotFlag);
                    break;
                case "price_asc":
                    result = result.OrderBy(x => x.Price);
                    break;
                case "price_desc":
                    result = result.OrderByDescending(x => x.Price);
                    break;
                case "name_asc":
                    result = result.OrderBy(x => x.Name);
                    break;
                case "name_desc":
                    result = result.OrderByDescending(x => x.Name);
                    break;
                case "updated_asc":
                    result = result.OrderBy(x => x.UpdatedDate);
                    break;
                case "updated_desc":
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
                case "sold_quantity":
                    result = result.OrderByDescending(x => x.QuantitySold);
                    break;
                default:
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
            }
            return result;
        }

        public IEnumerable<Product> GetNewProductPaging(string sort, string price, string color)
        {
            var query = db.Products.Include("ProductCategory").Where(x => x.Status).OrderByDescending(x => x.UpdatedDate).Take(20);

            IEnumerable<Product> resultColor = Enumerable.Empty<Product>();

            IEnumerable<Product> resultPrice = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(color))
            {
                var colorArr = color.Split(',');
                foreach (var item in colorArr)
                {
                    resultColor = resultColor.Concat(this.GetProductColor(int.Parse(item)));
                }
            }
            else
            {
                resultColor = resultColor.Concat(query);
            }

            resultColor = resultColor.Distinct();

            if (!string.IsNullOrEmpty(price))
            {
                var priceArr = price.Split(',');
                for (int i = 0; i < priceArr.Length; i++)
                {
                    if (priceArr[i] == "-100")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price < 100000));
                    else if (priceArr[i] == "100-300")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price >= 100000 && x.Price <= 300000));
                    else if (priceArr[i] == "300-500")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price >= 300000 && x.Price <= 500000));
                    else if (priceArr[i] == "500-1000")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price >= 500000 && x.Price <= 1000000));
                    else if (priceArr[i] == "1000")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price > 1000000));
                }
            }
            else
            {
                resultPrice = resultPrice.Concat(resultColor); ;
            }

            var result = resultPrice.Distinct();
            switch (sort)
            {
                case "manual":
                    result = result.OrderByDescending(x => x.HotFlag);
                    break;
                case "price_asc":
                    result = result.OrderBy(x => x.Price);
                    break;
                case "price_desc":
                    result = result.OrderByDescending(x => x.Price);
                    break;
                case "name_asc":
                    result = result.OrderBy(x => x.Name);
                    break;
                case "name_desc":
                    result = result.OrderByDescending(x => x.Name);
                    break;
                case "updated_asc":
                    result = result.OrderBy(x => x.UpdatedDate);
                    break;
                case "updated_desc":
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
                case "sold_quantity":
                    result = result.OrderByDescending(x => x.QuantitySold);
                    break;
                default:
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
            }
            return result;
        }

        public IEnumerable<Product> GetListProductByCategoryIdPaging(int categoryId, string sort, string price, string color)
        {
            IEnumerable<Product> query = Enumerable.Empty<Product>();
            var category = db.ProductCategories.Find(categoryId);
            if (category.ParentID == null)
            {
                var childCategories = db.ProductCategories.Where(x => x.ParentID == category.ID);
                foreach (var item in childCategories)
                {
                    query = query.Concat(db.Products.Include("ProductCategory").Where(x => x.Status && x.CategoryID == item.ID));
                }
            }
            else
            {
                query = db.Products.Include("ProductCategory").Where(x => x.Status && x.CategoryID == categoryId);
            }

            IEnumerable<Product> resultColor = Enumerable.Empty<Product>();
            if (!string.IsNullOrEmpty(color))
            {
                var colorArr = color.Split(',');
                foreach (var item in colorArr)
                {
                    resultColor = resultColor.Concat(this.GetProductColor(int.Parse(item)));
                }
            }
            else
            {
                resultColor = resultColor.Concat(query);
            }
            resultColor = resultColor.Distinct();

            IEnumerable<Product> resultPrice = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(price))
            {
                var priceArr = price.Split(',');
                for (int i = 0; i < priceArr.Length; i++)
                {
                    if (priceArr[i] == "-100")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price < 100000));
                    else if (priceArr[i] == "100-300")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price >= 100000 && x.Price <= 300000));
                    else if (priceArr[i] == "300-500")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price >= 300000 && x.Price <= 500000));
                    else if (priceArr[i] == "500-1000")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price >= 500000 && x.Price <= 1000000));
                    else if (priceArr[i] == "1000")
                        resultPrice = resultPrice.Concat(resultColor.Where(x => x.Price > 1000000));
                }
            }
            else
            {
                resultPrice = resultPrice.Concat(resultColor); ;
            }

            var result = resultPrice.Distinct();
            switch (sort)
            {
                case "manual":
                    result = result.OrderByDescending(x => x.HotFlag);
                    break;
                case "price_asc":
                    result = result.OrderBy(x => x.Price);
                    break;
                case "price_desc":
                    result = result.OrderByDescending(x => x.Price);
                    break;
                case "name_asc":
                    result = result.OrderBy(x => x.Name);
                    break;
                case "name_desc":
                    result = result.OrderByDescending(x => x.Name);
                    break;
                case "updated_asc":
                    result = result.OrderBy(x => x.UpdatedDate);
                    break;
                case "updated_desc":
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
                case "sold_quantity":
                    result = result.OrderByDescending(x => x.QuantitySold);
                    break;
                default:
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
            }
            return result;
        }

        public List<string> MoreImage(int id)
        {
            ProductDao dao = new ProductDao();
            var product = dao.GetAllById(id);
            var images = product.MoreImages;
            List<string> listImagesReturn = new List<string>();
            if (!string.IsNullOrEmpty(images))
            {
                XElement xImages = XElement.Parse(images);
                foreach (XElement element in xImages.Elements())
                {
                    listImagesReturn.Add(element.Value);
                }
            }
            return listImagesReturn;
        }

        public List<Tag> ListTag(int contentId)
        {
            var model = (from a in db.Tags
                         join b in db.ProductTags
                         on a.ID equals b.TagID
                         where b.ProductID == contentId
                         select new
                         {
                             ID = b.TagID,
                             Name = a.Name
                         }).AsEnumerable().Select(x => new Tag()
                         {
                             ID = x.ID,
                             Name = x.Name
                         });
            return model.ToList();
        }

        public List<Color> ListColor(int id)
        {
            var model = (from a in db.Colors
                         join b in db.ProductColors
                         on a.ID equals b.ColorID
                         where b.ProductID == id
                         select new
                         {
                             ID = b.ColorID,
                             Name = a.Name
                         }).AsEnumerable().Select(x => new Color()
                         {
                             ID = x.ID,
                             Name = x.Name
                         });
            return model.ToList();
        }

        public void InsertTag(string id, string name)
        {
            var tag = new Tag();
            tag.ID = id;
            tag.Name = name;
            tag.Type = CommonConstants.ProductTag;
            db.Tags.Add(tag);
            db.SaveChanges();
        }

        public void InsertProductTag(int productId, string tagId)
        {
            ProductTag productTag = new ProductTag();
            productTag.ProductID = productId;
            productTag.TagID = tagId;
            db.ProductTags.Add(productTag);
            db.SaveChanges();
        }

        public bool CheckTag(string id)
        {
            return db.Tags.Count(x => x.ID == id) > 0;
        }

        public void InsertColor(int id, string name)
        {
            var color = new Color();
            color.ID = id;
            color.Name = name;
            color.Background = null;
            db.Colors.Add(color);
            db.SaveChanges();
        }

        public void InsertProductColor(int productId, int colorId)
        {
            ProductColor productColor = new ProductColor();
            productColor.ProductID = productId;
            productColor.ColorID = colorId;
            db.ProductColors.Add(productColor);
            db.SaveChanges();
        }

        public bool CheckColor(int id)
        {
            return db.Colors.Count(x => x.ID == id) > 0;
        }

        public int Insert(Product product)
        {
            try
            {
                db.Products.Add(product);
                db.SaveChanges();
                if (!string.IsNullOrEmpty(product.Tags))
                {
                    string[] tags = product.Tags.Split(',');
                    foreach (var tag in tags)
                    {
                        var tagId = StringHelper.ToUnsignString(tag);
                        var existedTag = this.CheckTag(tagId);
                        //insert to to tag table
                        if (!existedTag)
                        {
                            this.InsertTag(tagId, tag);
                        }
                        //insert to product tag
                        this.InsertProductTag(product.ID, tagId);
                    }
                }

                return product.ID;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public bool Update(Product product)
        {
            try
            {
                var model = db.Products.Find(product.ID);
                model.Name = product.Name;
                model.Alias = StringHelper.ToUnsignString(product.Name);
                model.MetaKeyword = product.MetaKeyword;
                model.CategoryID = product.CategoryID;
                model.Image = product.Image;
                model.Price = product.Price;
                model.PromotionPrice = product.PromotionPrice;
                model.Quantity = product.Quantity;
                model.Description = product.Description;
                model.Content = product.Content;
                model.Tags = product.Tags;
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = product.UpdatedBy;
                model.HomeFlag = product.HomeFlag;
                model.HotFlag = product.HotFlag;
                model.Status = product.Status;
                db.SaveChanges();
                this.RemoveAllContentTag(product.ID);
                //Xử lý tag
                if (!string.IsNullOrEmpty(product.Tags))
                {
                    string[] tags = product.Tags.Split(',');
                    foreach (var tag in tags)
                    {
                        var tagId = StringHelper.ToUnsignString(tag);
                        var existedTag = this.CheckTag(tagId);
                        //insert to to tag table
                        if (!existedTag)
                        {
                            this.InsertTag(tagId, tag);
                        }
                        //insert to product tag
                        this.InsertProductTag(product.ID, tagId);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void RemoveAllContentTag(int id)
        {
            db.ProductTags.RemoveRange(db.ProductTags.Where(x => x.ProductID == id));
            db.SaveChanges();
        }

        public void RemoveAllProductSize(int id)
        {
            db.ProductColors.RemoveRange(db.ProductColors.Where(x => x.ProductID == id));
            db.SaveChanges();
        }
    }
}