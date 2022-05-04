using SpinWheel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpinWheel.ViewModel
{
    public class BannerViewModel
    {
        public Banner Banner { get; set; }
        public SelectList SelectGroup { get; set; }
        public BannerViewModel()
        {
            var listgroup = new Dictionary<int, string> { 
                { 1, "Khách hàng" },
                { 2, "Lý do nên chọn" }
            };
            SelectGroup = new SelectList(listgroup, "Key", "Value");
        }
    }
    public class ListBannerViewModel
    {
        public PagedList.IPagedList<Banner> Banners { get; set; }
        public int? GroupId { get; set; }
        public SelectList SelectGroup { get; set; }
        public ListBannerViewModel()
        {
            var listgroup = new Dictionary<int, string> { 
                { 1, "Khách hàng" },
                { 2, "Lý do nên chọn" }
            };
            SelectGroup = new SelectList(listgroup, "Key", "Value");
        }
    }
}