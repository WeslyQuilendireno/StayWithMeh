namespace StayWithMeh.Models
{
    public class HousekeepingDashboardViewModel
    {
        public List<Room> Rooms { get; set; } = new();
        public List<HousekeepingTask> Tasks { get; set; } = new();

        public int TotalRooms       => Rooms.Count;
        public int DirtyCount       => Rooms.Count(r => r.Status == "dirty");
        public int InProgressCount  => Rooms.Count(r => r.Status == "in_progress");
        public int CleanedTodayCount => Rooms.Count(r => r.Status == "available");

        // Tasks ordered urgent-first, then by due time, for the Priority Tasks panel
        public List<HousekeepingTask> PendingTasks => Tasks
            .Where(t => t.Status != "completed")
            .OrderBy(t => t.Priority == "urgent" ? 0 : 1)
            .ThenBy(t => t.DueAt)
            .ToList();

        public string RoomNumberFor(string? roomId)
        {
            var room = Rooms.FirstOrDefault(r => r.Id == roomId);
            return room?.RoomNumber ?? "—";
        }

        public string RoomTypeFor(string? roomId)
        {
            var room = Rooms.FirstOrDefault(r => r.Id == roomId);
            return room?.RoomType ?? "";
        }
    }
}
