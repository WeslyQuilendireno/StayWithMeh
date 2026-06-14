using Microsoft.AspNetCore.Mvc;
using StayWithMeh.Models;
using Supabase;

namespace StayWithMeh.Controllers
{
    public class RoomController : Controller
    {
        private readonly Client _supabase;

        public RoomController(Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<IActionResult> Index(string? filter)
        {
            ViewData["ActivePage"] = "Rooms";
            ViewData["Filter"]     = filter ?? "All Rooms";

            var result = await _supabase.From<Room>().Get();
            return View(result.Models);
        }

        public async Task<IActionResult> Details(string? id)
        {
            ViewData["ActivePage"] = "Rooms";

            if (string.IsNullOrEmpty(id))
            {
                var all = await _supabase.From<Room>().Get();
                return View(all.Models.First());
            }

            var result = await _supabase
                .From<Room>()
                .Where(r => r.Id == id)
                .Get();

            var room = result.Models.FirstOrDefault();
            if (room is null)
                return NotFound();

            return View(room);
        }

        [HttpPost]
        public async Task<IActionResult> Book([FromBody] BookingRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.GuestEmail) ||
                string.IsNullOrWhiteSpace(request.GuestName) ||
                string.IsNullOrWhiteSpace(request.RoomId))
            {
                return BadRequest("Missing required booking fields.");
            }

            var roomResult = await _supabase
                .From<Room>()
                .Where(r => r.Id == request.RoomId)
                .Get();

            var room = roomResult.Models.FirstOrDefault();
            if (room is null)
                return NotFound("Room not found.");

            var nights = (request.CheckOut - request.CheckIn).Days;
            if (nights <= 0)
                return BadRequest("Check-out date must be after check-in date.");

            var subtotal = room.BasePrice * nights;
            var tax      = subtotal * 0.12m;
            var levy     = 5m * nights;
            var total    = subtotal + tax + levy;

            // Lookup-or-create guest by email — stands in for auth until login exists
            var existingGuest = await _supabase
                .From<Guest>()
                .Where(g => g.Email == request.GuestEmail)
                .Get();

            var guest = existingGuest.Models.FirstOrDefault();

            if (guest is null)
            {
                var newGuest = new Guest
                {
                    FullName = request.GuestName,
                    Email    = request.GuestEmail
                };

                var insertedGuest = await _supabase.From<Guest>().Insert(newGuest);
                guest = insertedGuest.Models.First();
            }

            var booking = new Booking
            {
                GuestId     = guest.Id,
                RoomId      = room.Id,
                CheckIn     = request.CheckIn,
                CheckOut    = request.CheckOut,
                Status      = "upcoming",
                TotalAmount = total
            };

            var insertedBooking = await _supabase.From<Booking>().Insert(booking);
            var createdBooking  = insertedBooking.Models.First();

            var invoice = new Invoice
            {
                BookingId     = createdBooking.Id,
                Amount        = total,
                PaymentMethod = request.PaymentMethod,
                PaymentStatus = "paid"
            };

            await _supabase.From<Invoice>().Insert(invoice);

            return Json(new
            {
                success = true,
                bookingId = createdBooking.Id,
                total = total.ToString("0.00"),
                nights = nights
            });
        }
    }
}
