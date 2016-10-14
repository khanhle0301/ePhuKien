using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
    public class ContactDetailViewModel
    {
        public int ID { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập tên")]
        [Display(Name = "Tên")]
        public string Name { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Display(Name = "Số điện thoại")]
        public string Phone { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        public string Email { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập website")]      
        public string Website { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        [Display(Name = "Địa chỉ")]
        public string Address { set; get; }

        public string Other { set; get; }

        [Required]
        public double? Lat { set; get; }

        [Required]
        public double? Lng { set; get; }

        public bool Status { set; get; }
    }
}