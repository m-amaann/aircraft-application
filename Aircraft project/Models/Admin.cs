using System;
using System.ComponentModel.DataAnnotations;


namespace Aircraft_project.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }

        [Required] 
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
