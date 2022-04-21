using SpinWheel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpinWheel.ViewModel
{
    public class ListAwardViewModel
    {
        public PagedList.IPagedList<Award> Awards { get; set; }
        public int TotalAward { get; set; }
        public string Name { get; set; }
        [Display(Name = "Danh mục sự kiện")]
        public int? EventId { get; set; }
        public Event Event { get; set; }
        public IEnumerable<Event> Events { get; set; }
        public SelectList EventSelectList { get; set; }

    }
    public class InsertAwardViewModel
    {
        public Award Award { get; set; }
        [Display(Name = "Danh mục sự kiện")]
        public IEnumerable<Event> Events { get; set; }
    }
    public class ListClientViewModel
    {
        public PagedList.IPagedList<Client> Clients { get; set; }
        public string Name { get; set; }
        public Event Event { get; set; }
    }
    public class InsertEventViewModel
    {
        public Event Event { get; set; }
        public IEnumerable<Event> Events { get; set; }
    }
}