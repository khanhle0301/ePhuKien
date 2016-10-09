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
        public ActionResult Index(string alias)
        {
            var category = _productCategoryDao.GetByAlias(alias);
            ViewBag.Category = Mapper.Map<ProductCategory, ProductCategoryViewModel>(category);
            ViewBag.Colors = Mapper.Map<IEnumerable<Color>, IEnumerable<ColorViewModel>>(_colorDao.GetAll());
            return View();
        }

        [HttpGet]
        public JsonResult LoadDataCategory(int id, int page, int pageSize, string sort = "", string price = "", string color = "")
        {
            var model = _productDao.GetListProductByCategoryIdPaging(id, sort, price, color);
            int totalRow = model.Count();
            model = model.Skip((page - 1) * pageSize).Take(pageSize);
            return Json(new
            {
                data = model,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewAll()
        {
            ViewBag.Colors = Mapper.Map<IEnumerable<Color>, IEnumerable<ColorViewModel>>(_colorDao.GetAll());
            return View();
        }

        [HttpGet]
        public JsonResult LoadDataAll(int page, int pageSize, string sort = "", string price = "", string color = "")
        {
            var model = _productDao.GetAllProductPaging(sort, price, color);
            int totalRow = model.Count();
            model = model.Skip((page - 1) * pageSize).Take(pageSize);
            return Json(new
            {
                data = model,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewPopular()
        {
            ViewBag.Colors = Mapper.Map<IEnumerable<Color>, IEnumerable<ColorViewModel>>(_colorDao.GetAll());
            return View();
        }

        [HttpGet]
        public JsonResult LoadDataPopular(int page, int pageSize, string sort = "", string price = "", string color = "")
        {
            var model = _productDao.GetPopularProductPaging(sort, price, color);
            int totalRow = model.Count();
            model = model.Skip((page - 1) * pageSize).Take(pageSize);
            return Json(new
            {
                data = model,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewSale()
        {
            ViewBag.Colors = Mapper.Map<IEnumerable<Color>, IEnumerable<ColorViewModel>>(_colorDao.GetAll());
            return View();
        }

        [HttpGet]
        public JsonResult LoadDataOnsale(int page, int pageSize, string sort = "", string price = "", string color = "")
        {
            var model = _productDao.GetSaleProductPaging(sort, price, color);
            int totalRow = model.Count();
            model = model.Skip((page - 1) * pageSize).Take(pageSize);
            return Json(new
            {
                data = model,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewHot()
        {
            ViewBag.Colors = Mapper.Map<IEnumerable<Color>, IEnumerable<ColorViewModel>>(_colorDao.GetAll());
            return View();
        }

        public ActionResult ViewNew()
        {
            ViewBag.Colors = Mapper.Map<IEnumerable<Color>, IEnumerable<ColorViewModel>>(_colorDao.GetAll());
            return View();
        }

        [HttpGet]
        public JsonResult LoadDataHot(int page, int pageSize, string sort = "", string price = "", string color = "")
        {
            var model = _productDao.GetHotProductPaging(sort, price, color);
            int totalRow = model.Count();
            model = model.Skip((page - 1) * pageSize).Take(pageSize);
            return Json(new
            {
                data = model,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadDataNew(int page, int pageSize, string sort = "", string price = "", string color = "")
        {
            var model = _productDao.GetNewProductPaging(sort, price, color);
            int totalRow = model.Count();
            model = model.Skip((page - 1) * pageSize).Take(pageSize);
            return Json(new
            {
                data = model,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListByTag(string tagId)
        {
            ViewBag.Tag = tagId;
            ViewBag.Colors = Mapper.Map<IEnumerable<Color>, IEnumerable<ColorViewModel>>(_colorDao.GetAll());
            return View();
        }

        [HttpGet]
        public JsonResult LoadDataByTag(string tagid, int page, int pageSize, string sort = "", string price = "", string color = "")
        {
            var model = _productDao.GetAllByTagPaging(tagid, sort, price, color);
            int totalRow = model.Count();
            model = model.Skip((page - 1) * pageSize).Take(pageSize);
            return Json(new
            {
                data = model,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
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