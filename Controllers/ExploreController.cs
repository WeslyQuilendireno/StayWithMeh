using Microsoft.AspNetCore.Mvc;
using StayWithMeh.Models;
using Supabase;

namespace StayWithMeh.Controllers
{
    public class ExploreController : Controller
    {
        private readonly Client _supabase;

        public ExploreController(Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<IActionResult> Index(string? destination, string? checkIn,
                                                 string? checkOut, string? type, string? sort)
        {
            ViewData["ActivePage"]  = "Explore";
            ViewData["Destination"] = destination ?? "";
            ViewData["CheckIn"]     = checkIn     ?? "";
            ViewData["CheckOut"]    = checkOut    ?? "";
            ViewData["Type"]        = type        ?? "All Types";
            ViewData["Sort"]        = sort        ?? "Recommended";

            var properties = await _supabase.From<Property>().Get();
            var rooms      = await _supabase.From<Room>().Get();

            var model = new ExploreViewModel
            {
                Properties = properties.Models,
                Rooms      = rooms.Models
            };

            return View(model);
        }
    }
}
