using Helpers;
using PagedList;
using SpinWheel.DAL;
using SpinWheel.Models;
using SpinWheel.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
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

        private IEnumerable<ArticleCategory> ArticleCategories =>
            _unitOfWork.ArticleCategoryRepository.Get(a => a.CategoryActive, q => q.OrderBy(a => a.CategorySort));

        #region Home
        public ActionResult Index()
        {
            var model = new HomeViewModel
            {
                Products = _unitOfWork.ProductRepository.GetQuery(a => a.Active),
                Banners = _unitOfWork.BannerRepository.Get(b => b.Active, o => o.OrderBy(b => b.Sort)),
                Articles = _unitOfWork.ArticleRepository.Get(a => a.Active && a.Home && a.ArticleCategory.TypePost == TypePost.Article, o => o.OrderByDescending(a => a.CreateDate), 5),
            };
            return View(model);
        }
        [ChildActionOnly]
        public PartialViewResult Header()
        {
            var model = new HeaderViewModel
            {
                ArticleCategories = ArticleCategories.Where(a => a.ShowMenu && a.ParentId == null),
                Products = _unitOfWork.ProductRepository.GetQuery(p => p.Active && p.ShowMenu)
            };
            return PartialView(model);
        }
        [ChildActionOnly]
        public PartialViewResult Footer()
        {
            var model = new FooterViewModel
            {
                ArticleCategories = ArticleCategories.Where(a => a.ShowFooter),
                Articles = _unitOfWork.ArticleRepository.GetQuery(a => a.Active && a.ArticleCategory.TypePost == TypePost.Policy).Take(6),
                Policies = ArticleCategories.Where(a => a.TypePost == TypePost.Policy),
                Products = _unitOfWork.ProductRepository.GetQuery(a => a.Active),
            };
            return PartialView(model);
        }
        #endregion

        [ChildActionOnly]
        public PartialViewResult Contact()
        {
            return PartialView();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Contact(Contact model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { status = false, msg = "Hãy điền đúng định dạng." });
            }
            _unitOfWork.ContactRepository.Insert(model);
            _unitOfWork.Save();
            var subject = "Email liên hệ từ website: " + Request.Url?.Host;
            var body = $"<p>Tên người liên hệ: {model.Fullname},</p>" +
                       $"<p>Email liên hệ: {model.Email},</p>" +
                       $"<p>Chủ đề: {model.Theme},</p>" +
                       $"<p>Nội dung: {model.Body}</p>" +
                       $"<p>Đây là hệ thống gửi email tự động, vui lòng không phản hồi lại email này.</p>";
            Task.Run(() => HtmlHelpers.SendEmail("gmail", subject, body, ConfigSite.Email, Email, Email, Password, "Công ty Cổ phần công nghệ đa nền tảng VICO"));

            return Json(new { status = true, msg = "Gửi liên hệ thành công.\nChúng tôi sẽ liên lạc với bạn sớm nhất có thể." });
        }

        #region Article 
        [Route("bai-viet/{url}.html", Order = 1)]
        public ActionResult ArticleDetail(string url)
        {
            var article = _unitOfWork.ArticleRepository.GetQuery(a => a.Url == url && a.Active).FirstOrDefault();
            if (article == null)
            {
                return RedirectToAction("Index");
            }

            var model = new ArticleDetailsViewModel
            {
                Article = article,
                Articles = _unitOfWork.ArticleRepository.GetQuery(p => p.Active && p.ArticleCategoryId == article.ArticleCategoryId && p.Id != article.Id, q => q.OrderByDescending(a => a.CreateDate), 6)
            };
            return View(model);
        }
        [Route("bai-viet/{url:regex(^(?!.*(san-pham|vcms|uploader|article|banner|contact|productvcms)).*$)}", Order = 2)]
        public ActionResult ArticleCategory(int? page, string url)
        {
            var category = _unitOfWork.ArticleCategoryRepository.GetQuery(a => a.CategoryActive && a.Url == url).FirstOrDefault();
            if (category == null)
            {
                return RedirectToAction("Index");
            }

            var articles = _unitOfWork.ArticleRepository.GetQuery(
                a => a.Active && (a.ArticleCategoryId == category.Id || a.ArticleCategory.ParentId == category.Id),
                q => q.OrderByDescending(a => a.CreateDate));
            var pageNumber = page ?? 1;

            if (articles.Count() == 1)
            {
                var fi = articles.First();
                return RedirectToAction("ArticleDetail", new { url = fi.Url });
            }
            var model = new ArticleCategoryViewModel
            {
                Category = category,
                Articles = articles.ToPagedList(pageNumber, 12),
                Categories = ArticleCategories.Where(a => a.CategoryActive && a.TypePost == TypePost.Article)
            };

            if (category.ParentId != null)
            {
                model.RootCategory = _unitOfWork.ArticleCategoryRepository.GetById(category.ParentId);
            }
            return View(model);
        }
        [Route("bai-viet")]
        public ActionResult AllArticle(int? page)
        {
            var pageNumber = page ?? 1;
            var article = _unitOfWork.ArticleRepository.GetQuery(a => a.Active && a.ArticleCategory.TypePost == TypePost.Article, o => o.OrderByDescending(a => a.CreateDate));
            var model = new AllArticleViewModel()
            {
                Articles = article.ToPagedList(pageNumber, 12),
                Categories = ArticleCategories.Where(a => a.TypePost == TypePost.Article),
            };
            return View(model);
        }
        [ChildActionOnly]
        public PartialViewResult MenuArticle(int rootId = 0, int catId = 0)
        {
            var model = new MenuArticleViewModel
            {
                RootId = rootId,
                CatId = catId,
                ArticleCategories = ArticleCategories.Where(a => a.TypePost == TypePost.Article),
                Articles = _unitOfWork.ArticleRepository.GetQuery(l => l.Active && l.ArticleCategory.TypePost == TypePost.Article,
                a => a.OrderByDescending(c => c.CreateDate), 6)
            };
            return PartialView(model);
        }
        [Route("tim-kiem")]
        public ActionResult SearchArticle(int? page, string keywords, int catId = 0)
        {
            var pageNumber = page ?? 1;
            var pageSize = 12;
            var article = _unitOfWork.ArticleRepository.GetQuery(l => l.Active && l.Subject.ToLower().Contains(keywords.ToLower()), c => c.OrderByDescending(a => a.CreateDate));

            if (string.IsNullOrEmpty(keywords))
            {
                return RedirectToAction("Index");
            }
            if (catId > 0)
            {
                article = article.Where(a => a.ArticleCategoryId == catId);
            }

            var model = new ArticleSearchViewModel
            {
                Articles = article.ToPagedList(pageNumber, pageSize),
                Keywords = keywords,
                Categories = ArticleCategories,
                CatId = catId
            };
            return View(model);
        }
        #endregion

        #region Product
        [Route("san-pham/{url}.html", Order = 1)]
        public ActionResult Product(string url)
        {
            var product = _unitOfWork.ProductRepository.GetQuery(p => p.Url == url).FirstOrDefault();
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            var model = new ProductDetailViewModel
            {
                Product = product,
                Products = _unitOfWork.ProductRepository.GetQuery(
                    p => p.Id != product.Id && p.Active,
                    o => o.OrderByDescending(p => Guid.NewGuid()), 4)
            };
            return View(model);
        }
        #endregion

        [Route("phi-dich-vu")]
        public ActionResult Quote()
        {
            var model = new QuoteViewModel
            {
                Quotes = _unitOfWork.QuoteRepository.GetQuery(a => a.Active)
            };
            return View(model);
        }

        [HttpPost]
        public JsonResult InfoForm(Client model, FormCollection fc)
        {
            var fullName = fc["fullname"];
            var phone = fc["phone"];

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
            var model = new EventViewModel
            {
                Event = events,
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
                image = a.Image,
            });
            return Json(awards, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetClientData(string phone, int eventId)
        {
            var isPost = 1;
            var now = DateTime.Now;
            var client = _unitOfWork.ClientRepository.Get(p => p.Mobile == phone && p.ListClientAwards.Any(a => a.Award.EventId == eventId)).FirstOrDefault();

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

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}