using System;
using System.ComponentModel.DataAnnotations;


namespace Aircraft_project.Models
{
    public class Admin
    {

        [Key]
        public int AdminId { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
