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
            ViewData["Filter"] = filter ?? "All Rooms";

            var result = await _supabase.From<Room>().Get();
            return View(result.Models);
        }

        // id is the Supabase UUID (room.Id), not an integer
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
    }
}