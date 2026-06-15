namespace StayWithMeh.Models
{
    public class ReceptionistDashboardViewModel
    {
        public List<Room> Rooms { get; set; } = new();
        public List<Booking> Bookings { get; set; } = new();
        public List<Guest> Guests { get; set; } = new();

        // Derived counts used by the room status grid legend
        public int AvailableCount    => Rooms.Count(r => r.Status == "available");
        public int OccupiedCount     => Rooms.Count(r => r.Status == "occupied");
        public int MaintenanceCount  => Rooms.Count(r => r.Status == "maintenance");
        public int DirtyCount        => Rooms.Count(r => r.Status == "dirty");

        // Today's check-ins — used by the Upcoming Arrivals panel
        public List<Booking> TodayArrivals => Bookings
            .Where(b => b.CheckIn.Date == DateTime.UtcNow.Date)
            .OrderBy(b => b.CheckIn)
            .ToList();
    }
}
