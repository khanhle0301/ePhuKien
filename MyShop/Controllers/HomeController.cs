using AutoMapper;
using Common;
using Model.Dao;
using Model.EF;
using MyShop.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MyShop.Controllers
{
    public class HomeController : Controller
    {
        private SlideDao _slideDao = new SlideDao();
        private ProductDao _productDao = new ProductDao();
        private FooterDao _footerDao = new FooterDao();
        private ContactDao _contactDao = new ContactDao();
        private PageDao _pageDao = new PageDao();
        private ProductCategoryDao _productCategoryDao = new ProductCategoryDao();
        private PostDao _postDao = new PostDao();

        public ActionResult Index()
        {
            var homeViewModel = new HomeViewModel();
            var slideView = Mapper.Map<IEnumerable<Slide>, IEnumerable<SlideViewModel>>(_slideDao.GetSlides());
            var lastestProductModel = _productDao.GetHomeLastest(4);
            var hotSaleProductModel = _productDao.GetHomeHotSaleProduct(8);
            var saleProductModel = _productDao.GetHomeSale(4);
            var hotProductModel = _productDao.GetHomeHotProduct();

            var lastestProductViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lastestProductModel);
            var hotSaleProductViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(hotSaleProductModel);
            var saleProductViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(saleProductModel);
            var hotProductViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(hotProductModel);

            homeViewModel.Slides = slideView;
            homeViewModel.LastestProducts = lastestProductViewModel;
            homeViewModel.HotSaleProducts = hotSaleProductViewModel;
            homeViewModel.SaleProducts = saleProductViewModel;
            homeViewModel.HotProducts = hotProductViewModel;
            return View(homeViewModel);
        }

        [ChildActionOnly]
        public ActionResult Footer()
        {
            var footerViewModel = Mapper.Map<Footer, FooterViewModel>(_footerDao.GetFooter());
            return PartialView(footerViewModel);
        }

        [ChildActionOnly]
        public ActionResult HeaderCart()
        {
            if (Session[CommonConstants.SessionCart] == null)
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            return PartialView(cart);
        }

        [ChildActionOnly]
        public ActionResult TopBar()
        {
            var contactViewModel = Mapper.Map<ContactDetail, ContactDetailViewModel>(_contactDao.GetContact());
            return PartialView(contactViewModel);
        }

        [ChildActionOnly]
        public ActionResult Page()
        {
            var model = Mapper.Map<IEnumerable<Page>, IEnumerable<PageViewModel>>(_pageDao.GetPages());
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult PostNew()
        {
            var model = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(_postDao.GetNew(3));
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Category()
        {
            var model = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(_productCategoryDao.GetCategory());
            return PartialView(model);
        }
    }
}