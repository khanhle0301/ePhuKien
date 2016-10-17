using System;
using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
    public class ProductViewModel
    {
        public int ID { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập tên")]
        [Display(Name = "Tên")]
        public string Name { set; get; }

        public string Alias { set; get; }

        [Display(Name = "Danh mục")]
        public int CategoryID { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập số lượng")]
        [Display(Name = "Số lượng")]
        public int? Quantity { set; get; }

        public int? QuantitySold { set; get; }

        [Required(ErrorMessage = "Yêu cầu chọn hình ảnh 1")]
        [Display(Name = "Hình ảnh 1")]
        public string Image { set; get; }

        [Required(ErrorMessage = "Yêu cầu chọn hình ảnh 2")]
        [Display(Name = "Hình ảnh 2")]
        public string Image2 { set; get; }

        public string MoreImages { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập giá")]
        [Display(Name = "Giá")]
        public decimal Price { set; get; }

        public decimal? PromotionPrice { set; get; }

        public int? Warranty { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập mô tả ngắn")]
        [Display(Name = "Mô tả ngắn")]
        public string Description { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập mô tả")]
        [Display(Name = "Mô tả")]
        public string Content { set; get; }

        [Display(Name = "Hiển thị trang chủ")]
        public bool? HomeFlag { set; get; }

        [Display(Name = "Sản phẩm hot")]
        public bool? HotFlag { set; get; }

        public int? ViewCount { set; get; }

        public DateTime? CreatedDate { set; get; }

        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public string UpdatedBy { set; get; }

        public string MetaKeyword { set; get; }

        public string MetaDescription { set; get; }

        public bool Status { set; get; }

        public string Tags { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập màu sắc")]
        [Display(Name = "Màu sắc")]
        public string Colors { set; get; }

        public virtual ProductCategoryViewModel ProductCategory { set; get; }
    }
}