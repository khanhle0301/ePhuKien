using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.EF
{
    [Table("UserGroups")]
    public class UserGroup
    {
        [Key]
        [MaxLength(20)]
        [Column(TypeName = "varchar")]
        public string ID { set; get; }

        [Required]
        [MaxLength(50)]
        public string Name { set; get; }

        public bool? Status { set; get; }

        public virtual IEnumerable<User> Users { set; get; }
        public virtual IEnumerable<Credential> Credentials { set; get; }
    }
}