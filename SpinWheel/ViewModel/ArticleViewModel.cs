using SpinWheel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc; 

namespace SpinWheel.ViewModel
{
    public class ListArticleViewModel
    {
        public PagedList.IPagedList<Article> Articles { get; set; }
        public SelectList SelectCategories { get; set; }
        public SelectList ChildCategoryList { get; set; }
        public int? CatId { get; set; }
        public int? ChildId { get; set; }
        public string Name { get; set; }

        public ListArticleViewModel()
        {
            ChildCategoryList = new SelectList(new List<ArticleCategory>(), "Id", "CategoryName");
        }
    }
    public class InsertArticleViewModel
    {
        public Article Article { get; set; }
        public IEnumerable<ArticleCategory> Categories { get; set; }
        public SelectList SelectCategories { get; set; }


        public int? CategoryId { get; set; }
    }
}