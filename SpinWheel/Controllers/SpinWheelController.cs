using SpinWheel.DAL;
using SpinWheel.Models;
using SpinWheel.ViewModel;
using Helpers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using SpinWheel.Filters;

namespace SpinWheel.Controllers
{
    [MemberFilter]
    public class SpinWheelController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private IEnumerable<Event> Events => _unitOfWork.EventRepository.Get(orderBy: o => o.OrderBy(a => a.Sort));
        private int userId => Convert.ToInt32(RouteData.Values["UserId"]);

        #region Event
        [ChildActionOnly]
        public ActionResult ListEvent()
        {
            var events = _unitOfWork.EventRepository.Get(a => a.UserId == userId, o => o.OrderBy(a => a.Sort));
            return PartialView(events);
        }
        public ActionResult Event(string result = "")
        {
            ViewBag.Result = result;
            var events = new Event { Sort = 1, Active = true };
            var isExist = false;
            var user = _unitOfWork.UserRepository.GetById(userId);
            var evt = _unitOfWork.EventRepository.GetQuery(p => p.UserId == userId).Count();
            if (user.TypeUser == TypeUser.Normal)
            {
                if (evt < 1)
                {
                    isExist = true;
                }
            }
            if (user.TypeUser == TypeUser.Premium)
            {
                if (evt <= 5)
                {
                    isExist = true;
                }
            }
            ViewBag.CheckTypeUser = isExist;
            return View();
        }
        [HttpPost]
        public ActionResult Event(Event model)
        {
            if (ModelState.IsValid)
            {
                var isPost = true;
                var file = Request.Files["BgPC"];
                if (file != null && file.ContentLength > 0)
                {
                    if (!HtmlHelpers.CheckFileExt(file.FileName, "jpg|jpeg|png|gif"))
                    {
                        ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg");
                        isPost = false;
                    }
                    else
                    {

                        if (file.ContentLength > 4000 * 1024)
                        {
                            ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại");
                            isPost = false;
                        }
                        else
                        {
                            var imgPath = "/images/events/" + DateTime.Now.ToString("yyyy/MM/dd");
                            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                            var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file.FileName);

                            model.BgPC = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                            var newImage = Image.FromStream(file.InputStream);
                            var fixSizeImage = HtmlHelpers.FixedSize(newImage, 800, 600, false);
                            HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);
                        }
                    }
                }

                var file1 = Request.Files["BgMobile"];
                if (file1 != null && file1.ContentLength > 0)
                {
                    if (file1.ContentType != "image/jpeg" & file1.ContentType != "image/png" && file1.ContentType != "image/gif")
                    {
                        ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg");
                        isPost = false;
                    }
                    else
                    {
                        if (file1.ContentLength > 4000 * 1024)
                        {
                            ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại");
                            isPost = false;
                        }
                        else
                        {
                            var imgPath = "/images/events/" + DateTime.Now.ToString("yyyy/MM/dd");
                            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                            var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file1.FileName);

                            model.BgMobile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                            var newImage = Image.FromStream(file1.InputStream);
                            var fixSizeImage = HtmlHelpers.FixedSize(newImage, 600, 600, false);
                            HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);
                        }
                    }
                }
                if (isPost)
                {
                    model.UserId = userId;
                    model.Url = HtmlHelpers.ConvertToUnSign(null, model.Url ?? model.EventName);
                    var countUrl = _unitOfWork.EventRepository.GetQuery(a => a.Url == model.Url).Count();
                    if (countUrl > 1)
                    {
                        model.Url += "-" + DateTime.Now.Millisecond;
                    }
                    _unitOfWork.EventRepository.Insert(model);
                    _unitOfWork.Save();

                    return RedirectToAction("Event", new { result = "success" });
                }
            }
            return View(model);
        }
        public ActionResult UpdateEvent(int eventId = 0)
        {
            var events = _unitOfWork.EventRepository.GetById(eventId);
            if (events == null)
            {
                return RedirectToAction("Event");
            }
            return View(events);
        }
        [HttpPost]
        public ActionResult UpdateEvent(Event model, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                var isPost = true;
                var file = Request.Files["BgPC"];
                if (file != null && file.ContentLength > 0)
                {
                    if (file.ContentType != "image/jpeg" & file.ContentType != "image/png" && file.ContentType != "image/gif")
                    {
                        ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg");
                        isPost = false;
                    }
                    else
                    {
                        if (file.ContentLength > 4000 * 1024)
                        {
                            ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại");
                            isPost = false;
                        }
                        else
                        {
                            var imgPath = "/images/events/" + DateTime.Now.ToString("yyyy/MM/dd");
                            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                            var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file.FileName);

                            if (System.IO.File.Exists(Server.MapPath("/images/events/" + model.BgPC)))
                            {
                                System.IO.File.Delete(Server.MapPath("/images/events/" + model.BgPC));
                            }
                            model.BgPC = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                            var newImage = Image.FromStream(file.InputStream);
                            var fixSizeImage = HtmlHelpers.FixedSize(newImage, 600, 600, false);
                            HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);
                        }
                    }
                }
                else
                {
                    model.BgPC = fc["CurrentFile"];
                }

                var file1 = Request.Files["BgMobile"];
                if (file1 != null && file1.ContentLength > 0)
                {
                    if (file1.ContentType != "image/jpeg" & file1.ContentType != "image/png" && file1.ContentType != "image/gif")
                    {
                        ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg");
                        isPost = false;
                    }
                    else
                    {
                        if (file1.ContentLength > 4000 * 1024)
                        {
                            ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại");
                            isPost = false;
                        }
                        else
                        {
                            var imgPath = "/images/events/" + DateTime.Now.ToString("yyyy/MM/dd");
                            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                            var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file1.FileName);

                            if (System.IO.File.Exists(Server.MapPath("/images/events/" + model.BgMobile)))
                            {
                                System.IO.File.Delete(Server.MapPath("/images/events/" + model.BgMobile));
                            }
                            model.BgMobile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                            var newImage = Image.FromStream(file1.InputStream);
                            var fixSizeImage = HtmlHelpers.FixedSize(newImage, 600, 600, false);
                            HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);
                        }
                    }
                }
                else
                {
                    model.BgMobile = fc["CurrentFile1"];
                }
                if (isPost)
                {
                    model.Url = HtmlHelpers.ConvertToUnSign(null, model.Url ?? model.EventName);
                    _unitOfWork.EventRepository.Update(model);
                    _unitOfWork.Save();

                    var count = _unitOfWork.EventRepository.GetQuery(a => a.Url == model.Url).Count();
                    if (count > 1)
                    {
                        model.Url += "-" + model.Id;
                        _unitOfWork.Save();
                    }

                    return RedirectToAction("Event", new { result = "update" });
                }
            }
            return View(model);
        }
        [HttpPost]
        public bool DeleteEvent(int eventId = 0)
        {
            var events = _unitOfWork.EventRepository.GetById(eventId);
            if (events == null)
            {
                return false;
            }
            _unitOfWork.EventRepository.Delete(events);
            _unitOfWork.Save();
            return true;
        }
        public bool UpdateListEvent(int sort = 1, bool active = false, int eventId = 0)
        {
            var events = _unitOfWork.EventRepository.GetById(eventId);
            if (events == null)
            {
                return false;
            }
            events.Sort = sort;
            events.Active = active;

            _unitOfWork.Save();
            return true;
        }
        #endregion

        #region Award
        public ActionResult ListAward(int? page, int? eventId, string name, string result = "")
        {
            ViewBag.Result = result;
            var pageNumber = page ?? 1;
            const int pageSize = 15;
            var events = _unitOfWork.EventRepository.Get(a => a.UserId == userId, o => o.OrderBy(a => a.Sort));
            var awards = _unitOfWork.AwardRepository.GetQuery(a => a.Event.UserId == userId, l => l.OrderBy(a => a.Id));

            if (!string.IsNullOrEmpty(name))
            {
                awards = awards.Where(l => l.AwardName.ToLower().Contains(name.ToLower()));
            }
            if (eventId > 0)
            {
                awards = awards.Where(a => a.EventId == eventId);
            }

            var model = new ListAwardViewModel
            {
                EventSelectList = new SelectList(Events.Where(a => a.UserId == userId), "Id", "EventName"),
                Events = events,
                Awards = awards.ToPagedList(pageNumber, pageSize),
                Name = name,
                EventId = eventId,
                TotalAward = awards.Count(),
            };
            return View(model);
        }
        public ActionResult Award()
        {
            var model = new InsertAwardViewModel
            {
                Events = Events.Where(a => a.UserId == userId),
            };
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Award(InsertAwardViewModel model, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                var awardName = fc.GetValues("Award.AwardName");
                var bgColor = fc.GetValues("Award.BgColor");
                var textColor = fc.GetValues("Award.TextColor");
                var percent = fc.GetValues("Award.Percent");
                var quantity = fc.GetValues("Award.Quantity");
                var limited = fc.GetValues("Award.Limited");
                for (var i = 0; i < awardName.Length; i++)
                {
                    if (!string.IsNullOrEmpty(awardName[i]))
                    {
                        var award = new Award
                        {
                            EventId = Convert.ToInt32(fc["EventId"]),
                            AwardName = awardName[i],
                            BgColor = bgColor[i],
                            TextColor = textColor[i],
                            Percent = percent[i],
                            Quantity = quantity[i],
                            Limited = Convert.ToBoolean(limited[i]),
                            Sort = i
                        };
                        _unitOfWork.AwardRepository.Insert(award);
                        _unitOfWork.Save();
                    }
                }
                return RedirectToAction("ListAward", new { result = "success" });
            }
            return View(model);
        }
        public ActionResult UpdateAward(int awardId = 0)
        {
            var award = _unitOfWork.AwardRepository.GetById(awardId);
            if (award == null)
            {
                return RedirectToAction("ListAward");
            }
            var model = new InsertAwardViewModel
            {
                Award = award,
                Events = Events.Where(a => a.UserId == userId),
            };
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateAward(InsertAwardViewModel model, FormCollection fc)
        {
            var award = _unitOfWork.AwardRepository.GetById(model.Award.Id);
            if (award == null)
            {
                return RedirectToAction("ListAward");
            }
            if (ModelState.IsValid)
            {
                award.EventId = Convert.ToInt32(fc["EventId"]);
                award.AwardName = model.Award.AwardName;
                award.TextColor = model.Award.TextColor;
                award.BgColor = model.Award.BgColor;
                award.Percent = model.Award.Percent;
                award.Quantity = model.Award.Quantity;
                award.Limited = model.Award.Limited;

                _unitOfWork.Save();
                return RedirectToAction("ListAward", new { result = "update" });
            }
            model.Events = Events;
            return View(model);
        }
        [HttpPost]
        public bool DeleteAward(int awardId = 0)
        {
            var award = _unitOfWork.AwardRepository.GetById(awardId);
            if (award == null)
            {
                return false;
            }
            _unitOfWork.AwardRepository.Delete(award);
            _unitOfWork.Save();
            return true;
        }
        #endregion

        #region Client
        public ActionResult ListClient(int? page, string name)
        {
            var pageNumber = page ?? 1;
            const int pageSize = 15;
            var clients = _unitOfWork.ClientRepository.GetQuery(c => c.ListClientAwards.Any(a => a.Award.Event.UserId == userId), o => o.OrderByDescending(a => a.CreateDate));

            if (!string.IsNullOrEmpty(name))
            {
                clients = clients.Where(l => l.Fullname.ToLower().Contains(name.ToLower()));
            }
            var model = new ListClientViewModel
            {
                Clients = clients.ToPagedList(pageNumber, pageSize),
                Name = name
            };
            return View(model);
        }
        public bool DeleteClient(int clientId = 0)
        {
            var client = _unitOfWork.ClientRepository.GetById(clientId);
            if (client == null)
            {
                return false;
            }
            _unitOfWork.ClientRepository.Delete(clientId);
            _unitOfWork.Save();
            return true;
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}