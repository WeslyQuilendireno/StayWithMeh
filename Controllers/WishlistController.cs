using Microsoft.AspNetCore.Mvc;

namespace StayWithMeh.Controllers
{
    public class WishlistController : Controller
    {
        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Saved";
            return View();
        }
    }
}
