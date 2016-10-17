using AutoMapper;
using Common;
using Model.Dao;
using Model.EF;
using MyShop.Common;
using MyShop.Infrastructure.Extensions;
using MyShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.Areas.Admin.Controllers
{
    public class ColorController : BaseController
    {
        // GET: Admin/Color
        ColorDao _colorDao = new ColorDao();
        Color _color = new Color();
        [HasCredential(RoleID = "VIEW_COLOR")]
        public ActionResult Index()
        {
            return View(_colorDao.GetAll());
        }

        [HasCredential(RoleID = "DELETE_COLOR")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            _colorDao.Delete(id);
            return RedirectToAction("Index");
        }

        [HasCredential(RoleID = "ADD_COLOR")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HasCredential(RoleID = "ADD_COLOR")]
        [HttpPost]
        public ActionResult Create(ColorViewModel model)
        {
            if (ModelState.IsValid)
            {               
                var color = new Color();
                color.UpdateColor(model);
                int id = _colorDao.Insert(color);
                if (id == 0)
                {
                    ModelState.AddModelError("", "Tên đã tồn tại");
                }
                else if (id > 0)
                {
                    SetAlert("Thêm thành công", "success");
                    return RedirectToAction("Index", "Color");
                }
                else
                    ModelState.AddModelError("", "Thêm thất bại");
            }
            return View(model);
        }

        [HasCredential(RoleID = "EDIT_COLOR")]
        [HttpGet]
        public ActionResult Edit(int id)
        {       
            var result = Mapper.Map<Color, ColorViewModel>(_colorDao.ViewDetail(id));
            return View(result);
        }

        [HasCredential(RoleID = "EDIT_COLOR")]
        [HttpPost]
        public ActionResult Edit(ColorViewModel model)
        {
            if (ModelState.IsValid)
            {            
                var color = new Color();
                color.UpdateColor(model);
                var result = _colorDao.Update(color);
                if (result == 0)
                {
                    ModelState.AddModelError("", "Tên đã tồn tại");
                }
                else if (result > 0)
                {
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("Index", "Color");
                }
                else
                    ModelState.AddModelError("", "Cập nhật thất bại");
            }
            return View(model);
        }
    }
}