using Microsoft.AspNetCore.Mvc;
using StayWithMeh.Models;
using Supabase;

namespace StayWithMeh.Controllers
{
    public class ReceptionistController : Controller
    {
        private readonly Client _supabase;

        public ReceptionistController(Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"]           = "Dashboard Overview";
            ViewData["ActiveStaffPage"] = "Overview";
            ViewData["StaffName"]       = "Front Desk";
            ViewData["StaffRole"]       = "Receptionist";
            ViewData["StaffInitials"]   = "FD";

            var roomsResult    = await _supabase.From<Room>().Get();
            var bookingsResult = await _supabase.From<Booking>().Get();
            var guestsResult   = await _supabase.From<Guest>().Get();

            var model = new ReceptionistDashboardViewModel
            {
                Rooms    = roomsResult.Models,
                Bookings = bookingsResult.Models,
                Guests   = guestsResult.Models
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoomStatus([FromBody] RoomStatusUpdateRequest request)
        {
            if (string.IsNullOrEmpty(request.RoomId) || string.IsNullOrEmpty(request.Status))
                return BadRequest("RoomId and Status are required.");

            var validStatuses = new[] { "available", "occupied", "dirty", "maintenance" };
            if (!validStatuses.Contains(request.Status))
                return BadRequest("Invalid status value.");

            // Fetch the existing room, update status, and push the change back
            var result = await _supabase.From<Room>()
                .Where(r => r.Id == request.RoomId)
                .Get();

            var room = result.Models.FirstOrDefault();
            if (room is null) return NotFound("Room not found.");

            room.Status = request.Status;
            await _supabase.From<Room>().Upsert(room);

            return Json(new { success = true, roomId = request.RoomId, status = request.Status });
        }
    }
}
