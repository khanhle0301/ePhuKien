using AutoMapper;
using Model.Dao;
using Model.EF;
using MyShop.Common;
using MyShop.Infrastructure.Extensions;
using MyShop.Models;
using System.Web.Mvc;

namespace MyShop.Areas.Admin.Controllers
{
    public class SlideController : BaseController
    {
        // GET: Admin/Slide
        [HasCredential(RoleID = "VIEW_SLIDE")]
        public ActionResult Index()
        {
            var model = new SlideDao().ListAll();
            return View(model);
        }

        [HasCredential(RoleID = "CHANGESTATUS_SLIDE")]
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var result = new SlideDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        [HasCredential(RoleID = "DELETE_SLIDE")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new SlideDao().Delete(id);
            return RedirectToAction("Index");
        }

        [HasCredential(RoleID = "ADD_SLIDE")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HasCredential(RoleID = "ADD_SLIDE")]
        [HttpPost]
        public ActionResult Create(SlideViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var slide = new Slide();
                    slide.UpdateSlide(model);
                    int id = new SlideDao().Insert(slide);
                    if (id > 0)
                    {
                        SetAlert("Thêm thành công", "success");
                        return RedirectToAction("Index", "Slide");
                    }
                    else
                        ModelState.AddModelError("", "Thêm thất bại");
                }
                return View(model);
            }
            catch
            { return View(); }
        }

        [HasCredential(RoleID = "EDIT_SLIDE")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = new SlideDao().ViewDetail(id);
            var result = Mapper.Map<Slide, SlideViewModel>(model);
            return View(result);
        }

        [HasCredential(RoleID = "EDIT_SLIDE")]
        [HttpPost]
        public ActionResult Edit(SlideViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var slide = new Slide();
                    slide.UpdateSlide(model);
                    var result = new SlideDao().Update(slide);
                    if (result)
                    {
                        SetAlert("Cập nhật thành công", "success");
                        return RedirectToAction("Index", "Slide");
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