using AutoMapper;
using Model.Dao;
using Model.EF;
using MyShop.Common;
using MyShop.Infrastructure.Extensions;
using MyShop.Models;
using System.Web.Mvc;

namespace MyShop.Areas.Admin.Controllers
{
    public class FooterController : BaseController
    {
        // GET: Admin/Footer
        [HasCredential(RoleID = "VIEW_FOOTER")]
        public ActionResult Index()
        {
            var model = new FooterDao().ListAll();
            return View(model);
        }     

        [HasCredential(RoleID = "EDIT_FOOTER")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var dao = new FooterDao();
            var result = Mapper.Map<Footer, FooterViewModel>(dao.ViewDetail(id));
            return View(result);
        }

        [HasCredential(RoleID = "EDIT_FOOTER")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FooterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var footer = new Footer();
                    footer.UpdateFooter(model);
                    var result = new FooterDao().Update(footer);
                    if (result)
                    {
                        SetAlert("Cập nhật thành công", "success");
                        return RedirectToAction("Index", "Footer");
                    }
                    else
                        ModelState.AddModelError("", "Cập nhật thất bại");
                }
                return View(model);
            }
            catch
            { return View(); }
        }
    }
}