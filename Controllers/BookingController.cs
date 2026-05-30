using Microsoft.AspNetCore.Mvc;

namespace StayWithMeh.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult MyBookings(string? filter)
        {
            ViewData["ActivePage"] = "Bookings";
            ViewData["Filter"]     = filter ?? "All";
            return View();
        }
    }
}
