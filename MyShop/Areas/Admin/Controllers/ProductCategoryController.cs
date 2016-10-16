using MyShop.Common;
using Common;
using Model.Dao;
using Model.EF;
using System;
using System.Web.Mvc;
using MyShop.Models;
using AutoMapper;
using MyShop.Infrastructure.Extensions;

namespace MyShop.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        // GET: Admin/ProductCategory
        [HasCredential(RoleID = "VIEW_PRODUCTCATEGORY")]
        public ActionResult Index()
        {
            var result = new ProductCategoryDao().ListAllPaging();
            return View(result);
        }

        [HasCredential(RoleID = "ADD_PRODUCTCATEGORY")]
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HasCredential(RoleID = "ADD_PRODUCTCATEGORY")]
        [HttpPost]
        public ActionResult Create(ProductCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Alias = StringHelper.ToUnsignString(model.Name);
                model.CreatedDate = DateTime.Now;
                model.UpdatedDate = DateTime.Now;
                var session = (AdminLogin)Session[CommonConstants.ADMIN_SESSION];
                var entity = new UserDao().GetByID(session.UserName);
                model.CreatedBy = entity.UserName;
                var productCategory = new ProductCategory();
                productCategory.UpdateProductCategory(model);
                var result = new ProductCategoryDao().Insert(productCategory);
                if (result > 0)
                {
                    SetAlert("Thêm danh thành công", "success");
                    return RedirectToAction("Index", "ProductCategory");
                }
                else
                    ModelState.AddModelError("", "Thêm thất bại");
            }
            SetViewBag();
            return View(model);
        }

        [HasCredential(RoleID = "EDIT_PRODUCTCATEGORY")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var dao = new ProductCategoryDao();
            var result = Mapper.Map<ProductCategory, ProductCategoryViewModel>(dao.ViewDetail(id));
            SetViewBag(result.ParentID);
            return View(result);
        }

        [HasCredential(RoleID = "EDIT_PRODUCTCATEGORY")]
        [HttpPost]
        public ActionResult Edit(ProductCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var cate = new ProductCategory();
                var session = (AdminLogin)Session[CommonConstants.ADMIN_SESSION];
                var entity = new UserDao().GetByID(session.UserName);
                model.UpdatedBy = entity.UserName;
                var productCategory = new ProductCategory();
                productCategory.UpdateProductCategory(model);
                var result = new ProductCategoryDao().Update(productCategory);
                if (result > 0)
                {
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("Index", "ProductCategory");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật thất bại");
                }
            }
            SetViewBag(model.ID);
            return View(model);
        }

        public void SetViewBag(int? selectedId = null)
        {
            var dao = new ProductCategoryDao();
            ViewBag.ParentID = new SelectList(dao.ListAllParent(), "ID", "Name", selectedId);
        }

        [HasCredential(RoleID = "DELETE_PRODUCTCATEGORY")]
        public ActionResult Delete(int id)
        {
            var result = new ProductCategoryDao().ViewDetail(id);
            return View(result);
        }

        [HasCredential(RoleID = "DELETE_PRODUCTCATEGORY")]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var dao = new ProductCategoryDao().Delete(id);
            if (dao)
            {
                SetAlert("Xóa thành công", "success");
                return RedirectToAction("Index", "ProductCategory");
            }
            else
            {
                ViewData["Error"] = "Xóa thất bại!";
                var result = new ProductCategoryDao().ViewDetail(id);
                return View(result);
            }
        }

        [HasCredential(RoleID = "EDIT_PRODUCTCATEGORY")]
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var result = new ProductCategoryDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}