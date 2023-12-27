using System;

namespace Aircraft_project.Models
{
    public class Product
    {
        [Key]
        public int AircraftId { get; set; }

        public required string ModelName { get; set; }
        
        public required string Image { get; set; }

        public string Description { get; set; }

        public required string Price { get; set; }

        public string specification { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
