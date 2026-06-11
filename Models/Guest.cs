using Postgrest.Attributes;
using Postgrest.Models;

namespace StayWithMeh.Models
{
    [Table("guests")]
    public class Guest : BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = string.Empty;

        [Column("full_name")]
        public string FullName { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("phone")]
        public string Phone { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}