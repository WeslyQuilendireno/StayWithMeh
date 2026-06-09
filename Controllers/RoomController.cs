using Microsoft.AspNetCore.Mvc;

namespace StayWithMeh.Controllers
{
    public class RoomController : Controller
    {
        // ── Static room catalogue (replace with Supabase data in Week 2) ──
        private static readonly Dictionary<int, RoomInfo> Rooms = new()
        {
            [1] = new("Skyline Executive Suite",  "Business Suite",  "Executive Floor", "Room 402", 450, 400),
            [2] = new("Urban Deluxe King",        "Deluxe Suite",    "Premium Floor",   "Room 205", 215, 190),
            [3] = new("Grand Family Loft",        "Family Suite",    "Family Wing",     "Room 101", 450, 400),
            [4] = new("Standard King Room",       "Standard",        "Standard Floor",  "Room 304", 180, 160),
            [5] = new("Family Connection Suite",  "Family Suite",    "Family Wing",     "Room 510", 380, 340),
            [6] = new("Executive Studio",         "Business Suite",  "Executive Floor", "Room 102", 150, 130),
        };

        // GET /Room  or  /Room/Index
        public IActionResult Index(string? filter)
        {
            ViewData["ActivePage"] = "Rooms";
            ViewData["Filter"]     = filter ?? "All Rooms";
            return View();
        }

        // GET /Room/Details/{id}
        public IActionResult Details(int? id)
        {
            ViewData["ActivePage"] = "Rooms";

            var roomId = id ?? 1;
            if (!Rooms.TryGetValue(roomId, out var room))
            {
                // Fallback to room 1 if id is unknown
                room = Rooms[1];
                roomId = 1;
            }

            ViewData["RoomId"]    = roomId;
            ViewData["RoomType"]  = room.Type;
            ViewData["RoomNum"]   = room.RoomNumber;
            ViewData["Floor"]     = room.Floor;
            ViewData["Price"]     = "$" + room.PricePerNight;
            ViewData["BasePrice"] = room.BasePrice;

            return View();
        }

        // POST /Room/Book  (future Supabase integration)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Book(int roomId, string guestName,
                                  string checkIn, string checkOut,
                                  string payment)
        {
            // TODO (Week 2): persist to Supabase bookings table
            TempData["BookingSuccess"] = $"Booking confirmed for {guestName} in room {roomId}.";
            return RedirectToAction("MyBookings", "Booking");
        }

        // Simple record to hold room data
        private record RoomInfo(
            string Name,
            string Type,
            string Floor,
            string RoomNumber,
            decimal PricePerNight,
            decimal BasePrice);
    }
}
