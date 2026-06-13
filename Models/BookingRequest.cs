namespace StayWithMeh.Models
{
    public class BookingRequest
    {
        public string RoomId { get; set; } = string.Empty;
        public string GuestName { get; set; } = string.Empty;
        public string GuestEmail { get; set; } = string.Empty;
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string PaymentMethod { get; set; } = "cc";
    }
}
