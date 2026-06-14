namespace StayWithMeh.Models
{
    public class MyBookingsViewModel
    {
        public List<Booking> Bookings { get; set; } = new();

        // Keyed by Room.Id so the view can look up image/type per booking
        public Dictionary<string, Room> RoomsById { get; set; } = new();
    }
}
