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

namespace SpinWheel.Controllers
{
    [Authorize]
    public class SpinWheelController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private IEnumerable<Award> Awards => _unitOfWork.AwardRepository.Get();

        #region Event
        [ChildActionOnly]
        public ActionResult ListEvent()
        {
            var events = _unitOfWork.EventRepository.Get(orderBy: l => l.OrderBy(a => a.Sort));
            return PartialView(events);
        }
        public ActionResult Event(string result = "")
        {
            ViewBag.Result = result;
            var events = new Event { Sort = 1, Active = true };
            return View(events);
        }
        [HttpPost]
        public ActionResult Event(Event model)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Files["BgPC"];
                if (file != null && file.ContentLength > 0)
                {
                    if (!HtmlHelpers.CheckFileExt(file.FileName, "jpg|jpeg|png|gif"))
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
                            var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(file.FileName)) + "-" + DateTime.Now.Millisecond + Path.GetExtension(file.FileName);
                            var imgPath = "/images/events/" + DateTime.Now.ToString("yyyy/MM/dd");
                            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

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
                _unitOfWork.EventRepository.Insert(model);
                _unitOfWork.Save();

                return RedirectToAction("Event", new { result = "success" });
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

                _unitOfWork.EventRepository.Update(model);
                _unitOfWork.Save();

                return RedirectToAction("Event", new { result = "update" });
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
        public ActionResult ListAward(int? page, string name, string result = "")
        {
            ViewBag.Result = result;
            var pageNumber = page ?? 1;
            const int pageSize = 10;
            var award = _unitOfWork.AwardRepository.GetQuery(orderBy: l => l.OrderBy(a => a.Id));

            if (!string.IsNullOrEmpty(name))
            {
                award = award.Where(l => l.AwardName.ToLower().Contains(name.ToLower()));
            }
            var model = new ListAwardViewModel
            {
                Awards = award.ToPagedList(pageNumber, pageSize),
                Name = name,
            };
            return View(model);
        }
        public ActionResult Award()
        {
            return View();
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
        public ActionResult UpdateAward(int sort = 0)
        {
            var award = _unitOfWork.AwardRepository.GetQuery(a => a.Sort == sort);
            if (award == null)
            {
                return RedirectToAction("ListAward");
            }
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateAward(InsertAwardViewModel model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Save();
                return RedirectToAction("ListAward", new { result = "update" });
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
        public ActionResult ListClient(int? page, string name)
        {
            var pageNumber = page ?? 1;
            const int pageSize = 10;
            var clients = _unitOfWork.ClientRepository.Get(orderBy: l => l.OrderByDescending(a => a.Id));
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