using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aircraft_project.Models
{
    public class Orders
    {
        [Key]
        public int OrderId { get; set; }

        // [ForeignKey("Users")]
        public int UserId { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Pincode { get; set; }
        public string Address { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public string PaymentStatus { get; set; }

        // public virtual Users Users { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
