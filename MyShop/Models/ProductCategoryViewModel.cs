using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
    public class ProductCategoryViewModel
    {
        public int ID { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập tên danh mục")]
        [Display(Name = "Tên danh mục")]
        public string Name { set; get; }

        public string Alias { set; get; }

        [Display(Name = "Danh mục cha")]
        public int? ParentID { set; get; }

        public int? DisplayOrder { set; get; }

        [Display(Name = "Hình ảnh")]
        public string Image { set; get; }

        public virtual IEnumerable<PostViewModel> Posts { set; get; }

        public DateTime? CreatedDate { set; get; }

        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public string UpdatedBy { set; get; }

        public string MetaKeyword { set; get; }

        public string MetaDescription { set; get; }

        public bool Status { set; get; }
    }
}