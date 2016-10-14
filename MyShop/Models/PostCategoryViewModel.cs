using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyShop.Models
{
    public class PostCategoryViewModel
    {
        public int ID { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập tên danh mục")]
        [MaxLength(100, ErrorMessage = "Tên danh mục không được quá 100 ký tự")]
        [Display(Name = "Tên danh mục")]
        public string Name { set; get; }

        public string Alias { set; get; }
      
        public virtual IEnumerable<PostViewModel> Posts { set; get; }

        public DateTime? CreatedDate { set; get; }

        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public string UpdatedBy { set; get; }

        public string MetaKeyword { set; get; }

        public string MetaDescription { set; get; }
    
        [Display(Name = "Trạng thái")]
        public bool Status { set; get; }
    }
}