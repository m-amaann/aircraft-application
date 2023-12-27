using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aircraft_project.Models
{
    public class Orders
    {
        [Key] 
        public int OrderId { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        public required string OrderStatus { get; set; }

        public required int PostalCode { get; set; }

        public required string Country {  get; set; }

        public required string Shipment_Address { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;




        public virtual Users Users { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }



    }
}
