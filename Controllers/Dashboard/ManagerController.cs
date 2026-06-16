using Microsoft.AspNetCore.Mvc;
using StayWithMeh.Models;
using Supabase;

namespace StayWithMeh.Controllers
{
    public class ManagerController : Controller
    {
        private readonly Client _supabase;

        public ManagerController(Client supabase)
        {
            _supabase = supabase;
        }

        // Staff Operations — landing page for the Manager role
        public async Task<IActionResult> Index()
        {
            ViewData["Title"]           = "Staff Operations";
            ViewData["ActiveStaffPage"] = "Staff";
            ViewData["StaffRoleContext"] = "Manager";
            ViewData["StaffName"]       = "Alex Mercer";
            ViewData["StaffRole"]       = "Hotel Manager";
            ViewData["StaffInitials"]   = "AM";

            var roomsResult    = await _supabase.From<Room>().Get();
            var bookingsResult = await _supabase.From<Booking>().Get();
            var shiftsResult   = await _supabase.From<StaffShift>().Get();

            var model = new ManagerDashboardViewModel
            {
                Rooms    = roomsResult.Models,
                Bookings = bookingsResult.Models,
                Shifts   = shiftsResult.Models
            };

            return View(model);
        }

        public async Task<IActionResult> Analytics()
        {
            ViewData["Title"]           = "Manager Analytics";
            ViewData["ActiveStaffPage"] = "Analytics";
            ViewData["StaffRoleContext"] = "Manager";
            ViewData["StaffName"]       = "Alex Mercer";
            ViewData["StaffRole"]       = "Hotel Manager";
            ViewData["StaffInitials"]   = "AM";

            var roomsResult    = await _supabase.From<Room>().Get();
            var bookingsResult = await _supabase.From<Booking>().Get();
            var shiftsResult   = await _supabase.From<StaffShift>().Get();

            var model = new ManagerDashboardViewModel
            {
                Rooms    = roomsResult.Models,
                Bookings = bookingsResult.Models,
                Shifts   = shiftsResult.Models
            };

            return View(model);
        }
    }
}
