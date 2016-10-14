using MyShop.Common;
using Common;
using Model.Dao;
using Model.EF;
using System;
using System.Web.Mvc;
using AutoMapper;
using MyShop.Models;
using MyShop.Infrastructure.Extensions;

namespace MyShop.Areas.Admin.Controllers
{
    public class PostController : BaseController
    {
        // GET: Admin/Post
        [HasCredential(RoleID = "VIEW_POST")]
        public ActionResult Index()
        {
            var result = new PostDao().ListAllPaging();
            return View(result);
        }

        [HasCredential(RoleID = "ADD_POST")]
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HasCredential(RoleID = "ADD_POST")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(PostViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Alias = StringHelper.ToUnsignString(model.Name);
                model.CreatedDate = DateTime.Now;
                model.UpdatedDate = DateTime.Now;
                var session = (AdminLogin)Session[CommonConstants.ADMIN_SESSION];
                var entity = new UserDao().GetByID(session.UserName);
                model.CreatedBy = entity.Name;
                var post = new Post();
                post.UpdatePost(model);
                var result = new PostDao().Insert(post);
                if (result > 0)
                {
                    SetAlert("Thêm danh thành công", "success");
                    return RedirectToAction("Index", "Post");
                }
                else
                    ModelState.AddModelError("", "Thêm thất bại");
            }
            SetViewBag();
            return View(model);
        }

        [HasCredential(RoleID = "EDIT_POST")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var dao = new PostDao();
            var result = Mapper.Map<Post, PostViewModel>(dao.ViewDetail(id)); ;
            SetViewBag(result.CategoryID);
            return View(result);
        }

        [HasCredential(RoleID = "EDIT_POST")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(PostViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    var session = (AdminLogin)Session[CommonConstants.ADMIN_SESSION];
                    var entity = new UserDao().GetByID(session.UserName);
                    model.UpdatedBy = entity.Name;
                    var post = new Post();
                    post.UpdatePost(model);
                    var result = new PostDao().Update(post);
                    if (result)
                    {
                        SetAlert("Cập nhật thành công", "success");
                        return RedirectToAction("Index", "Post");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cập nhật thất bại");
                    }
                }
            }
            SetViewBag(model.ID);
            return View(model);
        }       

        public void SetViewBag(int? selectedId = null)
        {
            var dao = new PostCategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }

        [HasCredential(RoleID = "EDIT_POST")]
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var result = new PostDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        [HasCredential(RoleID = "DELETE_POST")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new PostDao().Delete(id);
            return RedirectToAction("Index");
        }
    }
}