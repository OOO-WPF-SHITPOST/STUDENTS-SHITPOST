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

        
        [Column("last_name")]
        public string LastName { get; set; }

        
        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("middle_name")]
        public string MiddleName { get; set; }

        
        [Column("role")]
        public string Role { get; set; } // teacher | student

        
        [Column("login")]
        public string Login { get; set; }

        
        [Column("password")]
        public string Password { get; set; }

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}