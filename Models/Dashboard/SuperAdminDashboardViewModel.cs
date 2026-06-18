namespace StayWithMeh.Models
{
    public class SuperAdminDashboardViewModel
    {
        public List<Room> Rooms { get; set; } = new();
        public List<Booking> Bookings { get; set; } = new();
        public List<UserProfile> Profiles { get; set; } = new();

        public int OccupancyPercent => Rooms.Count > 0
            ? (int)Math.Round((double)Rooms.Count(r => r.Status == "occupied") / Rooms.Count * 100)
            : 0;

        public decimal TotalRevenue => Bookings.Sum(b => b.TotalAmount);

        // Anyone with a real staff role, not the default Guest — shown
        // in the Privileged Accounts panel
        public List<UserProfile> PrivilegedAccounts => Profiles
            .Where(p => p.Role != "Guest")
            .OrderBy(p => p.Role)
            .ToList();
    }
}
