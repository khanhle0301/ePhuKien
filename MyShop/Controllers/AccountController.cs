using Common;
using Model.Dao;
using Model.EF;
using MyShop.Common;
using MyShop.Models;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MyShop.Controllers
{
    public class AccountController : Controller
    {
        private UserDao _userDao = new UserDao();

        public ActionResult Index(string urlredirect)
        {
            if (Session[CommonConstants.AccountSession] != null)
                return Redirect("/");
            ViewBag.UrlRedirect = urlredirect;
            return View();
        }

        public ActionResult Info()
        {
            if (Session[CommonConstants.AccountSession] == null)
                return Redirect("/dang-nhap.html");
            return View();
        }

        [HttpPost]
        public JsonResult RecoverPassword(string email)
        {
            var account = _userDao.GetByEmail(email);
            if (account == null)
            {
                return Json(new
                {
                    status = false
                });
            }
            var newpass = new KeyGenerator().GetUniqueKey();
            var user = new User();
            user.UserName = account.UserName;
            user.Email = account.Email;
            user.Password = Encryptor.MD5Hash(newpass);
            _userDao.RecoverPassword(user);

            string content = System.IO.File.ReadAllText(Server.MapPath("/Assets/client/template/recover.html"));
            content = content.Replace("{{UserName}}", account.Name);
            content = content.Replace("{{NewPw}}", newpass);
            content = content.Replace("{{Link}}", ConfigHelper.GetByKey("CurrentLink") + "dang-nhap.html");
            MailHelper.SendMail(email, "[ePhuKien] - Đổi mật khẩu", content);

            return Json(new
            {
                status = true
            });
        }

        public ActionResult Logout()
        {
            Session[CommonConstants.AccountSession] = null;
            return Redirect("/");
        }

        [HttpPost]
        public JsonResult Login(string userName, string passWord)
        {
            Session[CommonConstants.AccountSession] = null;
            var result = _userDao.Login(userName, Encryptor.MD5Hash(passWord));
            if (result != 1)
            {
                return Json(new
                {
                    status = false,
                    message = "Đăng nhập không đúng."
                });
            }
            var user = _userDao.GetByUserName(userName);
            var accountSession = new AccountLogin();
            accountSession.UserName = user.UserName;
            accountSession.UserID = user.ID;
            accountSession.Name = user.Name;
            accountSession.Email = user.Email;
            accountSession.Phone = user.Phone;
            accountSession.Address = user.Address;
            Session.Add(CommonConstants.AccountSession, accountSession);
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult Register(string accountViewModel)
        {
            var user = new JavaScriptSerializer().Deserialize<UserViewModel>(accountViewModel);
            if (_userDao.UserNameExists(user.UserName))
            {
                return Json(new
                {
                    status = false,
                    message = "Tài khoản đã tồn tại."
                });
            }
            else if (_userDao.EmailExists(user.Email))
            {
                return Json(new
                {
                    status = false,
                    message = "Email đã tồn tại."
                });
            }

            User newUser = new User();
            newUser.ID = user.ID;
            newUser.UserName = user.UserName;
            newUser.Password = Encryptor.MD5Hash(user.Password);
            newUser.GroupID = user.GroupID;
            newUser.Name = user.Name;
            newUser.Address = user.Address;
            newUser.Email = user.Email;
            newUser.Phone = user.Phone;
            _userDao.Insert(newUser);

            return Json(new
            {
                status = true
            });
        }
    }
}