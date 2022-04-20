using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpinWheel.Models
{
    public class Award
    {
        public int Id { get; set; }
        [Display(Name = "Tên giải thưởng"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string AwardName { get; set; }
        [Display(Name = "Màu nền"), Required(ErrorMessage = "Hãy chọn màu nền"), UIHint("TextBox")]
        public string BgColor { get; set; }
        [Display(Name = "Màu chữ"), Required(ErrorMessage = "Hãy chọn màu chữ"), UIHint("TextBox")]
        public string TextColor { get; set; }
        [Display(Name = "Số lượng"), UIHint("TextBox")]
        public string Quantity { get; set; }
        [Display(Name = "Tỷ lệ trúng (%)"), UIHint("TextBox")]
        public string Percent { get; set; }
        [Display(Name = "Tổng số trúng giải"), UIHint("TextBox")]
        public int TotalWin { get; set; }
        [Display(Name = "Không giới hạn")]
        public bool Limited { get; set; }
        public int Sort { get; set; }
        [Display(Name = "Danh mục sự kiện"), Required(ErrorMessage = "Hãy chọn danh mục sự kiện")]
        public int EventId { get; set; }
        public virtual ICollection<Client> Clients { get; set; }
        public virtual Event Event { get; set; }

        public Award()
        {
            Limited = false;
        }
    }
}