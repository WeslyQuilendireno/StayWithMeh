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
            var result = await _supabase.From<Booking>().Get();

            return View(result.Models);
        }
    }
}
