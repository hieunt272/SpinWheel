using SpinWheel.DAL;
using SpinWheel.Models;
using SpinWheel.ViewModel;
using Helpers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpinWheel.Controllers
{
    [Authorize]
    public class ProductVcmsController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        #region Product  
        public ActionResult ListProduct(int? page, string name, string sort = "date-desc", string result = "")
        {
            ViewBag.Result = result;
            var pageNumber = page ?? 1;
            const int pageSize = 15;
            var products = _unitOfWork.ProductRepository.GetQuery().AsNoTracking();
            if (!string.IsNullOrEmpty(name))
            {
                products = products.Where(l => l.Name.ToLower().Contains(name.ToLower()));
            }

            switch (sort)
            {
                case "sort-asc":
                    products = products.OrderBy(a => a.Sort);
                    break;
                case "sort-desc":
                    products = products.OrderByDescending(a => a.Sort);
                    break;
                case "date-asc":
                    products = products.OrderBy(a => a.CreateDate);
                    break;
                default:
                    products = products.OrderByDescending(a => a.CreateDate);
                    break;
            }
            var model = new ListProductViewModel
            {
                Products = products.ToPagedList(pageNumber, pageSize),
                Name = name,
                Sort = sort
            };
            return View(model);
        }
        public ActionResult Product()
        {
            var model = new InsertProductViewModel
            {
                Product = new Product { Sort = 1, Active = true },
            };
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Product(InsertProductViewModel model, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                model.Product.Url = HtmlHelpers.ConvertToUnSign(null, model.Product.Url ?? model.Product.Name);
                _unitOfWork.ProductRepository.Insert(model.Product);
                _unitOfWork.Save();

                var count = _unitOfWork.ProductRepository.GetQuery(a => a.Url == model.Product.Url).Count();
                if (count > 1)
                {
                    model.Product.Url += "-" + model.Product.Id;
                    _unitOfWork.Save();
                }

                return RedirectToAction("ListProduct", new { result = "success" });
            }

            return View(model);
        }
        public ActionResult UpdateProduct(int proId = 0)
        {
            var product = _unitOfWork.ProductRepository.GetById(proId);
            if (product == null)
            {
                return RedirectToAction("ListProduct");
            }
            var model = new InsertProductViewModel
            {
                Product = product,
            };
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateProduct(InsertProductViewModel model, FormCollection fc)
        {
            var product = _unitOfWork.ProductRepository.GetById(model.Product.Id);
            if (product == null)
            {
                return RedirectToAction("ListProduct");
            }
            if (ModelState.IsValid)
            {
                product.Url = HtmlHelpers.ConvertToUnSign(null, model.Product.Url ?? model.Product.Name);
                product.Name = model.Product.Name;
                product.Body = model.Product.Body;
                product.Active = model.Product.Active;
                product.Description = model.Product.Description;
                product.ShowMenu = model.Product.ShowMenu;
                product.TitleMeta = model.Product.TitleMeta;
                product.DescriptionMeta = model.Product.DescriptionMeta;
                product.Sort = model.Product.Sort;
                _unitOfWork.Save();

                return RedirectToAction("ListProduct", new { result = "update" });
            }

            return View(model);
        }
        [HttpPost]
        public bool DeleteProduct(int proId = 0)
        {
            var product = _unitOfWork.ProductRepository.GetById(proId);
            if (product == null)
            {
                return false;
            }
            _unitOfWork.ProductRepository.Delete(product);
            _unitOfWork.Save();
            return true;
        }
        [HttpPost]
        public bool QuickUpdate(bool? status, bool active, bool home, bool hot, int sort = 0, int proId = 0)
        {
            var product = _unitOfWork.ProductRepository.GetById(proId);
            if (product == null)
            {
                return false;
            }
            if (status != null)
            {
                product.Active = Convert.ToBoolean(status);
            }
            if (sort >= 0)
            {
                product.Sort = sort;
            }
            product.Active = active;
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