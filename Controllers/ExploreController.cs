using Microsoft.AspNetCore.Mvc;

namespace StayWithMeh.Controllers
{
    public class ExploreController : Controller
    {
        public IActionResult Index(string? destination, string? checkIn,
                                   string? checkOut, string? type, string? sort)
        {
            ViewData["ActivePage"]   = "Explore";
            ViewData["Destination"]  = destination ?? "";
            ViewData["CheckIn"]      = checkIn     ?? "";
            ViewData["CheckOut"]     = checkOut    ?? "";
            ViewData["Type"]         = type        ?? "All Types";
            ViewData["Sort"]         = sort        ?? "Recommended";
            return View();
        }
    }
}
