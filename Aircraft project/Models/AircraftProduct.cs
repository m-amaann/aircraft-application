using System;
using System.ComponentModel.DataAnnotations;

namespace Aircraft_project.Models
{
    public class AircraftProduct
    {
        [Key]
        public int AircraftId { get; set; }

        public required string ModelName { get; set; }
        
        public required string Image { get; set; }

        [StringLength(500)]
        public required string Description { get; set; }

        public required string Price { get; set; }

        public required string Colors { get; set; }

        public required int SeatCount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
