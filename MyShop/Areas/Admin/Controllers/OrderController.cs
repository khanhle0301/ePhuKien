using MyShop.Common;
using Model.Dao;
using System.Web.Mvc;

namespace MyShop.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Admin/Order
        [HasCredential(RoleID = "VIEW_ORDER")]
        public ActionResult Index()
        {
            var model = new OrderDao().ListAll();
            return View(model);
        }

        [HasCredential(RoleID = "DELETE_ORDER")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new OrderDao().Delete(id);
            return RedirectToAction("Index");
        }

        [HasCredential(RoleID = "DETAIL_ORDER")]
        public ActionResult Details(int id)
        {
            var result = new OrderDao().ViewDetail(id);
            ViewBag.OrderDetail= new OrderDao().GetOrderDetailByID(id);
            return View(result);
        }

        [HasCredential(RoleID = "EDIT_ORDER")]
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var result = new OrderDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        [HasCredential(RoleID = "EDIT_ORDER")]
        [HttpPost]
        public JsonResult ChangeStatusPayment(int id)
        {
            var result = new OrderDao().ChangeStatusPayment(id);
            return Json(new
            {
                status = result
            });
        }
    }
}