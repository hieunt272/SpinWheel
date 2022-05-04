using SpinWheel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpinWheel.DAL
{
    public class UnitOfWork : IDisposable
    {
        private readonly DataEntities _context = new DataEntities();
        private GenericRepository<Admin> _adminRepository;
        private GenericRepository<ConfigSite> _configRepository;
        private GenericRepository<Award> _awardRepository;
        private GenericRepository<Event> _eventRepository;
        private GenericRepository<Client> _clientRepository;
        private GenericRepository<User> _userRepository;
        private GenericRepository<ListClientAward> _listClientAwardRepository;
        private GenericRepository<Article> _articleRepository;
        private GenericRepository<ArticleCategory> _articleCategoryRepository;
        private GenericRepository<Product> _productRepository;
        private GenericRepository<Contact> _contactRepository;
        private GenericRepository<Banner> _bannerRepository;
        private GenericRepository<Quote> _quoteRepository;

        public GenericRepository<Quote> QuoteRepository =>
            _quoteRepository ?? (_quoteRepository = new GenericRepository<Quote>(_context));
        public GenericRepository<Banner> BannerRepository =>
            _bannerRepository ?? (_bannerRepository = new GenericRepository<Banner>(_context));
        public GenericRepository<Article> ArticleRepository =>
            _articleRepository ?? (_articleRepository = new GenericRepository<Article>(_context));
        public GenericRepository<ArticleCategory> ArticleCategoryRepository =>
            _articleCategoryRepository ?? (_articleCategoryRepository = new GenericRepository<ArticleCategory>(_context));
        public GenericRepository<Product> ProductRepository => 
            _productRepository ?? (_productRepository = new GenericRepository<Product>(_context));
        public GenericRepository<Contact> ContactRepository => 
            _contactRepository ?? (_contactRepository = new GenericRepository<Contact>(_context));
        public GenericRepository<ListClientAward> ListClientAwardRepository =>
            _listClientAwardRepository ?? (_listClientAwardRepository = new GenericRepository<ListClientAward>(_context));
        public GenericRepository<Award> AwardRepository =>
            _awardRepository ?? (_awardRepository = new GenericRepository<Award>(_context));
        public GenericRepository<Event> EventRepository =>
            _eventRepository ?? (_eventRepository = new GenericRepository<Event>(_context));
        public GenericRepository<Client> ClientRepository =>
            _clientRepository ?? (_clientRepository = new GenericRepository<Client>(_context));
        public GenericRepository<User> UserRepository =>
            _userRepository ?? (_userRepository = new GenericRepository<User>(_context));
        public GenericRepository<ConfigSite> ConfigSiteRepository =>
            _configRepository ?? (_configRepository = new GenericRepository<ConfigSite>(_context));
        public GenericRepository<Admin> AdminRepository =>
            _adminRepository ?? (_adminRepository = new GenericRepository<Admin>(_context));
        public void Save()
        {
            _context.SaveChanges();
        }
        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}