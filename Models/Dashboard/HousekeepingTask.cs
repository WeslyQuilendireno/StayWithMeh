using Postgrest.Attributes;
using Postgrest.Models;

namespace StayWithMeh.Models
{
    [Table("housekeeping_tasks")]
    public class HousekeepingTask : BaseModel
    {
        [PrimaryKey("id")]
        public string Id { get; set; } = string.Empty;

        [Column("room_id")]
        public string? RoomId { get; set; }

        [Column("task_type")]
        public string TaskType { get; set; } = string.Empty;

        [Column("priority")]
        public string Priority { get; set; } = "standard";

        [Column("description")]
        public string? Description { get; set; }

        [Column("due_at")]
        public DateTime? DueAt { get; set; }

        [Column("status")]
        public string Status { get; set; } = "pending";

        [Column("assigned_to")]
        public string? AssignedTo { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
