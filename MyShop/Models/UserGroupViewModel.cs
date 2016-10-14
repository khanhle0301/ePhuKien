using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
    public class UserGroupViewModel
    {
        public int ID { set; get; }

        [Required(ErrorMessage = "Yêu cầu tên")]
        [Display(Name = "Tên")]
        public string Name { set; get; }

        public string Role { set; get; }
    }
}