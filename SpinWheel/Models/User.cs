using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpinWheel.Models
{
    public class User
    {
        public int Id { get; set; }
        [Display(Name = "Tên đăng nhập", Description = "Tên đăng nhập"), Required(ErrorMessage = "Hãy điền tên đăng nhập"), RegularExpression(@"[a-z0-9_.]{4,10}", ErrorMessage = "Chỉ nhập chữ thường và số 0-9, từ 4-10 ký tự"), UIHint("TextBox")]
        public string Username { get; set; }
        [DisplayName("Mật khẩu"), Required(ErrorMessage = "Hãy nhập mật khẩu"), StringLength(60, ErrorMessage = "Tối đa 60 ký tự"), UIHint("Password")]
        public string Password { get; set; }
        [Display(Name = "Số điện thoại"), Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }
        [StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), Display(Name = "Địa chỉ email"), EmailAddress(ErrorMessage = "Email không chính xác")]
        public string Email { get; set; }
        [Display(Name = "Hoạt động", Description = "Hoạt động")]
        public bool Active { get; set; }
        public virtual Admin Admin { get; set; }
        public TypeUser TypeUser { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public User()
        {
            CreateDate = DateTime.Now;
            Active = true;
        }
    }
    public enum TypeUser
    {
        Normal,
        Premium,
        Unlimited,
    }
}