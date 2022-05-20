using Helpers;
using SpinWheel.DAL;
using SpinWheel.Filters;
using SpinWheel.Models;
using SpinWheel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;
using System.Threading.Tasks;

namespace SpinWheel.Controllers
{
    [RoutePrefix("account"), MemberFilter]
    public class UserController : Controller
    {
        public readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private int userId => Convert.ToInt32(RouteData.Values["UserId"]);

        [OverrideActionFilters, Route("dang-ky")]
        public ActionResult Register()
        {
            return View();
        }
        [Route("dang-ky")]
        [HttpPost, ValidateAntiForgeryToken, OverrideActionFilters]
        public ActionResult Register(RegisterViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var phoneVal = model.PhoneNumber.Trim();

                var checkUser = _unitOfWork.UserRepository.GetQuery(a => a.Username.Equals(model.Username)).SingleOrDefault();
                if (checkUser != null)
                {
                    ModelState.AddModelError("", @"Tên đăng nhập đã tồn tại!! Vui lòng nhập tên đăng nhập khác");
                }
                else
                {
                    var hashPass = HtmlHelpers.ComputeHash(model.Password, "SHA256", null);
                    var user = new User
                    {
                        Username = model.Username,
                        Password = hashPass,
                        Email = model.Email,
                        PhoneNumber = phoneVal,
                        Active = true,
                        TypeUser = 0,
                    };
                    _unitOfWork.UserRepository.Insert(user);
                    _unitOfWork.Save();
                    var userData = user.Id + "|" + user.Username + "|" + user.TypeUser;
                    var ticket = new FormsAuthenticationTicket(2, model.Username.ToLower(), DateTime.Now, DateTime.Now.AddDays(30), true,
                        userData, FormsAuthentication.FormsCookiePath);

                    var encTicket = FormsAuthentication.Encrypt(ticket);
                    // Create the cookie.
                    Response.Cookies.Add(new HttpCookie(".MEMBERAUTH", encTicket));
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "User");
                }
            }
            return View(model);
        }
        [OverrideActionFilters, Route("dang-nhap")]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost, Route("dang-nhap"), OverrideActionFilters]
        public ActionResult Login(UserLoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = _unitOfWork.UserRepository.Get(a => a.Username == model.Username && a.Active).SingleOrDefault();
                if (user == null || !HtmlHelpers.VerifyHash(model.Password, "SHA256", user.Password))
                {
                    ModelState.AddModelError("", @"Tên đăng nhập hoặc mật khẩu không chính xác.");
                    return View(model);
                }

                if (!user.Active)
                {
                    ModelState.AddModelError("", @"Tài khoản tạm thời bị khóa. Vui lòng liên hệ với quản trị viên.");
                    return View(model);
                }
                var userData = user.Id + "|" + user.Username + "|" + user.TypeUser;
                var ticket = new FormsAuthenticationTicket(2, model.Username.ToLower(), DateTime.Now, DateTime.Now.AddDays(30), true,
                    userData, FormsAuthentication.FormsCookiePath);

                var encTicket = FormsAuthentication.Encrypt(ticket);
                // Create the cookie.
                Response.Cookies.Add(new HttpCookie(".MEMBERAUTH", encTicket));
                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "User");
            }
            return View(model);
        }
        public RedirectToRouteResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }

        [Route("doi-mat-khau")]
        public ActionResult UserChangePassword(int result = 0)
        {
            ViewBag.Result = result;
            return View();
        }
        [HttpPost, Route("doi-mat-khau")]
        public ActionResult UserChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = Convert.ToInt32(RouteData.Values["UserId"]);
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

        [OverrideActionFilters]
        public JsonResult CheckPhone(string phoneNumber)
        {
            var user = _unitOfWork.UserRepository.GetQuery(a => a.PhoneNumber == phoneNumber).SingleOrDefault();

            if (user == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json("Số điện thoại đã được sử dụng, vui lòng thử lại", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
         {
            var model = new HomeUserViewModel
            {
                Events = _unitOfWork.EventRepository.GetQuery(a => a.UserId == userId),
                Awards = _unitOfWork.AwardRepository.GetQuery(a => a.Event.UserId == userId),
                ListClientAwards = _unitOfWork.ListClientAwardRepository.GetQuery(a => a.Award.Event.UserId == userId)
            };
            return View(model);
        }
    }
}