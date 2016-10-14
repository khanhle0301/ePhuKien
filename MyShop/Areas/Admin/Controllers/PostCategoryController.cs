using AutoMapper;
using Common;
using Model.Dao;
using Model.EF;
using MyShop.Common;
using MyShop.Models;
using System;
using System.Web.Mvc;
using MyShop.Infrastructure.Extensions;

namespace MyShop.Areas.Admin.Controllers
{
    public class PostCategoryController : BaseController
    {
        // GET: Admin/PostCategory
        [HasCredential(RoleID = "VIEW_POSTCATEGORY")]
        public ActionResult Index()
        {
            var result = new PostCategoryDao().ListAllPaging();
            return View(result);
        }

        [HasCredential(RoleID = "ADD_POSTCATEGORY")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HasCredential(RoleID = "ADD_POSTCATEGORY")]
        [HttpPost]
        public ActionResult Create(PostCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Alias = StringHelper.ToUnsignString(model.Name);
                model.CreatedDate = DateTime.Now;
                model.UpdatedDate = DateTime.Now;
                var session = (AdminLogin)Session[CommonConstants.ADMIN_SESSION];
                var entity = new UserDao().GetByID(session.UserName);
                model.CreatedBy = entity.Name;
                var postCategory = new PostCategory();
                postCategory.UpdatePostCategory(model);
                var result = new PostCategoryDao().Insert(postCategory);
                if (result == 0)
                {
                    ModelState.AddModelError("", "Tên danh mục đã tồn tại");
                }
                else if (result > 0)
                {
                    SetAlert("Thêm danh thành công", "success");
                    return RedirectToAction("Index", "PostCategory");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm thất bại");
                }
            }
            return View(model);
        }

        [HasCredential(RoleID = "EDIT_POSTCATEGORY")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var dao = new PostCategoryDao();
            var result = Mapper.Map<PostCategory, PostCategoryViewModel>(dao.ViewDetail(id));          
            return View(result);
        }

        [HasCredential(RoleID = "EDIT_POSTCATEGORY")]
        [HttpPost]
        public ActionResult Edit(PostCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var session = (AdminLogin)Session[CommonConstants.ADMIN_SESSION];
                var entity = new UserDao().GetByID(session.UserName);
                model.UpdatedBy = entity.Name;
                var postCategory = new PostCategory();
                postCategory.UpdatePostCategory(model);
                var result = new PostCategoryDao().Update(postCategory);
                if (result == 0)
                {
                    ModelState.AddModelError("", "Tên danh mục đã tồn tại");
                }
                else if (result > 0)
                {
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("Index", "PostCategory");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật thất bại");
                }
            }
            return View(model);
        }

        [HasCredential(RoleID = "DELETE_POSTCATEGORY")]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var result = new PostCategoryDao().ViewDetail(id);
            return View(result);
        }

        [HasCredential(RoleID = "DELETE_POSTCATEGORY")]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var result = new PostCategoryDao().Delete(id);
            if (result)
            {
                SetAlert("Xóa thành công", "success");
                return RedirectToAction("Index", "PostCategory");
            }
            else
            {
                ViewData["Error"] = "Xóa thất bại!";
                var postCate = new PostCategoryDao().ViewDetail(id);
                return View(postCate);
            }
        }


        [HasCredential(RoleID = "EDIT_POSTCATEGORY")]
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var result = new PostCategoryDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}