using Postgrest.Attributes;
using Postgrest.Models;

namespace StayWithMeh.Models
{
    [Table("bookings")]
    public class Booking : BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = string.Empty;

        [Column("guest_id")]
        public string GuestId { get; set; } = string.Empty;

        [Column("room_id")]
        public string RoomId { get; set; } = string.Empty;

        [Column("check_in")]
        public DateTime CheckIn { get; set; }

        [Column("check_out")]
        public DateTime CheckOut { get; set; }

        [Column("status")]
        public string Status { get; set; } = "Pending";

        [Column("total_amount")]
        public decimal TotalAmount { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}