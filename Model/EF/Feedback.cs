using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.EF
{
    [Table("Feedbacks")]
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [MaxLength(250)]
        [Required]
        public string Name { set; get; }

        [MaxLength(250)]
        public string Email { set; get; }

        [MaxLength(25)]
        public string Phone { set; get; }

        [MaxLength(500)]
        public string Message { set; get; }

        public DateTime? CreatedDate { set; get; }

        public bool Status { set; get; }
    }
}