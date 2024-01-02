using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aircraft_project.Models
{
    public class OrderItem
    {
        [Key] 
        public int OrderItemId { get; set; }

        [ForeignKey("Orders")]
        public int OrderId { get; set; }

        [ForeignKey("Aircraft")]
        public int AircraftId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int Price { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;



        public virtual Orders Orders { get; set; }
        public virtual Aircraft Aircraft { get; set; }

    }
}
