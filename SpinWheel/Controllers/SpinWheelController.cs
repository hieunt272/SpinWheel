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
using System.Web;
using System.Web.Hosting;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;

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
                            file.SaveAs(Server.MapPath(Path.Combine(imgPath, imgFileName)));
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
                            file.SaveAs(Server.MapPath(Path.Combine(imgPath, imgFileName)));
                        }
                    }
                }

                if (isPost)
                {
                    model.UserId = userId;
                    model.Url = HtmlHelpers.ConvertToUnSign(null, model.Url ?? model.EventName) + "-" + userId;
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
                            file.SaveAs(Server.MapPath(Path.Combine(imgPath, imgFileName)));
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
                            file.SaveAs(Server.MapPath(Path.Combine(imgPath, imgFileName)));
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
                var image = fc.GetValues("Pictures_insert");

                var awardId = fc.GetValues("Award_id");
                var awardNameUpdate = fc.GetValues("AwardName_update");
                var bgColorUpdate = fc.GetValues("BgColor_update");
                var textColorUpdate = fc.GetValues("TextColor_update");
                var percentUpdate = fc.GetValues("Percent_update");
                var quantityUpdate = fc.GetValues("Quantity_update");
                var limitedUpdate = fc.GetValues("item.Limited");
                var imageUpdate = fc.GetValues("Pictures_update");

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
                                Sort = i,
                                Image = image[i],
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
                            award.Image = imageUpdate[i];
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
                                Sort = i,
                                Image = image[i],
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
        public ActionResult ListClient(int? page, int? eventId, string name, string startDate, string endDate)
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
            if (eventId > 0)
            {
                listClientAwards = listClientAwards.Where(a => a.Award.EventId == eventId);
            }

            var model = new ListClientViewModel
            {
                EventSelectList = new SelectList(Events.Where(a => a.UserId == userId), "Id", "EventName"),
                EventId = eventId,
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

        #region Upload
        [HttpGet]
        public JsonResult Upload(IEnumerable<string> names, string folder)
        {
            if (names == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            var f = GetUploadFolder(folder).GetFiles();
            var listFile = (from fileInfo in f from name in names where name.Contains(fileInfo.Name) select fileInfo.Name).ToList();

            return Json(new
            {
                files = listFile.Select(
                    file => new
                    {
                        deleteType = "POST",
                        name = DateTime.Now.ToString("yyyy/MM/dd") + "/" + file,
                        size = file.Length,
                        url = Url.Action("GetFile", "Uploader", new
                        {
                            Name = file
                        }),
                        thumbnailUrl = Url.Action("GetFile", "Uploader", new
                        {
                            Name = file,
                            thumbnail = true
                        }),
                        deleteUrl = Url.Action("DeleteFile", "Uploader", new
                        {
                            Name = file
                        })
                    })
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Upload(string folder = "awards")
        {
            var array = Request.Files.Cast<string>().Select(k => Request.Files[k]).ToArray();
            var stringList = new List<string>();
            foreach (var file in array)
            {
                var disk = SaveFileToDisk(file, folder);
                stringList.Add(disk);
            }
            return Upload(stringList, folder);
        }
        private string SaveFileToDisk(HttpPostedFileBase fileData, string folderPath)
        {
            var folderDate = DateTime.Now.ToString("yyyy/MM/dd");
            var folder = "~/images/" + folderPath + "/" + folderDate;
            var result = "";
            if (fileData.ContentLength <= 4000 * 1024 &&
                HtmlHelpers.CheckFileExt(Path.GetExtension(fileData.FileName), "jpg|png|gif|jpeg"))
            {
                HtmlHelpers.CreateFolder(Server.MapPath(folder));

                var randomName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(fileData.FileName)) + "-" + DateTime.Now.Millisecond + Path.GetExtension(fileData.FileName);

                var fileName = Server.MapPath(Path.Combine(folder, randomName));

                Resize(fileData, 1200, 900, Path.Combine(folder, fileName));
                result = folderDate + "/" + randomName;
            }
            return result;
        }
        private static DirectoryInfo GetUploadFolder(string folder)
        {
            var directoryInfo = new DirectoryInfo(HostingEnvironment.MapPath(Path.Combine("/images/" + folder + "/" + DateTime.Now.ToString("yyyy/MM/dd"))));
            if (!directoryInfo.Exists)
                directoryInfo.Create();
            return directoryInfo;
        }
        public static void Resize(HttpPostedFileBase originalImage, int maxWidth, int maxHeight, string path, string font = null, int fontsize = 0)
        {
            var originalBmp = new Bitmap(originalImage.InputStream);
            //Check EXIF
            if (originalBmp.PropertyIdList.Contains(0x0112))
            {
                int rotationValue = originalBmp.GetPropertyItem(0x0112).Value[0];
                switch (rotationValue)
                {
                    case 1: // landscape, do nothing
                        break;
                    case 8: // rotated 90 right
                        // de-rotate:
                        originalBmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;

                    case 3: // bottoms up
                        originalBmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;

                    case 6: // rotated 90 left
                        originalBmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                }
            }

            float origWidth = originalBmp.Width;
            float origHeight = originalBmp.Height;
            float sngRatio;

            if (origWidth > maxWidth)
            {
                sngRatio = maxWidth / origWidth;
                origWidth = maxWidth;
                origHeight = origHeight * sngRatio;
            }

            if (origHeight > maxHeight)
            {
                sngRatio = maxHeight / origHeight;
                origHeight = maxHeight;
                origWidth = origWidth * sngRatio;
            }

            var newBmp = new Bitmap(originalBmp, (int)origWidth, (int)origHeight);
            var oGraphics = Graphics.FromImage(newBmp);

            oGraphics.SmoothingMode = SmoothingMode.HighQuality;
            oGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            oGraphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            oGraphics.CompositingQuality = CompositingQuality.HighQuality;

            if (font != null)
            {
                using (var font1 = new Font(font, fontsize, FontStyle.Bold, GraphicsUnit.Pixel))
                {
                    var rect1 = new Rectangle(0, 0, (int)origWidth - 1, (int)origHeight - 1);
                    var rect2 = new Rectangle(0, 0, (int)origWidth, (int)origHeight);

                    var stringFormat = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far };

                    oGraphics.DrawString("www.vico.vn", font1, new SolidBrush(Color.FromArgb(255, Color.Black)), rect1, stringFormat);
                    oGraphics.DrawString("www.vico.vn", font1, new SolidBrush(Color.FromArgb(255, Color.White)), rect2, stringFormat);
                    //oGraphics.DrawRectangle(Pens.DarkRed, rect1);
                }
            }

            oGraphics.DrawImage(newBmp, 0, 0, origWidth, origHeight);

            //var jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            var mimeType = HtmlHelpers.GetMimeType(originalImage.FileName);
            var jgpEncoder = HtmlHelpers.GetEncoderInfo(mimeType);

            var myEncoder = Encoder.Quality;
            var myEncoderParameters = new EncoderParameters(1);

            var myEncoderParameter = new EncoderParameter(myEncoder, 90L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            newBmp.Save(path, jgpEncoder, myEncoderParameters);

            originalBmp.Dispose();
            newBmp.Dispose();
            oGraphics.Dispose();
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}