using SpinWheel.DAL;
using SpinWheel.Models;
using Helpers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpinWheel.Controllers
{
    [Authorize]
    public class QuoteController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        #region Quote
        [ChildActionOnly]
        public ActionResult ListQuote()
        {
            var quotes = _unitOfWork.QuoteRepository.Get(orderBy: q => q.OrderBy(a => a.Sort));
            return PartialView(quotes);
        }
        public ActionResult Quote(string result = "")
        {
            ViewBag.Quote = result;
            var quote = new Quote { Sort = 1, Active = true };
            return View(quote);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Quote(Quote model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.QuoteRepository.Insert(model);
                _unitOfWork.Save();

                return RedirectToAction("Quote", new { result = "success" });
            }

            return View(model);
        } 
        public ActionResult UpdateQuote(int quoteId = 0)
        {
            var quote = _unitOfWork.QuoteRepository.GetById(quoteId);
            if (quote == null)
            {
                return RedirectToAction("Quote");
            }
            return View(quote);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateQuote(Quote model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.QuoteRepository.Update(model);
                _unitOfWork.Save();

                return RedirectToAction("Quote", new { result = "update" });
            }
            return View(model);
        }
        [HttpPost]
        public bool DeleteQuote(int quoteId = 0)
        {

            var quote = _unitOfWork.QuoteRepository.GetById(quoteId);
            if (quote == null)
            {
                return false;
            }
            _unitOfWork.QuoteRepository.Delete(quote);
            _unitOfWork.Save();
            return true;
        }
        public bool QuickUpdateQuote(int sort = 1, bool active = false, int quoteId = 0)
        {

            var quote = _unitOfWork.QuoteRepository.GetById(quoteId);
            if (quote == null)
            {
                return false;
            }
            quote.Sort = sort;
            quote.Active = active;
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