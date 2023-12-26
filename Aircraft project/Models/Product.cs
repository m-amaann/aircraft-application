using System;

namespace Aircraft_project.Models
{
    public class Product
    {
        [Key]
        public int AircraftId { get; set; }

        [Required]
        [StringLength(100)]
        public string ModelName { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ImagePath { get; set; }

        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public string specification { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
