using SpinWheel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpinWheel.ViewModel
{
    public class ListAwardViewModel
    {
        public PagedList.IPagedList<Award> Awards { get; set; }
        public string Name { get; set; }
        public IEnumerable<Client> Clients { get; set; }
    }
    public class InsertAwardViewModel
    {
        public Award Award { get; set; }
    }
    public class ListClientViewModel
    {
        public PagedList.IPagedList<Client> Clients { get; set; }
        public string Name { get; set; }
        public IEnumerable<Award> Awards { get; set; }
    }
}