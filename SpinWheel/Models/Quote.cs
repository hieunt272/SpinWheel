using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpinWheel.Models
{
    public class Quote
    {
        public int Id { get; set; }
        [Display(Name = "Tên dịch vụ"), Required(ErrorMessage = "Hãy nhập tên dịch vụ"), StringLength(150, ErrorMessage = "Tối đa 150 ký tự"), UIHint("TextBox")]
        public string Name { get; set; }
        [Display(Name = "Giá dịch vụ"), UIHint("TextBox")]
        public string Price { get; set; }
        [Display(Name = "Nội dung"), UIHint("EditorBox")]
        public string Body { get; set; }
        [Display(Name = "Gói dịch vụ")]
        public TypeService TypeService { get; set; }
        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập số thứ tự")
        , RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int Sort { get; set; }
        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }
    }
    public enum TypeService
    {
        [Display(Name = "Start")]
        Start,
        [Display(Name = "Standard")]
        Standard,
        [Display(Name = "Custom")]
        Custom
    }
}