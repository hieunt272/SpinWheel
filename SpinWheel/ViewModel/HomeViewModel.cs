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
        public IEnumerable<Event> Events { get; set; }
    }

    public class EventViewModel
    {
        public IEnumerable<Award> Awards { get; set; }
        public Event Event { get; set; }
        public IEnumerable<Client> Clients { get; set; }
    }
}