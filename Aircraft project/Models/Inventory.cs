using System.ComponentModel.DataAnnotations;

namespace Aircraft_project.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool Availability { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }
        // Add more properties as needed
    }
}