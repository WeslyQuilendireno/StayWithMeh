using Postgrest.Attributes;
using Postgrest.Models;

namespace StayWithMeh.Models
{
    [Table("properties")]
    public class Property : BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = string.Empty;

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("city")]
        public string City { get; set; } = string.Empty;

        [Column("country")]
        public string Country { get; set; } = string.Empty;

        [Column("flag_emoji")]
        public string FlagEmoji { get; set; } = string.Empty;

        [Column("address")]
        public string Address { get; set; } = string.Empty;

        [Column("image_url")]
        public string ImageUrl { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}