using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Aircraft_project.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        [ForeignKey("Aircraft")]
        public int AircraftId { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public virtual Aircraft Aircraft { get; set; }
        public virtual Users Users { get; set; }
    }
}