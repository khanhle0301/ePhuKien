using Common;
using Model.Dao;
using Model.EF;
using MyShop.Common;
using MyShop.Infrastructure.Extensions;
using MyShop.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MyShop.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/Product
        [HasCredential(RoleID = "VIEW_PRODUCT")]
        public ActionResult Index()
        {
            var result = new ProductDao().ListAllPaging();
            return View(result);
        }

        [HasCredential(RoleID = "ADD_PRODUCT")]
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HasCredential(RoleID = "ADD_PRODUCT")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.PromotionPrice >= model.Price)
                {
                    ModelState.AddModelError("", "Vui lòng kiểm tra lại giá khuyến mãi.");
                }
                else
                {
                    model.Alias = StringHelper.ToUnsignString(model.Name);
                    model.CreatedDate = DateTime.Now;
                    model.UpdatedDate = DateTime.Now;
                    model.MoreImages = "[]";
                    var session = (AdminLogin)Session[CommonConstants.ADMIN_SESSION];
                    var entity = new UserDao().GetByID(session.UserName);
                    model.CreatedBy = entity.UserName;
                    model.ViewCount = 0;
                    model.QuantitySold = 0;
                    var product = new Product();
                    product.UpdateProduct(model);
                    var result = new ProductDao().Insert(product);
                    if (result > 0)
                    {
                        SetAlert("Thêm danh thành công", "success");
                        return RedirectToAction("Index", "Product");
                    }
                    else
                        ModelState.AddModelError("", "Thêm thất bại");
                }
            }
            SetViewBag();
            return View(model);
        }

        [HasCredential(RoleID = "EDIT_PRODUCT")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var dao = new ProductDao();
            var result = dao.ViewDetail(id);
            SetViewBag(result.CategoryID);
            return View(result);
        }

        [HasCredential(RoleID = "EDIT_PRODUCT")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.PromotionPrice >= model.Price)
                {
                    ModelState.AddModelError("", "Vui lòng kiểm tra lại giá khuyến mãi.");
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        var session = (AdminLogin)Session[CommonConstants.ADMIN_SESSION];
                        var entity = new UserDao().GetByID(session.UserName);
                        model.UpdatedBy = entity.UserName;
                        model.MoreImages = "[]";
                        var product = new Product();
                        product.UpdateProduct(model);
                        var result = new ProductDao().Update(product);
                        if (result)
                        {
                            SetAlert("Cập nhật thành công", "success");
                            return RedirectToAction("Index", "Product");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Cập nhật thất bại");
                        }
                    }
                }
            }
            SetViewBag(model.CategoryID);
            return View(model);
        }

        [HasCredential(RoleID = "DELETE_PRODUCT")]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var result = new ProductDao().ViewDetail(id);
            return View(result);
        }

        [HasCredential(RoleID = "DELETE_PRODUCT")]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var dao = new ProductDao().Delete(id);
            if (dao)
            {
                SetAlert("Xóa thành công", "success");
                return RedirectToAction("Index", "Product");
            }
            else
            {
                ViewData["Error"] = "Xóa thất bại!";
                var result = new ProductDao().ViewDetail(id);
                return View(result);
            }
        }

        public void SetViewBag(long? selectedId = null)
        {
            var dao = new ProductCategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ListByChild(), "ID", "Name", selectedId);
        }

        [HasCredential(RoleID = "EDIT_PRODUCT")]
        public JsonResult LoadImages(int id)
        {
            ProductDao dao = new ProductDao();
            var product = dao.ViewDetail(id);
            List<string> listImagesReturn = new JavaScriptSerializer().Deserialize<List<string>>(product.MoreImages);
            return Json(new
            {
                data = listImagesReturn
            }, JsonRequestBehavior.AllowGet);
        }

        [HasCredential(RoleID = "EDIT_PRODUCT")]
        public JsonResult SaveImages(int id, string images)
        {
            var listImages = images.Replace(ConfigHelper.GetByKey("CurrentLink"), "/");

            ProductDao dao = new ProductDao();
            try
            {
                dao.UpdateImages(id, listImages);
                return Json(new
                {
                    status = true
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    status = false
                });
            }
        }

        [HasCredential(RoleID = "EDIT_PRODUCT")]
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var result = new ProductDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}