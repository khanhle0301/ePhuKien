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


        //public IEnumerable<Product> ListAllByTag(string tag, int page, int pageSize, string sort, out int totalRow)
        //{
        //    var query = (from a in db.Products
        //                 join b in db.ProductTags
        //                 on a.ID equals b.ProductID
        //                 where b.TagID == tag
        //                 select new
        //                 {
        //                     ID = a.ID,
        //                     Name = a.Name,
        //                     MetaTitle = a.Metatitle,
        //                     Image = a.Image,
        //                     MoreImage = a.MoreImage,
        //                     Price = a.Price,
        //                     PromotionPrice = a.PromotionPrice,
        //                     Quantity = a.Quantity,
        //                     Description = a.Description,
        //                     Detail = a.Detail,
        //                     CreatedDate = a.CreatedDate,
        //                     CreatedBy = a.CreatedBy,
        //                     UpdatedDate = a.UpdatedDate,
        //                     UpdatedBy = a.UpdatedBy,
        //                     ViewCount = a.ViewCount,
        //                     Status = a.Status

        //                 }).AsEnumerable().Select(x => new Product()
        //                 {
        //                     ID = x.ID,
        //                     Name = x.Name,
        //                     Metatitle = x.MetaTitle,
        //                     Image = x.Image,
        //                     MoreImage = x.MoreImage,
        //                     Price = x.Price,
        //                     PromotionPrice = x.PromotionPrice,
        //                     Quantity = x.Quantity,
        //                     Description = x.Description,
        //                     Detail = x.Detail,
        //                     CreatedDate = x.CreatedDate,
        //                     CreatedBy = x.CreatedBy,
        //                     UpdatedDate = x.UpdatedDate,
        //                     UpdatedBy = x.UpdatedBy,
        //                     ViewCount = x.ViewCount,
        //                     Status = x.Status
        //                 });
        //    switch (sort)
        //    {
        //        case "hot":
        //            query = query.OrderByDescending(x => x.ViewCount);
        //            break;
        //        case "price":
        //            query = query.OrderBy(x => x.Price);
        //            break;
        //        case "price_desc":
        //            query = query.OrderByDescending(x => x.Price);
        //            break;
        //        case "name":
        //            query = query.OrderBy(x => x.Name);
        //            break;
        //        case "name_desc":
        //            query = query.OrderByDescending(x => x.Name);
        //            break;
        //        case "new_desc":
        //            query = query.OrderByDescending(x => x.CreatedDate);
        //            break;
        //        case "new":
        //            query = query.OrderBy(x => x.CreatedDate);
        //            break;
        //        case "popular":
        //            query = query.OrderByDescending(x => x.QuantitySold);
        //            break;
        //        default:
        //            query = query.OrderByDescending(x => x.CreatedDate);
        //            break;
        //    }
        //    totalRow = query.Count();
        //    return query.Skip((page - 1) * pageSize).Take(pageSize);
        //}

        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }

        public List<Product> ListNewProduct(int top)
        {
            return db.Products.Where(x => x.Status && x.HomeFlag == true).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        public List<Product> ListPromotionProduct(int top)
        {
            return db.Products.Where(x => x.Status && x.PromotionPrice.HasValue && x.HomeFlag == true).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        public List<Product> ListHotProduct(int top)
        {
            return db.Products.Where(x => x.Status && x.QuantitySold > 0 && x.HomeFlag == true).OrderByDescending(x => x.QuantitySold).Take(top).ToList();
        }

        public List<Product> ListRelatedProducts(int productId)
        {
            var product = db.Products.Find(productId);
            return db.Products.Where(x => x.ID != productId && x.CategoryID == product.CategoryID).ToList();
        }

        public IEnumerable<Product> ListAllProduct(int page, int pageSize, string sort, out int totalRow)
        {
            var query = db.Products.Where(x => x.Status);
            switch (sort)
            {
                case "hot":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;
                case "price":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(x => x.Price);
                    break;
                case "name":
                    query = query.OrderBy(x => x.Name);
                    break;
                case "name_desc":
                    query = query.OrderByDescending(x => x.Name);
                    break;
                case "new_desc":
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
                case "new":
                    query = query.OrderBy(x => x.CreatedDate);
                    break;
                case "popular":
                    query = query.OrderByDescending(x => x.QuantitySold);
                    break;
                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }
            totalRow = query.Count();
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<Product> GetPopularProductPaging(string sort, string price, string color)
        {
            var query = db.Products.Include("ProductCategory").Where(x => x.Status && x.ViewCount.HasValue).Take(20);

            IEnumerable<Product> resultColor = Enumerable.Empty<Product>();

            IEnumerable<Product> resultPrice = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(color))
            {
                var colorArr = color.Split(',');
                foreach (var item in colorArr)
                {
                    resultColor = resultColor.Concat(query.Where(x => x.Colors != null && x.Colors.ToLower().Contains(item.ToLower())));
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
                    resultColor = resultColor.Concat(query.Where(x => x.Colors != null && x.Colors.ToLower().Contains(item.ToLower())));
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
                    resultColor = resultColor.Concat(query.Where(x => x.Colors != null && x.Colors.ToLower().Contains(item.ToLower())));
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
                    resultColor = resultColor.Concat(query.Where(x => x.Colors != null && x.Colors.ToLower().Contains(item.ToLower())));
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
                    resultColor = resultColor.Concat(query.Where(x => x.Colors != null && x.Colors.ToLower().Contains(item.ToLower())));
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
                    resultColor = resultColor.Concat(query.Where(x => x.Colors != null && x.Colors.ToLower().Contains(item.ToLower())));
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
                    resultColor = resultColor.Concat(query.Where(x => x.Colors != null && x.Colors.ToLower().Contains(item.ToLower())));
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


        public IEnumerable<Product> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            var query = db.Products.Where(x => x.Status && x.Name.Contains(keyword));
            switch (sort)
            {
                case "hot":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;
                case "price":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(x => x.Price);
                    break;
                case "name":
                    query = query.OrderBy(x => x.Name);
                    break;
                case "name_desc":
                    query = query.OrderByDescending(x => x.Name);
                    break;
                case "new_desc":
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
                case "new":
                    query = query.OrderBy(x => x.CreatedDate);
                    break;
                case "popular":
                    query = query.OrderByDescending(x => x.QuantitySold);
                    break;
                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }
            totalRow = query.Count();
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<Product> ListAllSaleOffProduct(int page, int pageSize, string sort, out int totalRow)
        {
            var query = db.Products.Where(x => x.Status && x.PromotionPrice.HasValue && x.PromotionPrice > 0);
            switch (sort)
            {
                case "hot":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;
                case "price":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(x => x.Price);
                    break;
                case "name":
                    query = query.OrderBy(x => x.Name);
                    break;
                case "name_desc":
                    query = query.OrderByDescending(x => x.Name);
                    break;
                case "new_desc":
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
                case "new":
                    query = query.OrderBy(x => x.CreatedDate);
                    break;
                case "popular":
                    query = query.OrderByDescending(x => x.QuantitySold);
                    break;
                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }
            totalRow = query.Count();
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<Product> ListAllHotProduct(int page, int pageSize, string sort, out int totalRow)
        {
            var query = db.Products.Where(x => x.Status && x.QuantitySold > 0);
            switch (sort)
            {
                case "hot":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;
                case "price":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(x => x.Price);
                    break;
                case "name":
                    query = query.OrderBy(x => x.Name);
                    break;
                case "name_desc":
                    query = query.OrderByDescending(x => x.Name);
                    break;
                case "new_desc":
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
                case "new":
                    query = query.OrderBy(x => x.CreatedDate);
                    break;
                case "popular":
                    query = query.OrderByDescending(x => x.QuantitySold);
                    break;
                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }
            totalRow = query.Count();
            return query.Skip((page - 1) * pageSize).Take(pageSize);
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

        public bool UpdateViewCount(int id)
        {
            try
            {
                var product = db.Products.Find(id);
                product.ViewCount = product.ViewCount + 1;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
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
    }
}