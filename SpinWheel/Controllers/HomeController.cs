using SpinWheel.DAL;
using SpinWheel.Models;
using SpinWheel.ViewModel;
using System;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;

namespace SpinWheel.Controllers
{
    public class HomeController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        public ConfigSite ConfigSite => (ConfigSite)HttpContext.Application["ConfigSite"];

        #region Home
        public ActionResult Index()
        {
            var model = new HomeViewModel
            {
                Events = _unitOfWork.EventRepository.GetQuery(a => a.Active, o => o.OrderBy(a => a.Sort)),
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
        [HttpPost]
        public JsonResult InfoForm(Client model, FormCollection fc)
        {
            var fullName = fc["fullname"];
            var phone = fc["phone"];
            var prize = fc["prize"];
            var awardId = Convert.ToInt32(fc["awardId"]);
            var award = _unitOfWork.AwardRepository.GetById(awardId);
            model.Mobile = phone;
            model.Fullname = fullName;
            award.TotalWin += 1;
            _unitOfWork.ClientRepository.Insert(model);
            var listClientAward = new ListClientAward
            {
                AwardId = awardId,
                ClientId = model.Id
            };
            var client = _unitOfWork.ClientRepository.Get(a => a.Mobile == phone).FirstOrDefault();
            if (client == null)
            {
                _unitOfWork.ClientRepository.Insert(model);
            }
            else
            {

                client.CheckDate = DateTime.Now;
                _unitOfWork.ClientRepository.Update(client);
            }
            _unitOfWork.ListClientAwardRepository.Insert(listClientAward);
            _unitOfWork.Save();
            return Json(new { status = true });
        }
        [Route("{url:regex(^(?!.*(user|admin|vcms|uploader|article|banner|contact|productvcms)).*$)}")]
        public ActionResult Event(string url)
        {
            var events = _unitOfWork.EventRepository.GetQuery(a => a.Active && a.Url == url).FirstOrDefault();
            if (events == null)
            {
                return RedirectToAction("Index");
            }
            var award = _unitOfWork.AwardRepository.GetQuery(a => a.EventId == events.Id);
            var model = new EventViewModel
            {
                Event = events,
                Awards = award
            };
            return View(model);
        }
        public JsonResult GetAwardData(string url)
        {
            var events = _unitOfWork.EventRepository.GetQuery(a => a.Active && a.Url == url).FirstOrDefault();
            var awards = _unitOfWork.AwardRepository.GetQuery(a => a.EventId == events.Id).Select(a => new
            {
                percent = a.Percent,
                name = a.AwardName,
                bgColor = a.BgColor,
                txtColor = a.TextColor,
                bgPc = a.Event.BgPC,
                bgMobile = a.Event.BgMobile,
                quantity = a.Quantity,
                totalWin = a.TotalWin,
                id = a.Id,
            });
            return Json(awards, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetClientData(string phone, int eventId)
        {
            var isPost = 1;
            var now = DateTime.Now;
            var client = _unitOfWork.ClientRepository.Get(p => p.Mobile == phone).FirstOrDefault();

            // so sánh ngày quay gần nhất với now
            if (client != null)
            {
                if (DateTime.Compare(client.CheckDate, now) <= 0 && client.CheckDate.Day == now.Day)
                {
                    isPost = 0;
                }
            }
            return Json(isPost, JsonRequestBehavior.AllowGet);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}