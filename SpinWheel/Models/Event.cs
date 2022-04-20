using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpinWheel.Models
{
    public class Event
    {
        public int Id { get; set; }
        [Display(Name = "Tên sự kiện"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string EventName { get; set; }
        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }
        [Display(Name = "Hình nền PC"), UIHint("ImageArticle")]
        public string BgPC { get; set; }
        [Display(Name = "Hình nền Mobile"), UIHint("ImageArticle")]
        public string BgMobile { get; set; }
        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập số thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int Sort { get; set; }
        [StringLength(300)]
        public string Url { get; set; }
        public int AdminId { get; set; }
        public virtual Admin Admin { get; set; }
        public virtual ICollection<Award> Awards { get; set; }
        public Event()
        {
            Active = true;
        }
    }
}