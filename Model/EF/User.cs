using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.EF
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string UserName { set; get; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string Password { set; get; }
      
        public int GroupID { set; get; }

        [Required]
        [MaxLength(100)]      
        public string Name { set; get; }

        [MaxLength(200)]       
        public string Address { set; get; }

        [MaxLength(200)]
        public string Email { set; get; }

        [MaxLength(50)]
        public string Phone { set; get; }

        [ForeignKey("GroupID")]
        public virtual UserGroup UserGroups { set; get; }
    }
}