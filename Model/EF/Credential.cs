using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.EF
{
    [Table("Credentials")]
    public class Credential
    {
        [Key]
        [Column(TypeName = "varchar", Order = 1)]
        [MaxLength(20)]
        public string UserGroupID { set; get; }

        [Key]
        [Column(TypeName = "varchar", Order = 2)]
        [MaxLength(50)]
        public string RoleID { set; get; }
    
        public bool Status { set; get; }

        [ForeignKey("UserGroupID")]
        public virtual UserGroup UserGroups { set; get; }

        [ForeignKey("RoleID")]
        public virtual Role Role { set; get; }
    }
}