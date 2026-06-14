using Postgrest.Attributes;
using Postgrest.Models;

namespace StayWithMeh.Models
{
    [Table("rooms")]
    public class Room : BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = string.Empty;

        [Column("property_id")]
        public string PropertyId { get; set; } = string.Empty;

        [Column("room_number")]
        public string RoomNumber { get; set; } = string.Empty;

        [Column("room_type")]
        public string RoomType { get; set; } = string.Empty;

        [Column("status")]
        public string Status { get; set; } = "Available";

        [Column("price_per_night")]
        public decimal PricePerNight { get; set; }

        [Column("base_price")]
        public decimal BasePrice { get; set; }

        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [Column("image_url")]
        public string ImageUrl { get; set; } = string.Empty;

        [Column("floor")]
        public int? Floor { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
