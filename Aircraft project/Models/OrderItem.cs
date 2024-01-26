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

        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        // public virtual Orders Orders { get; set; }
    }
}
