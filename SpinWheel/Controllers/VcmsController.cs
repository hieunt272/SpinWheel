﻿using Helpers;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SpinWheel.DAL;
using SpinWheel.Models;
using SpinWheel.ViewModel;
using PagedList;

namespace SpinWheel.Controllers
{
    [Authorize]
    public class VcmsController : Controller
    {
        public readonly UnitOfWork _unitOfWork = new UnitOfWork();

        #region Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(AdminLoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var admin = _unitOfWork.AdminRepository.Get(a => a.Username == model.Username && a.Active).SingleOrDefault();

                if (admin != null && HtmlHelpers.VerifyHash(model.Password, "SHA256", admin.Password))
                {
                    var ticket = new FormsAuthenticationTicket(1, model.Username.ToLower(), DateTime.Now, DateTime.Now.AddDays(30), true,
                        admin.ToString(), FormsAuthentication.FormsCookiePath);

                    var encTicket = FormsAuthentication.Encrypt(ticket);
                    // Create the cookie.
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Vcms");
                }
                ModelState.AddModelError("", @"Tên đăng nhập  hoặc mật khẩu không chính xác.");
            }
            return View(model);
        }
        public RedirectToRouteResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Vcms");
        }
        #endregion

        #region Admin
        public ActionResult Index()
        {
            var model = new InfoAdminViewModel
            {
                Admins = _unitOfWork.AdminRepository.Get(),
                Events = _unitOfWork.EventRepository.Get(),
                Users = _unitOfWork.UserRepository.Get(),
                Banners = _unitOfWork.BannerRepository.Get(),
                Articles = _unitOfWork.ArticleRepository.Get(),
                Contacts = _unitOfWork.ContactRepository.Get(),
            };
            return View(model);
        }
        [ChildActionOnly]
        public PartialViewResult ListAdmin()
        {
            var admins = _unitOfWork.AdminRepository.Get();
            return PartialView("ListAdmin", admins);
        }
        public ActionResult CreateAdmin(string result = "")
        {
            ViewBag.Result = result;
            return View();
        }
        [HttpPost]
        public ActionResult CreateAdmin(Admin model)
        {
            if (ModelState.IsValid)
            {
                var admin = _unitOfWork.AdminRepository.GetQuery(a => a.Username.Equals(model.Username)).SingleOrDefault();
                if (admin != null)
                {
                    ModelState.AddModelError("", @"Tên đăng nhập này có rồi");
                }
                else
                {
                    var hashPass = HtmlHelpers.ComputeHash(model.Password, "SHA256", null);
                    _unitOfWork.AdminRepository.Insert(new Admin { Username = model.Username, Password = hashPass, Active = model.Active, Role = model.Role });
                    _unitOfWork.Save();
                    return RedirectToAction("CreateAdmin", new { result = "success" });
                }
            }
            return View();
        }
        public ActionResult EditAdmin(int adminId = 0)
        {
            var admin = _unitOfWork.AdminRepository.GetById(adminId);
            if (admin == null)
            {
                return RedirectToAction("CreateAdmin");
            }

            var model = new EditAdminViewModel
            {
                Id = admin.Id,
                Username = admin.Username,
                Active = admin.Active,
                Role = admin.Role
            };

            return View(model);
        }
        [HttpPost]
        public ActionResult EditAdmin(EditAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = _unitOfWork.AdminRepository.GetById(model.Id);
                if (admin == null)
                {
                    return RedirectToAction("CreateAdmin");
                }
                if (admin.Username != model.Username)
                {
                    var exists = _unitOfWork.AdminRepository.GetQuery(a => a.Username.Equals(model.Username)).SingleOrDefault();
                    if (exists != null)
                    {
                        ModelState.AddModelError("", @"Tên đăng nhập này có rồi");
                        return View(model);
                    }
                    admin.Username = model.Username;
                }
                admin.Role = model.Role;
                admin.Active = model.Active;
                if (model.Password != null)
                {
                    admin.Password = HtmlHelpers.ComputeHash(model.Password, "SHA256", null);
                }
                _unitOfWork.Save();
                return RedirectToAction("CreateAdmin", new { result = "update" });
            }
            return View(model);
        }
        public bool DeleteAdmin(string username)
        {
            var admin = _unitOfWork.AdminRepository.GetQuery(a => a.Username.Equals(username)).SingleOrDefault();
            if (admin == null)
            {
                return false;
            }
            if (username == "admin")
            {
                return false;
            }
            _unitOfWork.AdminRepository.Delete(admin);
            _unitOfWork.Save();
            return true;
        }
        public ActionResult ChangePassword(int result = 0)
        {
            ViewBag.Result = result;
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = _unitOfWork.AdminRepository.GetQuery(a => a.Username.Equals(User.Identity.Name,
                StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                if (admin == null)
                {
                    return HttpNotFound();
                }
                if (HtmlHelpers.VerifyHash(model.OldPassword, "SHA256", admin.Password))
                {
                    admin.Password = HtmlHelpers.ComputeHash(model.Password, "SHA256", null);
                    _unitOfWork.Save();
                    return RedirectToAction("ChangePassword", new { result = 1 });
                }
                else
                {
                    ModelState.AddModelError("", @"Mật khẩu hiện tại không đúng!");
                    return View();
                }
            }
            return View(model);
        }
        #endregion

        #region ConfigSite
        public ActionResult ConfigSite(string result = "")
        {
            ViewBag.Result = result;
            var config = _unitOfWork.ConfigSiteRepository.Get().FirstOrDefault();
            return View(config);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ConfigSite(ConfigSite model, FormCollection fc)
        {
            var config = _unitOfWork.ConfigSiteRepository.Get().FirstOrDefault();
            if (config == null)
            {
                _unitOfWork.ConfigSiteRepository.Insert(model);
            }
            else
            {
                config.Facebook = model.Facebook;
                config.GoogleMap = model.GoogleMap;
                config.Youtube = model.Youtube;
                config.Twitter = model.Twitter;
                config.Instagram = model.Instagram;
                config.Title = model.Title;
                config.Description = model.Description;
                config.GoogleAnalytics = model.GoogleAnalytics;
                config.Hotline = model.Hotline;
                config.Email = model.Email;
                config.LiveChat = model.LiveChat;
                config.Place = model.Place;
                config.AboutText = model.AboutText;
                config.AboutUrl = model.AboutUrl;
                config.InfoFooter = model.InfoFooter;
                config.InfoContact = model.InfoContact;

                var file = Request.Files["Image"];
                if (file != null && file.ContentLength > 0)
                {
                    if (!HtmlHelpers.CheckFileExt(file.FileName, "jpg|jpeg|png|gif"))
                    {
                        ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg");
                        return View(config);
                    }

                    if (file.ContentLength > 4000 * 1024)
                    {
                        ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại");
                        return View(config);
                    }

                    var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file.FileName);
                    var imgPath = "/images/configs/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    config.Image = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;
                    file.SaveAs(Server.MapPath(Path.Combine(imgPath, imgFileName)));
                }

                var file1 = Request.Files["AboutImage"];
                if (file1 != null && file1.ContentLength > 0)
                {
                    if (!HtmlHelpers.CheckFileExt(file1.FileName, "jpg|jpeg|png|gif"))
                    {
                        ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg");
                        return View(config);
                    }

                    if (file1.ContentLength > 4000 * 1024)
                    {
                        ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại");
                        return View(config);
                    }

                    var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file1.FileName);
                    var imgPath = "/images/configs/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    config.AboutImage = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;
                    file1.SaveAs(Server.MapPath(Path.Combine(imgPath, imgFileName)));
                }
                _unitOfWork.Save();
                HttpContext.Application["ConfigSite"] = config;
                return RedirectToAction("ConfigSite", "Vcms", new { result = "success" });
            }
            return View("ConfigSite", model);
        }
        #endregion

        #region User
        public ActionResult ListUser(int? page, string name, string typeUser, string result)
        {
            ViewBag.Result = result;
            var pageNumber = page ?? 1;
            var pageSize = 15;
            var users = _unitOfWork.UserRepository.Get(orderBy: o => o.OrderByDescending(a => a.CreateDate));
            if (!string.IsNullOrEmpty(name))
            {
                users = users.Where(a => a.Username.Contains(name.ToLower()));
            }
            if (!string.IsNullOrEmpty(typeUser))
            {
                switch (typeUser)
                {
                    case "0":
                        users = users.Where(a => a.TypeUser == TypeUser.Normal);
                        break;
                    case "1":
                        users = users.Where(a => a.TypeUser == TypeUser.Premium);
                        break;
                    case "2":
                        users = users.Where(a => a.TypeUser == TypeUser.Unlimited);
                        break;
                }

            }
            var model = new ListUserViewModel
            {
                Users = users.ToPagedList(pageNumber, pageSize),
                Name = name,
                TypeUser = typeUser,
            };
            return View(model);
        }
        public ActionResult CreateUser(string result = "")
        {
            ViewBag.Result = result;
            return View();
        }
        [HttpPost]
        public ActionResult CreateUser(User model)
        {
            if (ModelState.IsValid)
            {
                var user = _unitOfWork.UserRepository.GetQuery(a => a.Username.Equals(model.Username)).SingleOrDefault();
                if (user != null)
                {
                    ModelState.AddModelError("", @"Tên đăng nhập đã tồn tại!! Vui lòng nhập tên đăng khác");
                }
                else
                {
                    var hashPass = HtmlHelpers.ComputeHash(model.Password, "SHA256", null);
                    _unitOfWork.UserRepository.Insert(new User { Username = model.Username, Password = hashPass, Active = model.Active, TypeUser = model.TypeUser });
                    _unitOfWork.Save();
                    return RedirectToAction("ListUser", new { result = "success" });
                }
            }
            return View();
        }
        public ActionResult EditUser(int userId = 0)
        {
            var user = _unitOfWork.UserRepository.GetById(userId);
            if (user == null)
            {
                return RedirectToAction("CreateUser");
            }

            var model = new EditAdminViewModel
            {
                Id = user.Id,
                Username = user.Username,
                Active = user.Active,
                TypeUser = user.TypeUser,
            };

            return View(model);
        }
        [HttpPost]
        public ActionResult EditUser(EditAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _unitOfWork.UserRepository.GetById(model.Id);
                if (user == null)
                {
                    return RedirectToAction("CreateUser");
                }
                if (user.Username != model.Username)
                {
                    var exists = _unitOfWork.UserRepository.GetQuery(a => a.Username.Equals(model.Username)).SingleOrDefault();
                    if (exists != null)
                    {
                        ModelState.AddModelError("", @"Tên đăng nhập này có rồi");
                        return View(model);
                    }
                    user.Username = model.Username;
                }
                user.TypeUser = model.TypeUser;
                user.Active = model.Active;
                if (model.Password != null)
                {
                    user.Password = HtmlHelpers.ComputeHash(model.Password, "SHA256", null);
                }
                _unitOfWork.Save();
                return RedirectToAction("ListUser", new { result = "update" });
            }
            return View(model);
        }
        public bool DeleteUser(int userId)
        {
            var user = _unitOfWork.UserRepository.GetById(userId);
            if (user == null)
            {
                return false;
            }
            _unitOfWork.UserRepository.Delete(user);
            _unitOfWork.Save();
            return true;
        }
        public ActionResult UserChangePassword(int result = 0)
        {
            ViewBag.Result = result;
            return View();
        }
        [HttpPost]
        public ActionResult UserChangePassword(ChangePasswordModel model, int userId)
        {
            if (ModelState.IsValid)
            {
                var user = _unitOfWork.UserRepository.GetById(userId);
                if (user == null)
                {
                    return RedirectToAction("ListUser", "Vcms");
                }
                if (HtmlHelpers.VerifyHash(model.OldPassword, "SHA256", user.Password))
                {
                    user.Password = HtmlHelpers.ComputeHash(model.Password, "SHA256", null);
                    _unitOfWork.Save();
                    return RedirectToAction("UserChangePassword", new { result = 1 });
                }
                else
                {
                    ModelState.AddModelError("", @"Mật khẩu hiện tại không đúng!");
                    return View();
                }
            }
            return View(model);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}