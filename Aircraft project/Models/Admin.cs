using System.ComponentModel.DataAnnotations;

namespace Aircraft_project.Models
{
    public class Admin
    {
        [Key]
        public required string AdminId { get; set; }
        public required string Email { get; set; }
        public required string  Password { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
