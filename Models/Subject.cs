using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nedelyaeva.Models
{
    public class Subject
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("teacher_id")]
        public int TeacherId { get; set; }

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }
    }
}