using AutoMapper;
using Model.Dao;
using Model.EF;
using MyShop.Common;
using MyShop.Infrastructure.Extensions;
using MyShop.Models;
using System.Web.Mvc;

namespace MyShop.Areas.Admin.Controllers
{
    public class ContactController : BaseController
    {
        // GET: Admin/Contact
        [HasCredential(RoleID = "VIEW_CONTACT")]
        public ActionResult Index()
        {
            var model = new ContactDao().ListAll();
            return View(model);
        }

        [HasCredential(RoleID = "EDIT_CONTACT")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = new ContactDao().ViewDetail(id);
            var res = Mapper.Map<ContactDetail,ContactDetailViewModel>(model);
            return View(res);
        }

        [HasCredential(RoleID = "EDIT_CONTACT")]
        [HttpPost]
        public ActionResult Edit(ContactDetailViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var contact = new ContactDetail();
                    contact.UpdateContactDetail(model);
                    var result = new ContactDao().Update(contact);
                    if (result)
                    {
                        SetAlert("Cập nhật thành công", "success");
                        return RedirectToAction("Index", "Contact");
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