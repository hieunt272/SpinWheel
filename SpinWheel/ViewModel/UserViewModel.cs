using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpinWheel.ViewModel
{
    public class RegisterViewModel
    {
        [Display(Name = "Tên đăng nhập"), Required(ErrorMessage = "Hãy nhập tên đăng nhập"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự")]
        public string Username { get; set; }
        [DisplayName("Mật khẩu"), Required(ErrorMessage = "Bạn chưa nhập mật khẩu"), StringLength(20, MinimumLength = 6, ErrorMessage = "Mật khẩu từ 6 - 20 ký tự")]
        public string Password { get; set; }
        [DisplayName("Nhập lại mật khẩu"), Required(ErrorMessage = "Bạn chưa nhập lại mật khẩu")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Nhập lại mật khẩu không chính xác")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Số điện thoại"), Required(ErrorMessage = "Hãy nhập số điện thoại"), Phone(ErrorMessage = "Số điện thoại không hợp lệ"), Remote("CheckPhone", "User")]
        public string PhoneNumber { get; set; }
        [StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), Display(Name = "Địa chỉ email"), EmailAddress(ErrorMessage = "Email không chính xác")]
        public string Email { get; set; }
    }
}