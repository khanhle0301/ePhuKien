using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.EF
{
    [Table("Colors")]
    public class Color
    {
        [Key]       
        public int ID { set; get; }

        [MaxLength(50)]
        [Required]
        public string Name { set; get; }

        public string Background { set; get; }

        public virtual IEnumerable<ProductColor> ProductColor { set; get; }
    }
}