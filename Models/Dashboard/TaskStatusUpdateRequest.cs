namespace StayWithMeh.Models
{
    public class TaskStatusUpdateRequest
    {
        public string TaskId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
