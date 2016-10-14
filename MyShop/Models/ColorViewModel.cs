using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
    public class ColorViewModel
    {
        public string ID { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập tên")]
        [Display(Name = "Tên màu")]
        public string Name { set; get; }

        [Display(Name = "Mã màu")]
        [Required(ErrorMessage = "Vui lòng nhập mã màu")]
        public string Background { set; get; }
    }
}