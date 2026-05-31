using Microsoft.AspNetCore.Mvc;

namespace StayWithMeh.Controllers
{
    public class RoomController : Controller
    {
        public IActionResult Index(string? filter)
        {
            ViewData["ActivePage"] = "Rooms";
            ViewData["Filter"]     = filter ?? "All Rooms";
            return View();
        }

        public IActionResult Details(int? id)
        {
            ViewData["ActivePage"] = "Rooms";
            ViewData["RoomId"]     = id ?? 1;
            return View();
        }
    }
}
