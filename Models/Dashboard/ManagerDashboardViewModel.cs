namespace StayWithMeh.Models
{
    public class ManagerDashboardViewModel
    {
        public List<Room> Rooms { get; set; } = new();
        public List<Booking> Bookings { get; set; } = new();
        public List<StaffShift> Shifts { get; set; } = new();

        public int AvailableCount   => Rooms.Count(r => r.Status == "available");
        public int OccupiedCount    => Rooms.Count(r => r.Status == "occupied");
        public int DirtyCount       => Rooms.Count(r => r.Status == "dirty");
        public int MaintenanceCount => Rooms.Count(r => r.Status == "maintenance");

        public int OccupancyPercent => Rooms.Count > 0
            ? (int)Math.Round((double)OccupiedCount / Rooms.Count * 100)
            : 0;

        public int TodayCheckInsCount => Bookings
            .Count(b => b.CheckIn.Date == DateTime.UtcNow.Date);

        // Floor 1 only, to match the prototype's "Floor 1 Overview" panel
        public List<Room> FloorOneRooms => Rooms
            .Where(r => r.Floor == 1)
            .OrderBy(r => r.RoomNumber)
            .ToList();
    }
}
