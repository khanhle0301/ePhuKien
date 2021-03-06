﻿using System.Web.Mvc;
using System.Web.Routing;

namespace MyShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
             name: "Login",
             url: "dang-nhap.html",
             defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional },
             namespaces: new string[] { "MyShop.Controllers" }
         );

            routes.MapRoute(
          name: "Search",
          url: "tim-kiem.html",
          defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
          namespaces: new string[] { "MyShop.Controllers" }
      );

            routes.MapRoute(
           name: "Checkout",
           url: "thanh-toan.html",
           defaults: new { controller = "ShoppingCart", action = "Checkout", id = UrlParameter.Optional },
           namespaces: new string[] { "MyShop.Controllers" }
       );

            routes.MapRoute(
         name: "PaymentSuccess",
         url: "gui-don-hang-thanh-cong.html",
         defaults: new { controller = "ShoppingCart", action = "PaymentSuccess", id = UrlParameter.Optional },
         namespaces: new string[] { "MyShop.Controllers" }
     );

            routes.MapRoute(
            name: "ViewAll",
            url: "san-pham/all/",
            defaults: new { controller = "Product", action = "ViewAll", id = UrlParameter.Optional },
            namespaces: new string[] { "MyShop.Controllers" }
        );


            routes.MapRoute(
            name: "ViewNew",
            url: "san-pham/news/",
            defaults: new { controller = "Product", action = "ViewNew", id = UrlParameter.Optional },
            namespaces: new string[] { "MyShop.Controllers" }
        );

            routes.MapRoute(
          name: "ViewSale",
          url: "san-pham/khuyen-mai/",
          defaults: new { controller = "Product", action = "ViewSale", id = UrlParameter.Optional },
          namespaces: new string[] { "MyShop.Controllers" }
      );


            routes.MapRoute(
          name: "ViewPopular",
          url: "san-pham/duoc-xem-nhieu/",
          defaults: new { controller = "Product", action = "ViewPopular", id = UrlParameter.Optional },
          namespaces: new string[] { "MyShop.Controllers" }
      );

            routes.MapRoute(
        name: "ViewHot",
        url: "san-pham/noi-bat/",
        defaults: new { controller = "Product", action = "ViewHot", id = UrlParameter.Optional },
        namespaces: new string[] { "MyShop.Controllers" }
    );

            routes.MapRoute(
            name: "Account",
            url: "tai-khoan.html",
            defaults: new { controller = "Account", action = "Info", id = UrlParameter.Optional },
            namespaces: new string[] { "MyShop.Controllers" }
        );

            routes.MapRoute(
               name: "Contact",
               url: "trang/lien-he.html",
               defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "MyShop.Controllers" }
           );

            routes.MapRoute(
             name: "ShoppingCart",
             url: "gio-hang.html",
             defaults: new { controller = "ShoppingCart", action = "Index", id = UrlParameter.Optional },
             namespaces: new string[] { "MyShop.Controllers" }
         );

            routes.MapRoute(
           name: "Page",
           url: "trang/{alias}.html",
           defaults: new { controller = "Page", action = "Index", alias = UrlParameter.Optional },
           namespaces: new string[] { "MyShop.Controllers" }
       );
            routes.MapRoute(
               name: "Post",
               url: "bai-viet/",
               defaults: new { controller = "Post", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "MyShop.Controllers" }
           );

            routes.MapRoute(
               name: "Post Category",
               url: "bai-viet/{alias}/",
               defaults: new { controller = "Post", action = "Category", alias = UrlParameter.Optional },
                 namespaces: new string[] { "MyShop.Controllers" }
           );

            routes.MapRoute(
              name: "Post Tag",
              url: "bai-viet/tag/{tagId}/",
              defaults: new { controller = "Post", action = "ListByTag", tagId = UrlParameter.Optional },
                namespaces: new string[] { "MyShop.Controllers" }
          );
            routes.MapRoute(
             name: "Post Detail",
             url: "bai-viet/{catealias}/{alias}-{id}.html",
             defaults: new { controller = "Post", action = "Detail", id = UrlParameter.Optional },
               namespaces: new string[] { "MyShop.Controllers" }
         );

            routes.MapRoute(
               name: "Product Category",
               url: "san-pham/{alias}-{id}/",
               defaults: new { controller = "Product", action = "Index", alias = UrlParameter.Optional },
                namespaces: new string[] { "MyShop.Controllers" }
           );

            routes.MapRoute(
            name: "Product Tag",
            url: "san-pham/tag/{tagId}/",
            defaults: new { controller = "Product", action = "ListByTag", tagId = UrlParameter.Optional },
              namespaces: new string[] { "MyShop.Controllers" }
        );

            routes.MapRoute(
              name: "Product Detail",
              url: "san-pham/{catealias}/{alias}-{id}.html",
              defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
               namespaces: new string[] { "MyShop.Controllers" }
          );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                 namespaces: new string[] { "MyShop.Controllers" }
            );
        }
    }
}