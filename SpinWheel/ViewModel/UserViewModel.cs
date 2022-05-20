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
        [Display(Name = "Tên đăng nhập"), Required(ErrorMessage = "Hãy nhập tên đăng nhập"), 
         StringLength(50, ErrorMessage = "Tối đa 20 ký tự"), RegularExpression(@"[a-z0-9_.]{4,20}", ErrorMessage = "Từ 4 đến 20 ký tự, chỉ nhập chữ thường, số 0-9, dấu . và dấu _")]
        public string Username { get; set; }
        [DisplayName("Mật khẩu"), Required(ErrorMessage = "Bạn chưa nhập mật khẩu"), StringLength(20, MinimumLength = 6, ErrorMessage = "Mật khẩu từ 6 - 20 ký tự")]
        public string Password { get; set; }
        [DisplayName("Nhập lại mật khẩu"), Required(ErrorMessage = "Bạn chưa nhập lại mật khẩu")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Nhập lại mật khẩu không chính xác")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Số điện thoại"), Required(ErrorMessage = "Hãy nhập số điện thoại"), 
         RegularExpression(@"^\(?(09|03|07|08|05)\)?[-. ]?([0-9]{8})$", ErrorMessage = "Số điện thoại không đúng định dạng!"), Remote("CheckPhone", "User")]
        public string PhoneNumber { get; set; }
        [StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), Display(Name = "Địa chỉ email"), EmailAddress(ErrorMessage = "Email không chính xác")]
        public string Email { get; set; }
    }
    public class UserLoginModel
    {
        [Display(Name = "Tên đăng nhập"), Required(ErrorMessage = "Hãy nhập tên đăng nhập"), RegularExpression(@"[a-z0-9_.]{4,20}", ErrorMessage = "Từ 4 đến 20 ký tự, chỉ nhập chữ thường, số 0-9, dấu . và dấu _")]
        public string Username { get; set; }
        [Display(Name = "Mật khẩu"), Required(ErrorMessage = "Hãy nhập mật khẩu")]
        public string Password { get; set; }
    }
}
