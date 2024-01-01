using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aircraft_project.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        public virtual Users Users { get; set; }

/*        public List<CartItem> Items { get; set; } = new List<CartItem>();
*/
        public int TotalAmount { get; set; }

        public int ShippingFee { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
