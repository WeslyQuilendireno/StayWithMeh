using Postgrest.Attributes;
using Postgrest.Models;

namespace StayWithMeh.Models
{
    [Table("staff_shifts")]
    public class StaffShift : BaseModel
    {
        [PrimaryKey("id")]
        public string Id { get; set; } = string.Empty;

        [Column("staff_name")]
        public string StaffName { get; set; } = string.Empty;

        [Column("department")]
        public string Department { get; set; } = string.Empty;

        [Column("shift_date")]
        public DateTime ShiftDate { get; set; }

        [Column("start_time")]
        public TimeSpan StartTime { get; set; }

        [Column("end_time")]
        public TimeSpan EndTime { get; set; }

        [Column("status")]
        public string Status { get; set; } = "scheduled";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
