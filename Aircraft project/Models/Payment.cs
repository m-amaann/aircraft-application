using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aircraft_project.Models
{
    public class Payment
    {
        [Key] public required int PaymentId { get; set; }

        [ForeignKey("Orders")]
        public int OrderId { get; set; }

        public required string Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        public virtual Orders Orders { get; set; }
    }
}
