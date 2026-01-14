using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nedelyaeva.Models
{
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("group_id")]
        public int? GroupId { get; set; }

        [Required]
        [Column("last_name")]
        public string LastName { get; set; }

        [Required]
        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("middle_name")]
        public string MiddleName { get; set; }

        [Required]
        [Column("role")]
        public string Role { get; set; } // teacher | student

        [Required]
        [Column("login")]
        public string Login { get; set; }

        [Required]
        [Column("password")]
        public string Password { get; set; }

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}