# StayWithMeh — Setup Guide

This guide covers everything needed to run StayWithMeh locally from a clean clone.

---

## Prerequisites

- Visual Studio 2022 (ASP.NET and web development workload)
- .NET 8.0 SDK
- Python 3.10+
- Node.js (for Playwright testing, optional)
- A Supabase project ([supabase.com](https://supabase.com))

---

## 1. Clone the Repository

```bash
git clone https://github.com/WeslyQuilendireno/StayWithMeh.git
cd StayWithMeh
```

---

## 2. Environment Variables (.env)

StayWithMeh uses `DotNetEnv` to load environment variables at runtime. Create a `.env` file at the project root (same level as `StayWithMeh.csproj`). **Never commit this file — it is already in `.gitignore`.**

Copy `.env.example` and fill in your own values:

```bash
cp .env.example .env
```

### `.env.example`

```
# Supabase Project URL
# Found in: Supabase Dashboard ? Project Settings ? API ? Project URL
SUPABASE_URL=https://your-project-ref.supabase.co

# Supabase Anon/Public Key
# Found in: Supabase Dashboard ? Project Settings ? API ? Project API Keys ? anon public
SUPABASE_ANON_KEY=your-anon-key-here

# Supabase Service Role Key (used only by Python pricing microservice)
# Found in: Supabase Dashboard ? Project Settings ? API ? Project API Keys ? service_role
# WARNING: Never expose this key to the browser or commit it to source control
SUPABASE_SERVICE_ROLE_KEY=your-service-role-key-here

# Python Pricing Microservice
# The branch/property ID the pricing script will target (leave blank to run all branches)
PRICING_TARGET_PROPERTY_ID=

# ASP.NET Environment
ASPNETCORE_ENVIRONMENT=Development
```

### Loading in C\#

`DotNetEnv` is already installed. In `Program.cs`, load the file before building the app:

```csharp
using DotNetEnv;

Env.Load();

var builder = WebApplication.CreateBuilder(args);
// ...
```

Access values anywhere in the app:

```csharp
var supabaseUrl = Environment.GetEnvironmentVariable("SUPABASE_URL");
var supabaseKey = Environment.GetEnvironmentVariable("SUPABASE_ANON_KEY");
```

---

## 3. User Secrets (Alternative to .env for local development)

If you prefer not to use a `.env` file, ASP.NET Core User Secrets store sensitive values outside the project directory entirely.

### Initialize User Secrets

Run this once from the project root (where `StayWithMeh.csproj` lives):

```bash
dotnet user-secrets init --project StayWithMeh.csproj
```

### Set Each Secret

```bash
dotnet user-secrets set "Supabase:Url" "https://your-project-ref.supabase.co" --project StayWithMeh.csproj
dotnet user-secrets set "Supabase:AnonKey" "your-anon-key-here" --project StayWithMeh.csproj
dotnet user-secrets set "Supabase:ServiceRoleKey" "your-service-role-key-here" --project StayWithMeh.csproj
```

### Access in C\#

Secrets are automatically loaded into `IConfiguration` in Development mode:

```csharp
var supabaseUrl = builder.Configuration["Supabase:Url"];
var supabaseKey = builder.Configuration["Supabase:AnonKey"];
```

### View All Set Secrets

```bash
dotnet user-secrets list --project StayWithMeh.csproj
```

### Remove a Secret

```bash
dotnet user-secrets remove "Supabase:ServiceRoleKey" --project StayWithMeh.csproj
```

Secrets are stored at:
- Windows: `%APPDATA%\Microsoft\UserSecrets\<user-secrets-id>\secrets.json`
- macOS/Linux: `~/.microsoft/usersecrets/<user-secrets-id>/secrets.json`

The `user-secrets-id` is defined in `StayWithMeh.csproj` under `<UserSecretsId>`.

---

## Secrets Management Strategy

StayWithMeh splits secrets across two mechanisms depending on which part of the system consumes them.

| Storage | Used by | Contains | Why |
|---|---|---|---|
| User Secrets (`secrets.json`) | ASP.NET Core app | `Supabase:Url`, `Supabase:Key` (anon key) | Lives outside the project folder — cannot be committed by accident, no `.gitignore` dependency |
| `pricing/.env` | Python microservice | `SUPABASE_URL`, `SUPABASE_SERVICE_ROLE_KEY` | Python has no equivalent to User Secrets; `.env` + `.gitignore` is the standard pattern |

### Key Types

- **Anon / publishable key** (`Supabase:Key`) — safe to use in the ASP.NET app. Row Level Security policies (see `ARCHITECTURE.md`) restrict what this key can read or write per role. This key is *not* a secret in the traditional sense, but is still stored via User Secrets for convenience and consistency.

- **Service role key** (`SUPABASE_SERVICE_ROLE_KEY`) — bypasses RLS entirely. Used **only** by the Python pricing microservice to write `price_per_night` overnight. This key must never appear in:
  - Client-side JavaScript
  - Any `.cshtml` file
  - Git history (committed `.env` files, screenshots, chat logs)

### Rule of Thumb

If a key can bypass RLS, it goes in `pricing/.env` and stays on the server filesystem only. If a key is meant to be used under RLS restrictions, User Secrets is sufficient for local development.

---

## 4. Supabase C# Client Setup

The `supabase-csharp` package is already installed. Wire up the client in `Program.cs`:

```csharp
using Supabase;

var supabaseUrl = builder.Configuration["Supabase:Url"]
    ?? Environment.GetEnvironmentVariable("SUPABASE_URL")
    ?? throw new InvalidOperationException("Supabase URL is not configured.");

var supabaseKey = builder.Configuration["Supabase:AnonKey"]
    ?? Environment.GetEnvironmentVariable("SUPABASE_ANON_KEY")
    ?? throw new InvalidOperationException("Supabase anon key is not configured.");

var supabaseOptions = new SupabaseOptions
{
    AutoConnectRealtime = true
};

var supabase = new Client(supabaseUrl, supabaseKey, supabaseOptions);
await supabase.InitializeAsync();

builder.Services.AddSingleton(supabase);
```

Inject the client into any controller:

```csharp
public class RoomController : Controller
{
    private readonly Supabase.Client _supabase;

    public RoomController(Supabase.Client supabase)
    {
        _supabase = supabase;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _supabase
            .From<Room>()
            .Where(r => r.Status == "available")
            .Get();

        return View(result.Models);
    }
}
```

---

## 5. Python Pricing Microservice Setup

The pricing engine lives in the `pricing/` directory. It uses a virtual environment to keep dependencies isolated from your local Python installation.

### Create the Virtual Environment

From the project root:

```bash
cd pricing
python -m venv .venv
```

### Activate the Virtual Environment

**Windows (Command Prompt):**
```bash
.venv\Scripts\activate
```

**Windows (PowerShell):**
```bash
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

You should see `(.venv)` appear in your terminal prompt.

### Install Dependencies

```bash
pip install -r requirements.txt
```

### `requirements.txt`

```
supabase==2.3.4
python-dotenv==1.0.1
schedule==1.2.1
numpy==1.26.4
```

### Configure the Pricing Script

Create a `pricing/.env` file (separate from the root `.env`):

```
SUPABASE_URL=https://your-project-ref.supabase.co
SUPABASE_SERVICE_ROLE_KEY=your-service-role-key-here
PRICING_TARGET_PROPERTY_ID=
```

### Run the Pricing Script Manually

```bash
python run_pricing.py
```

### Schedule as an Overnight Cron Job

**Linux/macOS (crontab):**

```bash
crontab -e
```

Add this line to run at 2:00 AM daily:

```
0 2 * * * /path/to/project/pricing/.venv/bin/python /path/to/project/pricing/run_pricing.py >> /path/to/project/pricing/logs/pricing.log 2>&1
```

**Windows (Task Scheduler):**

Create a new Basic Task:
- Trigger: Daily at 2:00 AM
- Action: Start a program
- Program: `C:\path\to\project\pricing\.venv\Scripts\python.exe`
- Arguments: `run_pricing.py`
- Start in: `C:\path\to\project\pricing\`

### Deactivate the Virtual Environment

```bash
deactivate
```

---

## 6. Running the Application

```bash
dotnet run --project StayWithMeh.csproj
```

Or press `F5` in Visual Studio. The app will be available at:
- `https://localhost:7255`
- `http://localhost:5027`

---

## 7. Running Tests

```bash
dotnet test
```

Playwright tests (when configured):

```bash
dotnet test --filter "Category=E2E"
```

---

## Project Structure

```
StayWithMeh/
??? Controllers/
?   ??? HomeController.cs
?   ??? ExploreController.cs
?   ??? RoomController.cs
?   ??? BookingController.cs
?   ??? WishlistController.cs
?   ??? SupportController.cs
??? Models/
?   ??? ErrorViewModel.cs
??? Views/
?   ??? Shared/_Layout.cshtml
?   ??? Home/Index.cshtml
?   ??? Explore/Index.cshtml
?   ??? Room/Index.cshtml
?   ??? Room/Details.cshtml
?   ??? Booking/MyBookings.cshtml
?   ??? Wishlist/Index.cshtml
?   ??? Support/Index.cshtml
??? wwwroot/
?   ??? images/
??? pricing/                    ? Python microservice
?   ??? .venv/                  ? not committed
?   ??? run_pricing.py
?   ??? requirements.txt
?   ??? .env                    ? not committed
??? .env                        ? not committed
??? .env.example                ? committed (no real keys)
??? .gitignore
??? Program.cs
??? appsettings.json
??? README.md
??? ARCHITECTURE.md
??? SETUP.md
```