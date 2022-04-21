using SpinWheel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PagedList;

namespace SpinWheel.ViewModel
{
    public class ChangePasswordModel
    {
        [Display(Name = "Mật khẩu hiện tại"), Required(ErrorMessage = "Hãy nhập mật khẩu hiện tại"), UIHint("Password")]
        public string OldPassword { get; set; }
        [Display(Name = "Mật khẩu mới"), Required(ErrorMessage = "Hãy nhập mật khẩu mới"),
         StringLength(16, MinimumLength = 4, ErrorMessage = "Mật khẩu từ 4, 16 ký tự"), UIHint("Password")]
        public string Password { get; set; }
        [Display(Name = "Nhập lại mật khẩu"), System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Nhập lại mật khẩu không chính xác"),
         UIHint("Password")]
        public string ConfirmPassword { get; set; }
    }

    public class AdminLoginModel
    {
        [Display(Name = "Tên đăng nhập"), Required(ErrorMessage = "Hãy nhập tên đăng nhập"), RegularExpression(@"[a-z0-9_.]{4,10}", ErrorMessage = "Từ 4 đến 10 ký tự, chỉ nhập chữ thường, số 0-9, dấu . và dấu _")]
        public string Username { get; set; }
        [Display(Name = "Mật khẩu"), Required(ErrorMessage = "Hãy nhập mật khẩu")]
        public string Password { get; set; }
    }
    public class InfoAdminViewModel
    {
        public IEnumerable<Admin> Admins { get; set; }
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<Client> Clients { get; set; }
    }
    public class EditAdminViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }
        [Display(Name = "Tên đăng nhập"), Required(ErrorMessage = "Bạn chưa điền tên đăng nhập"), RegularExpression(@"[a-z0-9_.]{4,10}", ErrorMessage = "Từ 4 đến 10 ký tự, chỉ nhập chữ thường, số 0-9, dấu . và dấu _"), UIHint("TextBox")]
        public string Username { get; set; }
        [Display(Name = "Mật khẩu"), UIHint("Password"), StringLength(20, MinimumLength = 6, ErrorMessage = "Mật khẩu từ 6 - 20 ký tự")]
        public string Password { get; set; }
        [Display(Name = "Phân quyền")]
        public RoleAdmin Role { get; set; }
        [Display(Name = "Phân quyền người dùng")]
        public TypeUser TypeUser { get; set; }
    }

    public class HomeUserViewModel
    {
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<Award> Awards { get; set; }
    }

    public class ListUserViewModel
    {
        public IPagedList<User> Users { get; set; }
        public string Name { get; set; }
        public string TypeUser { get; set; }
    }
}