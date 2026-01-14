using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nedelyaeva.Models
{
    public class Grade
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("student_id")]
        public int StudentId { get; set; }

        [Column("subject_id")]
        public int SubjectId { get; set; }

        [Column("grade_value")]
        public int? GradeValue { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }
    }
}