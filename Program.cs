using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Supabase;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

var supabaseUrl = builder.Configuration["Supabase:Url"]
    ?? throw new InvalidOperationException("Supabase:Url is not set in User Secrets.");

var supabaseKey = builder.Configuration["Supabase:Key"]
    ?? throw new InvalidOperationException("Supabase:Key is not set in User Secrets.");

var supabaseClient = new Client(supabaseUrl, supabaseKey, new SupabaseOptions
{
    AutoConnectRealtime = true
});

await supabaseClient.InitializeAsync();

builder.Services.AddSingleton(supabaseClient);

// Validates Supabase-issued JWTs against the project's public JWKS endpoint.
// No shared secret needed — Supabase rotated to asymmetric ECC signing keys.
var supabaseAuthority = $"{supabaseUrl}/auth/v1";

// Cookie auth carries the logged-in session for this server-rendered MVC app.
// AccountController.Login() decodes the Supabase JWT once at sign-in time and
// builds this cookie's claims from it — every request after that just reads
// the cookie, no re-validation against Supabase's JWKS per request.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    })
    .AddJwtBearer(options =>
    {
        options.Authority = supabaseAuthority;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = supabaseAuthority,
            ValidateAudience = true,
            ValidAudience = "authenticated",
            ValidateLifetime = true,
            RoleClaimType = "role"
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var identity = context.Principal?.Identity as System.Security.Claims.ClaimsIdentity;
                var roleClaim = context.Principal?.FindFirst("role");

                if (identity is not null && roleClaim is not null)
                {
                    identity.AddClaim(new System.Security.Claims.Claim(
                        System.Security.Claims.ClaimTypes.Role, roleClaim.Value));
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
