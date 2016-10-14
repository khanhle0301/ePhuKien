using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
    public class FooterViewModel
    {
        public int ID { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập tên")]
        [Display(Name = "Tên")]
        public string Name { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung")]
        [Display(Name = "Nội dung")]
        public string Content { set; get; }
    }
}