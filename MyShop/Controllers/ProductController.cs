using AutoMapper;
using Common;
using Model.Dao;
using Model.EF;
using MyShop.Infrastructure.Core;
using MyShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MyShop.Controllers
{
    public class ProductController : Controller
    {
        private ProductDao _productDao = new ProductDao();
        private ProductCategoryDao _productCategoryDao = new ProductCategoryDao();
        private TagDao _tagDao = new TagDao();       
        private ColorDao _colorDao = new ColorDao();      
        public ActionResult Index(string alias, int page = 1, string sort = "updated_desc", string price = "", string color = "")
        {
            var category = _productCategoryDao.GetByAlias(alias);
            ViewBag.Category = Mapper.Map<ProductCategory, ProductCategoryViewModel>(category);
            ViewBag.Sort = sort;
            ViewBag.Price = price;
            ViewBag.Color = color;
            ViewBag.Colors = Mapper.Map<IEnumerable<Color>, IEnumerable<ColorViewModel>>(_colorDao.GetAll());
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var productModel = _productDao.GetListProductByCategoryIdPaging(category.ID, page, pageSize, sort, price, color, out totalRow);
            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = 5,
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            return View(paginationSet);
        }

        public ActionResult ViewAll(int page = 1, string sort = "updated_desc", string price = "", string color = "")
        {
            ViewBag.Sort = sort;
            ViewBag.Price = price;
            ViewBag.Color = color;
            ViewBag.Colors = Mapper.Map<IEnumerable<Color>, IEnumerable<ColorViewModel>>(_colorDao.GetAll());
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var productModel = _productDao.GetAllProductPaging(page, pageSize, sort, price, color, out totalRow);
            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = 5,
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            return View(paginationSet);
        }

        public ActionResult ViewPopular(int page = 1, string sort = "manual", string price = "", string color = "")
        {           
            ViewBag.Sort = sort;
            ViewBag.Price = price;
            ViewBag.Color = color;
            ViewBag.Colors = Mapper.Map<IEnumerable<Color>, IEnumerable<ColorViewModel>>(_colorDao.GetAll());
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var productModel = _productDao.GetPopularProductPaging(page, pageSize, sort, price, color, out totalRow);
            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = 5,
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            return View(paginationSet);
        }

        public ActionResult ViewSale(int page = 1, string sort = "updated_desc", string price = "", string color = "")
        {
            ViewBag.Sort = sort;
            ViewBag.Price = price;
            ViewBag.Color = color;
            ViewBag.Colors = Mapper.Map<IEnumerable<Color>, IEnumerable<ColorViewModel>>(_colorDao.GetAll());
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var productModel = _productDao.GetSaleProductPaging(page, pageSize, sort, price, color, out totalRow);
            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = 5,
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            return View(paginationSet);
        }

        public ActionResult ViewHot(int page = 1, string sort = "updated_desc", string price = "", string color = "")
        {
            ViewBag.Sort = sort;
            ViewBag.Price = price;
            ViewBag.Color = color;
            ViewBag.Colors = Mapper.Map<IEnumerable<Color>, IEnumerable<ColorViewModel>>(_colorDao.GetAll());
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var productModel = _productDao.GetHotProductPaging(page, pageSize, sort, price, color, out totalRow);
            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = 5,
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            return View(paginationSet);
        }


        public ActionResult Detail(int id)
        {
            var productModel = _productDao.GetAllById(id);
            var viewModel = Mapper.Map<Product, ProductViewModel>(productModel);
            //List<string> listImages = new JavaScriptSerializer().Deserialize<List<string>>(viewModel.MoreImages);
            //ViewBag.MoreImages = listImages;
            ViewBag.Tags = Mapper.Map<IEnumerable<Tag>, IEnumerable<TagViewModel>>(_tagDao.GetListTagByProductId(id));
            ViewBag.Colors = Mapper.Map<IEnumerable<Color>, IEnumerable<ColorViewModel>>(_colorDao.GetListColorByProductId(id));
            ViewBag.Related = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(_productDao.GetReatedProducts(id, 12));

            _productDao.IncreaseView(id);           

            var recentProduct = (List<RecentProductItem>)Session[CommonConstants.RecentProductSession];
            if (recentProduct == null)
            {
                recentProduct = new List<RecentProductItem>();
            }
            if (recentProduct.Any(x => x.ProductId == id))
            {
                foreach (var item in recentProduct)
                {
                    if (item.ProductId == id)
                    {
                        item.CreateDate = DateTime.Now;
                    }
                }
            }
            else
            {
                RecentProductItem newItem = new RecentProductItem();
                newItem.ProductId = id;
                newItem.Product = productModel;
                newItem.CreateDate = DateTime.Now;
                recentProduct.Add(newItem);
            }
            Session[CommonConstants.RecentProductSession] = recentProduct;

            return View(viewModel);
        }

        [ChildActionOnly]
        public ActionResult RecentProduct()
        {
            var recentProduct = Session[CommonConstants.RecentProductSession];
            var list = new List<RecentProductItem>();
            if (recentProduct != null)
            {
                list = (List<RecentProductItem>)recentProduct;
            }
            return PartialView(list);
        }

        [ChildActionOnly]
        public ActionResult Category()
        {
            var model = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(_productCategoryDao.GetCategory());
            return PartialView(model);
        }


        [ChildActionOnly]
        public ActionResult PopularProduct()
        {
            var model = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(_productDao.GetViewCount(3));
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult TopOnSale()
        {
            var model = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(_productDao.GetHomeSale(3));
            return PartialView(model);
        }

        public JsonResult GetAll(int id)
        {
            var model = _productDao.GetAllById(id);
            return Json(new
            {
                data = model,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetColor(int id)
        {
            var model = _colorDao.GetListColorByProductId(id);
            return Json(new
            {
                data = model,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
    }
}