using Microsoft.AspNetCore.Mvc;
using StayWithMeh.Models;
using Supabase;

namespace StayWithMeh.Controllers
{
    public class SuperAdminController : Controller
    {
        private readonly Client _supabase;

        // Only roles a SuperAdmin is allowed to assign through this dashboard.
        // SuperAdmin itself is excluded here on purpose — promoting someone
        // to SuperAdmin is too sensitive for a dropdown and should stay a
        // manual SQL action for now.
        private static readonly string[] AssignableRoles =
            { "Manager", "Receptionist", "Housekeeper", "Guest" };

        public SuperAdminController(Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"]            = "Owner Dashboard";
            ViewData["ActiveStaffPage"]  = "Dashboard";
            ViewData["StaffRoleContext"] = "SuperAdmin";
            ViewData["StaffName"]        = "Wesly Quilendireno";
            ViewData["StaffRole"]        = "Owner";
            ViewData["StaffInitials"]    = "WQ";

            var roomsResult    = await _supabase.From<Room>().Get();
            var bookingsResult = await _supabase.From<Booking>().Get();
            var profilesResult = await _supabase.From<UserProfile>().Get();

            var model = new SuperAdminDashboardViewModel
            {
                Rooms     = roomsResult.Models,
                Bookings  = bookingsResult.Models,
                Profiles  = profilesResult.Models
            };

            ViewData["AssignableRoles"] = AssignableRoles;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole([FromBody] RoleUpdateRequest request)
        {
            if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.NewRole))
                return BadRequest("UserId and NewRole are required.");

            if (!AssignableRoles.Contains(request.NewRole))
                return BadRequest("That role cannot be assigned from this dashboard.");

            var result = await _supabase.From<UserProfile>()
                .Where(p => p.Id == request.UserId)
                .Get();

            var profile = result.Models.FirstOrDefault();
            if (profile is null) return NotFound("User not found.");

            profile.Role = request.NewRole;
            await _supabase.From<UserProfile>().Update(profile);

            return Json(new { success = true, userId = request.UserId, newRole = request.NewRole });
        }
    }
}
