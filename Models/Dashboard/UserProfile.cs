using Postgrest.Attributes;
using Postgrest.Models;

namespace StayWithMeh.Models
{
    [Table("user_profiles")]
    public class UserProfile : BaseModel
    {
        [PrimaryKey("id")]
        public string Id { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("full_name")]
        public string? FullName { get; set; }

        [Column("role")]
        public string Role { get; set; } = "Guest";

        [Column("branch_id")]
        public string? BranchId { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
