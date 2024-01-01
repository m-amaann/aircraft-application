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

        public string Name { get; set; }

        [Required]
        public string PaymentType { get; set; }

        [Required]
        public string OrderStatus { get; set; }

        [Required]
        public int PostalCode { get; set; }

        [Required]
        public string Country {  get; set; }

        [Required]
        public string ShipmentAddress { get; set; }

        [Required]
        public int TotalPrice { get; set; }

        [Required]
        public string OrderTracking_No { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;



        public virtual Users Users { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }



    }
}
