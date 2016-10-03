using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.EF
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        [MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string ID { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public virtual IEnumerable<Credential> Credentials { set; get; }
    }
}