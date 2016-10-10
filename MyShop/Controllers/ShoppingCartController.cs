using AutoMapper;
using Common;
using Model.Dao;
using Model.EF;
using MyShop.Common;
using MyShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;

namespace MyShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ProductDao _productDao = new ProductDao();

        public ActionResult Index()
        {
            if (Session[CommonConstants.SessionCart] == null)
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            return View();
        }

        public ActionResult Checkout()
        {
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            if (cart == null)
                return Redirect("/gio-hang.html");
            return View(cart);
        }

        public JsonResult GetAll()
        {
            if (Session[CommonConstants.SessionCart] == null)
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            return Json(new
            {
                data = cart,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Add(int productId, int quantity, string color)
        {
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            var product = _productDao.GetAllById(productId);
            if (cart == null)
            {
                cart = new List<ShoppingCartViewModel>();
            }
            if (quantity > product.Quantity || product.Quantity == null)
            {
                return Json(new
                {
                    status = false,
                    message = "Số lượng không đủ."
                });
            }

            if (cart.Any(x => x.ProductId == productId && x.Color == color))
            {
                foreach (var item in cart)
                {
                    if (item.ProductId == productId && item.Color == color)
                    {
                        item.Quantity += quantity;
                    }
                }
            }
            else
            {
                ShoppingCartViewModel newItem = new ShoppingCartViewModel();
                newItem.ProductId = productId;
                newItem.Product = Mapper.Map<Product, ProductViewModel>(product);
                newItem.Quantity = quantity;
                newItem.Color = color;
                cart.Add(newItem);
            }

            Session[CommonConstants.SessionCart] = cart;
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult DeleteItem(int productId, string Color)
        {
            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            if (cartSession != null)
            {
                cartSession.RemoveAll(x => x.ProductId == productId && x.Color == Color);
                Session[CommonConstants.SessionCart] = cartSession;
                return Json(new
                {
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
        }

        [HttpPost]
        public JsonResult Update(string cartData)
        {
            var cartViewModel = new JavaScriptSerializer().Deserialize<List<ShoppingCartViewModel>>(cartData);

            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];

            foreach (var item in cartSession)
            {
                foreach (var jitem in cartViewModel)
                {
                    if (item.ProductId == jitem.ProductId && item.Color == jitem.Color)
                    {
                        item.Quantity = jitem.Quantity;
                        item.Color = jitem.Color;
                    }
                }
            }

            Session[CommonConstants.SessionCart] = cartSession;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult LoadProvince()
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/assets/client/data/Provinces_Data.xml"));

            var xElements = xmlDoc.Element("Root").Elements("Item")
                .Where(x => x.Attribute("type").Value == "province");
            var list = new List<ProvinceModel>();
            ProvinceModel province = null;
            foreach (var item in xElements)
            {
                province = new ProvinceModel();
                province.ID = int.Parse(item.Attribute("id").Value);
                province.Name = item.Attribute("value").Value;
                list.Add(province);
            }
            return Json(new
            {
                data = list,
                status = true
            });
        }

        public JsonResult LoadDistrict(int provinceID)
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/assets/client/data/Provinces_Data.xml"));

            var xElement = xmlDoc.Element("Root").Elements("Item")
                .Single(x => x.Attribute("type").Value == "province" && int.Parse(x.Attribute("id").Value) == provinceID);

            var list = new List<DistrictModel>();
            DistrictModel district = null;
            foreach (var item in xElement.Elements("Item").Where(x => x.Attribute("type").Value == "district"))
            {
                district = new DistrictModel();
                district.ID = int.Parse(item.Attribute("id").Value);
                district.Name = item.Attribute("value").Value;
                district.ProvinceID = int.Parse(xElement.Attribute("id").Value);
                list.Add(district);
            }
            return Json(new
            {
                data = list,
                status = true
            });
        }

        public JsonResult GetUser()
        {
            var model = (AccountLogin)Session[CommonConstants.AccountSession];
            if (model == null)
            {
                return Json(new
                {
                    status = false
                });
            }
            return Json(new
            {
                data = model,
                status = true
            });
        }

        [HttpPost]
        public JsonResult CreateOrder(string orderViewModel)
        {
            var order = new JavaScriptSerializer().Deserialize<OrderViewModel>(orderViewModel);
            var orderDao = new OrderDao();
            var orderNew = new Order();
            orderNew.CreatedDate = DateTime.Now;
            orderNew.CustomerAddress = order.CustomerAddress;
            orderNew.CustomerMobile = order.CustomerMobile;
            orderNew.CustomerName = order.CustomerName;
            orderNew.CustomerEmail = order.CustomerEmail;
            orderNew.CustomerMessage = order.CustomerMessage;
            orderNew.PaymentMethod = order.PaymentMethod;
            orderNew.PaymentStatus = order.PaymentStatus;
            orderNew.Status = order.Status;
            orderDao.Insert(orderNew);
            var detailDao = new OrderDetailDao();
            var sessionCart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            var cart = new List<CartInsertViewModel>();
            foreach (var session in sessionCart)
            {
                if (cart.Any(x => x.ProductId == session.ProductId))
                {
                    foreach (var item in cart)
                    {
                        if (item.ProductId == session.ProductId)
                        {
                            item.Quantity += session.Quantity;
                            item.Note = item.Note + " " + session.Quantity + " " + "màu" + " " + session.Color + ";";
                        }
                    }
                }
                else
                {
                    CartInsertViewModel newItem = new CartInsertViewModel();
                    newItem.ProductId = session.ProductId;
                    newItem.Product = session.Product;
                    newItem.Quantity = session.Quantity;
                    newItem.Color = session.Color;
                    newItem.Note = session.Quantity + " " + "màu" + " " + session.Color + ";";
                    cart.Add(newItem);
                }
            }

            decimal total = 0;
            foreach (var item in cart)
            {
                var detail = new OrderDetail();
                detail.OrderID = orderNew.ID;
                detail.ProductID = item.ProductId;
                detail.Quantitty = item.Quantity;
                if (item.Product.PromotionPrice.HasValue)
                {
                    detail.Price = item.Product.PromotionPrice.Value;
                    total += (item.Product.PromotionPrice.GetValueOrDefault(0) * item.Quantity);
                }
                else
                {
                    detail.Price = item.Product.Price;
                    total += (item.Product.Price * item.Quantity);
                }
                detail.Note = item.Note;
                detailDao.Insert(detail);
            }

            string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/client/template/neworder.html"));
            content = content.Replace("{{CustomerName}}", order.CustomerName);
            content = content.Replace("{{Phone}}", order.CustomerMobile);
            content = content.Replace("{{Email}}", order.CustomerEmail);
            content = content.Replace("{{Address}}", order.CustomerAddress);
            content = content.Replace("{{Total}}", total.ToString("N0"));
            var adminEmail = ConfigHelper.GetByKey("AdminEmail");
            MailHelper.SendMail(order.CustomerEmail, "Đơn hàng mới từ ePhuKien", content);
            MailHelper.SendMail(adminEmail, "Đơn hàng mới từ ePhuKien", content);
            Session[CommonConstants.SessionCart] = null;

            return Json(new
            {
                status = true
            });
        }

        public JsonResult DeleteAll()
        {
            Session[CommonConstants.SessionCart] = null;
            return Json(new
            {
                status = true
            });
        }      
    }
}