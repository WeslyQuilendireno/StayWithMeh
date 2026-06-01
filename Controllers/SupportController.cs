using Microsoft.AspNetCore.Mvc;

namespace StayWithMeh.Controllers
{
    public class SupportController : Controller
    {
        // GET: /Support  or  /Support/Index
        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Support";
            return View();
        }
    }
}
