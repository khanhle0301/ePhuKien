using System;
using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
    public class PageViewModel
    {
        public int ID { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập tên")]
        [Display(Name = "Tiêu đề")]
        public string Name { set; get; }

        public string Alias { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập thứ tự")]
        [Display(Name = "Thứ tự")]
        public int? DisplayOrder { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung")]
        [Display(Name = "Nội dung")]
        public string Content { set; get; }

        public DateTime? CreatedDate { set; get; }

        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public string UpdatedBy { set; get; }

        public string MetaKeyword { set; get; }

        public string MetaDescription { set; get; }

        public bool Status { set; get; }
    }
}