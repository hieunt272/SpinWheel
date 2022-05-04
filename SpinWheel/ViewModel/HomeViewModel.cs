using SpinWheel.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpinWheel.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Article> Articles { get; set; }
        public IEnumerable<Banner> Banners { get; set; }
        public Contact Contact { get; set; }
    }

    public class EventViewModel
    {
        public IEnumerable<Award> Awards { get; set; }
        public Event Event { get; set; }
        public IEnumerable<Client> Clients { get; set; }
    }

    public class ProductDetailViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public Product Product { get; set; }
    }
    public class HeaderViewModel
    {
        public IEnumerable<ArticleCategory> ArticleCategories { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
    public class FooterViewModel
    {
        public IEnumerable<ArticleCategory> ArticleCategories { get; set; }
        public IEnumerable<Article> Articles { get; set; }
        public IEnumerable<ArticleCategory> Policies { get; set; }
    }
    public class AllArticleViewModel
    {
        public IPagedList<Article> Articles { get; set; }
        public IEnumerable<ArticleCategory> Categories { get; set; }
    }
    public class ArticleCategoryViewModel
    {
        public ArticleCategory RootCategory { get; set; }
        public ArticleCategory Category { get; set; }
        public IPagedList<Article> Articles { get; set; }
        public IEnumerable<ArticleCategory> Categories { get; set; }
    }
    public class ArticleDetailsViewModel
    {
        public Article Article { get; set; }
        public IEnumerable<Article> Articles { get; set; }
    }

    public class MenuArticleViewModel
    {
        public int RootId { get; set; }
        public int CatId { get; set; }
        public IEnumerable<Article> Articles { get; set; }
        public IEnumerable<ArticleCategory> ArticleCategories { get; set; }
    }
    public class ArticleSearchViewModel
    {
        public string Keywords { get; set; }
        public IPagedList<Article> Articles { get; set; }
        public IEnumerable<ArticleCategory> Categories { get; set; }
        public int? CatId { get; set; }
    }
    public class QuoteViewModel
    {
        public IEnumerable<Quote> Quotes { get; set; }
    }
}