using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using StayWithMeh.Models;
using Supabase;

namespace StayWithMeh.Controllers
{
    public class AccountController : Controller
    {
        private readonly Client _supabase;

        public AccountController(Client supabase)
        {
            _supabase = supabase;
        }

        [HttpGet]
        public IActionResult Login() => View(new LoginViewModel());

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            Supabase.Gotrue.Session? session;
            try
            {
                session = await _supabase.Auth.SignInWithPassword(model.Email, model.Password);
            }
            catch
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            if (session?.AccessToken is null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            // The custom access token hook injects "role" and "branch_id" into
            // the JWT — decode it once here to build the cookie identity.
            // From this point on, [Authorize(Roles=...)] reads from the cookie,
            // not from the raw token (Option A — see prior discussion).
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(session.AccessToken);

            var role = jwt.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? "Guest";
            var email = jwt.Claims.FirstOrDefault(c => c.Type == "email")?.Value ?? model.Email;
            var userId = jwt.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ?? "";

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId),
                new(ClaimTypes.Email, email),
                new(ClaimTypes.Role, role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectByRole(role);
        }

        [HttpGet]
        public IActionResult Register() => View(new RegisterViewModel());

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                await _supabase.Auth.SignUp(model.Email, model.Password);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Could not create account: " + ex.Message);
                return View(model);
            }

            // The on_auth_user_created trigger auto-creates a Guest-role
            // profile row; update full_name now that we have it.
            var profileResult = await _supabase.From<UserProfile>()
                .Where(p => p.Email == model.Email)
                .Get();

            var profile = profileResult.Models.FirstOrDefault();
            if (profile is not null)
            {
                profile.FullName = model.FullName;
                await _supabase.From<UserProfile>().Update(profile);
            }

            TempData["RegisterSuccess"] = "Account created. You can now log in.";
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _supabase.Auth.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private IActionResult RedirectByRole(string role) => role switch
        {
            "SuperAdmin"   => RedirectToAction("Index", "SuperAdmin"),
            "Manager"      => RedirectToAction("Index", "Manager"),
            "Receptionist" => RedirectToAction("Index", "Receptionist"),
            "Housekeeper"  => RedirectToAction("Index", "Housekeeping"),
            _              => RedirectToAction("Index", "Home")
        };
    }
}
