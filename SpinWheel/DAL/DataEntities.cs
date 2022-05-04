using SpinWheel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SpinWheel.DAL
{
    public class DataEntities : DbContext
    {
        public DataEntities() : base("name=DataEntities") { }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<ConfigSite> ConfigSites { get; set; }
        public DbSet<Award> Awards { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ListClientAward> ListClientAward { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleCategory> ArticleCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Quote> Quotes { get; set; }
    }
}