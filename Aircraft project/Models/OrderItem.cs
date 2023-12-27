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

        [ForeignKey("AircraftProduct")]
        public int AircraftId { get; set; }

        public int Quantity { get; set; }

        public string Price { get; set; }

        public virtual Orders Orders { get; set; }
        public virtual AircraftProduct AircraftProduct { get; set; }

    }
}
