using AutoMapper;
using Model.Dao;
using Model.EF;
using MyShop.Models;
using System.Web.Mvc;

namespace MyShop.Controllers
{
    public class PageController : Controller
    {
        private PageDao _pageDao = new PageDao();

        // GET: Page
        public ActionResult Index(string alias)
        {
            var page = _pageDao.GetByAlias(alias);
            var model = Mapper.Map<Page, PageViewModel>(page);
            return View(model);
        }
    }
}