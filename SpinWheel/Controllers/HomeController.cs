using SpinWheel.DAL;
using SpinWheel.Models;
using SpinWheel.ViewModel;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;

namespace SpinWheel.Controllers
{
    public class HomeController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private static string Email => WebConfigurationManager.AppSettings["email"];
        private static string Password => WebConfigurationManager.AppSettings["password"];
        public ConfigSite ConfigSite => (ConfigSite)HttpContext.Application["ConfigSite"];

        #region Home
        public ActionResult Index()
        {
            var model = new HomeViewModel
            {
                Clients = _unitOfWork.ClientRepository.Get(),
            };
            return View(model);
        }
        [ChildActionOnly]
        public PartialViewResult Header()
        {
            return PartialView();
        }
        [ChildActionOnly]
        public PartialViewResult Footer()
        {
            return PartialView();
        }
        public JsonResult InfoForm(Client model, FormCollection fc)
        {
            var fullName = fc["fullname"];
            var phone = fc["phone"];
            var prize = fc["prize"];

            model.Mobile = phone;
            model.Fullname = fullName;
            model.AwardName = prize;

            var award = _unitOfWork.AwardRepository.Get(p => p.AwardName == prize).FirstOrDefault();
            award.Clients.Add(model);
            _unitOfWork.ClientRepository.Insert(model);
            _unitOfWork.Save();
            return Json(new { status = true });
        }
        public JsonResult GetAwardData()
        {
            var awards = _unitOfWork.AwardRepository.GetQuery().Select(a => new
            {
                percent = a.Percent,
                name = a.AwardName,
                bgColor = a.BgColor,
                txtColor = a.TextColor
            });
            return Json(awards, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetClientData()
        {
            var clients = _unitOfWork.ClientRepository.Get();
            return Json(clients, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEventData()
        {
            var events = _unitOfWork.EventRepository.Get();
            return Json(events, JsonRequestBehavior.AllowGet);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}