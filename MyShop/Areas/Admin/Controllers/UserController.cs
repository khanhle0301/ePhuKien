using Model.Dao;
using MyShop.Areas.Admin.Models;
using MyShop.Common;
using MyShop.Models;
using System.Web.Mvc;
using MyShop.Infrastructure.Extensions;
using Model.EF;
using AutoMapper;

namespace MyShop.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/AdminUser
        [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index()
        {
            var result = new UserDao().ListAllPaging();
            return View(result);
        }

        [HasCredential(RoleID = "ADD_USER")]
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HasCredential(RoleID = "ADD_USER")]
        [HttpPost]
        public ActionResult Create(AdminModel adminModel)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var admin = new User();
                var encryptedMd5Pas = Encryptor.MD5Hash(adminModel.Password);
                admin.UserName = adminModel.UserName;
                admin.Password = encryptedMd5Pas;
                admin.Name = adminModel.Name;
                admin.Address = adminModel.Address;
                admin.Email = adminModel.Email;
                admin.Phone = adminModel.Phone;
                admin.GroupID = adminModel.GroupID;
                int id = dao.Insert(admin);
                if (id > 0)
                {
                    SetAlert("Thêm thành công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                    ModelState.AddModelError("", "Thêm thất bại");
            }
            SetViewBag(adminModel.GroupID);
            return View(adminModel);
        }

        [HasCredential(RoleID = "EDIT_USER")]
        [HttpGet]
        public ActionResult Edit(int id)
        {         
            var result = Mapper.Map<User, UserViewModel>(new UserDao().ViewDetail(id));           
            SetViewBag(result.GroupID);
            return View(result);
        }

        [HasCredential(RoleID = "EDIT_USER")]
        [HttpPost]
        public ActionResult Edit(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User();
                user.UpdateUser(model);
                var result = new UserDao().Update(user);
                if (result)
                {
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                    ModelState.AddModelError("", "Cập nhật thất bại");
            }
            SetViewBag(model.GroupID);
            return View(model);
        }
        
        [HasCredential(RoleID = "DELETE_USER")]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var result = new UserDao().ViewDetail(id);
            return View(result);
        }

        [HasCredential(RoleID = "DELETE_USER")]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var result = new UserDao().Delete(id);
            if (result)
            {
                SetAlert("Xóa thành công", "success");
                return RedirectToAction("Index", "User");
            }
            else
            {
                ViewData["Error"] = "Xóa thất bại!";
                var model = new UserDao().ViewDetail(id);
                return View(model);
            }
        }

        [HttpGet]
        public JsonResult UserNameExists(string username)
        {
            var result = new UserDao().UserNameExists(username);
            return Json(!result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult EmailExists(string email)
        {
            var result = new UserDao().EmailExists(email);
            return Json(!result, JsonRequestBehavior.AllowGet);
        }

        public void SetViewBag(int? selectedId = null)
        {
            var dao = new UserGroupDao();
            ViewBag.GroupID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }
    }
}