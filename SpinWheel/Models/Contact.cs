﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpinWheel.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Display(Name = "Họ và tên"), Required(ErrorMessage = "Hãy nhập họ tên"),UIHint("TextBox"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự")]
        public string Fullname { get; set; }

        [Display(Name = "Địa chỉ"), UIHint("TextBox"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự")]
        public string Address { get; set; }

        [Display(Name = "Số điện thoại"), Required(ErrorMessage = "Hãy nhập số điện thoại"),StringLength(20, ErrorMessage = "Tối đa 20 ký tự"), UIHint("TextBox")]
        public string Mobile { get; set; }

        [Display(Name = "Email"), Required(ErrorMessage = "Hãy nhập số diện thoại"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), EmailAddress(ErrorMessage = "Email không hợp lệ"), UIHint("TextBox")]
        public string Email { get; set; }

        [Display(Name = "Nội dung"), DataType(DataType.MultilineText), StringLength(4000)]
        public string Body { get; set; }

        [Display(Name = "Chủ đề"), UIHint("TextBox")]
        public string Theme { get; set; }

        public DateTime CreateDate { get; set; }

        public Contact()
        {
            CreateDate = DateTime.Now;
        }
    }
}