using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.EF
{
    [Table("ContactDetails")]
    public class ContactDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [MaxLength(250)]
        [Required]
        public string Name { set; get; }

        [MaxLength(50)]
        public string Phone { set; get; }

        [MaxLength(250)]
        public string Email { set; get; }

        [MaxLength(250)]
        public string Website { set; get; }

        [MaxLength(250)]
        public string Address { set; get; }

        public string Other { set; get; }

        public double? Lat { set; get; }

        public double? Lng { set; get; }

        public bool Status { set; get; }
    }
}