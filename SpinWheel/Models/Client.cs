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
        [Display(Name = "Thời gian quay thưởng gần nhất")]
        public DateTime CheckDate { get; set; }
        public string AwardName { get; set; }
        public virtual ICollection<ListClientAward> ListClientAwards { get; set; }
        public Client()
        {
            CreateDate = DateTime.Now;
            CheckDate = DateTime.Now;
        }
    }
}