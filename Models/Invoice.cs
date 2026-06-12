using Postgrest.Attributes;
using Postgrest.Models;

namespace StayWithMeh.Models
{
    [Table("invoices")]
    public class Invoice : BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = string.Empty;

        [Column("booking_id")]
        public string BookingId { get; set; } = string.Empty;

        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("payment_method")]
        public string PaymentMethod { get; set; } = string.Empty;

        [Column("payment_status")]
        public string PaymentStatus { get; set; } = "Unpaid";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}