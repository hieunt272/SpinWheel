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
using System.Data.Entity;
using System.Globalization;

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
            return View(events);
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
                    if (file.ContentType != "image/jpeg" & file.ContentType != "image/png" && file.ContentType != "image/gif")
                    {
                        ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg");
                    }
                    else
                    {
                        if (file.ContentLength > 4000 * 1024)
                        {
                            ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại");
                        }
                        else
                        {
                            var imgPath = "/images/events/" + DateTime.Now.ToString("yyyy/MM/dd");
                            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                            var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file.FileName);

                            model.BgPC = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                            var newImage = Image.FromStream(file.InputStream);
                            var fixSizeImage = HtmlHelpers.FixedSize(newImage, 600, 600, false);
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
                    }
                    else
                    {
                        if (file1.ContentLength > 4000 * 1024)
                        {
                            ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại");
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
                    }
                    else
                    {
                        if (file.ContentLength > 4000 * 1024)
                        {
                            ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại");
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
                    }
                    else
                    {
                        if (file1.ContentLength > 4000 * 1024)
                        {
                            ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại");
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
                    model.UserId = userId;
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
        public ActionResult ListAward(int? page, int eventId, string name, string result = "")
        {
            ViewBag.Result = result;
            var pageNumber = page ?? 1;
            const int pageSize = 12;

            var awards = _unitOfWork.AwardRepository.Get(a => a.EventId == eventId);

            if (!string.IsNullOrEmpty(name))
            {
                awards = awards.Where(l => l.AwardName.ToLower().Contains(name.ToLower()));
            }

            var model = new ListAwardViewModel
            {
                Events = Events.Where(a => a.UserId == userId),
                EventId = eventId,
                Awards = awards.ToPagedList(pageNumber, pageSize),
                Name = name,
                TotalAward = awards.Count(),
            };
            return View(model);
        }
        public ActionResult Award(int eventId)
        {
            var awards = _unitOfWork.AwardRepository.Get(a => a.EventId == eventId);
            var results = awards.GroupBy(
                a => a.Id,
                a => a.EventId,
                (key, g) => new { AwardId = key, EventId = g.ToList() });
            var model = new InsertAwardViewModel
            {
                Events = Events.Where(a => a.UserId == userId),
                Awards = awards,
                TotalAward = awards.Count(),
                EventId = eventId,

            };
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Award(InsertAwardViewModel model, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                var awardName = fc.GetValues("AwardName_insert");
                var bgColor = fc.GetValues("BgColor_insert");
                var textColor = fc.GetValues("TextColor_insert");
                var percent = fc.GetValues("Percent_insert");
                var quantity = fc.GetValues("Quantity_insert");
                var limited = fc.GetValues("Limited_insert");

                var awardId = fc.GetValues("Award_id");
                var awardNameUpdate = fc.GetValues("AwardName_update");
                var bgColorUpdate = fc.GetValues("BgColor_update");
                var textColorUpdate = fc.GetValues("TextColor_update");
                var percentUpdate = fc.GetValues("Percent_update");
                var quantityUpdate = fc.GetValues("Quantity_update");
                var limitedUpdate = fc.GetValues("item.Limited");

                var events = Convert.ToInt32(fc["EventId"]);
                var awards = _unitOfWork.AwardRepository.Get(a => a.EventId == events);
                var awardCount = 12 - awards.Count();

                if (awards.Count() <= 0)
                {
                    for (var i = 0; i < 12; i++)
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
                        }
                    }
                }
                else
                {
                    for (var i = 0; i < awards.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(awardNameUpdate[i]))
                        {
                            var _awardId = Convert.ToInt32(awardId[i]);
                            var award = _unitOfWork.AwardRepository.GetById(_awardId);

                            award.EventId = Convert.ToInt32(fc["EventId"]);
                            award.AwardName = awardNameUpdate[i];
                            award.BgColor = bgColorUpdate[i];
                            award.TextColor = textColorUpdate[i];
                            award.Percent = percentUpdate[i];
                            award.Quantity = quantityUpdate[i];
                            award.Limited = Convert.ToBoolean(limitedUpdate[i]);
                            award.Sort = i;
                            _unitOfWork.Save();
                        }
                    }
                    for (var i = 0; i < awardCount; i++)
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
                        }
                    }
                }
                _unitOfWork.Save();
                return RedirectToAction("ListAward", new { result = "success", eventId = Convert.ToInt32(fc["EventId"]) });
            }
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
        public ActionResult ListClient(int? page, string name, string startDate, string endDate)
        {
            var pageNumber = page ?? 1;
            const int pageSize = 15;
            var clients = _unitOfWork.ClientRepository.GetQuery(c => c.ListClientAwards.Any(a => a.Award.Event.UserId == userId), o => o.OrderByDescending(a => a.CreateDate));
            var listClientAwards = _unitOfWork.ListClientAwardRepository.GetQuery(a => a.Award.Event.UserId == userId, o => o.OrderByDescending(a => a.Id));
            if (startDate != null && endDate != null)
            {
                if (DateTime.TryParse(startDate, new CultureInfo("vi-VN"), DateTimeStyles.None, out var start))
                {
                    listClientAwards = listClientAwards.Where(a => DbFunctions.TruncateTime(a.CreateDate) >= DbFunctions.TruncateTime(start));
                }
                if (DateTime.TryParse(endDate, new CultureInfo("vi-VN"), DateTimeStyles.None, out var end))
                {
                    listClientAwards = listClientAwards.Where(a => DbFunctions.TruncateTime(a.CreateDate) <= DbFunctions.TruncateTime(end));
                }
            }

            if (!string.IsNullOrEmpty(name))
            {
                listClientAwards = listClientAwards.Where(l => l.Award.AwardName.ToLower().Contains(name.ToLower()));
            }
            var model = new ListClientViewModel
            {
                Clients = clients.ToPagedList(pageNumber, pageSize),
                ListClientAwards = listClientAwards.ToPagedList(pageNumber, pageSize),
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