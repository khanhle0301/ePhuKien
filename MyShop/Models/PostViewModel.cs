using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
    public class PostViewModel
    {
        public int ID { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập tên danh mục")]
        [Display(Name = "Tên danh mục")]
        [MaxLength(100, ErrorMessage = "Tên danh mục không được quá 100 ký tự")]
        public string Name { set; get; }

        public string Alias { set; get; }

        [Display(Name = "Danh mục")]
        public int CategoryID { set; get; }

        [Required(ErrorMessage = "Vui lòng chọn hình")]    
        [Display(Name = "Hình")]
        public string Image { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả ngắn")]
        [Display(Name = "Mô tả ngắn")]
        public string Description { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả")]
        [Display(Name = "Mô tả")]
        public string Content { set; get; }     

        public int? ViewCount { set; get; }

        public DateTime? CreatedDate { set; get; }

        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public string UpdatedBy { set; get; }

        public string MetaKeyword { set; get; }

        public string MetaDescription { set; get; }
      
        [Display(Name = "Trạng thái")]
        public bool Status { set; get; }

        public string Tags { set; get; }

        public virtual PostCategoryViewModel PostCategory { set; get; }

        public virtual IEnumerable<PostTagViewModel> PostTags { set; get; }
    }
}