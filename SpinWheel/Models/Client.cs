using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpinWheel.Models
{
    public class Client
    { 
        public int Id { get; set; }
        [Display(Name = "Họ và tên"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), UIHint("TextBox")]
        public string Fullname { get; set; }
        [Display(Name = "Số điện thoại"), StringLength(20, ErrorMessage = "Tối đa 20 ký tự"), UIHint("TextBox")]
        public string Mobile { get; set; } 
        public DateTime CreateDate { get; set; }
        public string AwardName { get; set; }
        public int AdminId { get; set; }
        public virtual Admin Admin { get; set; }
        public IEnumerable<Award> Awards { get; set; }
        public Client()
        {
            CreateDate = DateTime.Now;
        }
    }
}