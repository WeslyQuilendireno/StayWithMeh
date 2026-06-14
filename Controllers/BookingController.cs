using Microsoft.AspNetCore.Mvc;
using StayWithMeh.Models;
using Supabase;

namespace StayWithMeh.Controllers
{
    public class BookingController : Controller
    {
        private readonly Client _supabase;

        public BookingController(Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<IActionResult> MyBookings(string? filter)
        {
            ViewData["ActivePage"] = "Bookings";
            ViewData["Filter"]     = filter ?? "All";

            // No guest auth yet — once login exists, add
            // .Where(b => b.GuestId == currentGuestId) here
            var bookingsResult = await _supabase.From<Booking>().Get();
            var roomsResult    = await _supabase.From<Room>().Get();

            var model = new MyBookingsViewModel
            {
                Bookings  = bookingsResult.Models,
                RoomsById = roomsResult.Models.ToDictionary(r => r.Id)
            };

            return View(model);
        }
    }
}
