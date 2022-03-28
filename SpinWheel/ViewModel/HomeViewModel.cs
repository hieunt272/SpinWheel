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
        public IEnumerable<Award> Awards { get; set; }
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<Client> Clients { get; set; }
    }
}