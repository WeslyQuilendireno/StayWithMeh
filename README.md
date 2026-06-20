# StayWithMeh — Weekly Sprint Log

**Project:** Hotel Chain Management System
**Developer:** Wesly Quilendireno
**Stack:** ASP.NET Core MVC · C# · Supabase · Tailwind CSS · Razor Views
**Repository:** https://github.com/WeslyQuilendireno/StayWithMeh

---

## Project Overview

A full-stack web-based Hotel Chain Management System supporting multiple user roles (Guest, Receptionist, Manager, Housekeeper, SuperAdmin/Owner). Built for educational/personal use with a focus on clean architecture, real-time features, role-based access control, and AI-powered room pricing. StayWithMeh operates across **8 global branches**: New York, London, Tokyo, Dubai, Caracas, Addis Ababa, Melbourne, and Singapore.

---

## Week 1 — May 24–31, 2026

### Sprint Goal
Set up the project foundation, design system, database, and implement all guest-facing pages.

### Completed

#### System Design & Planning
- Defined system requirements and core actors (Guest, Receptionist, Manager, Housekeeper, System)
- Designed OOD class diagram with key classes: Hotel, Room, RoomBooking, RoomKey, HouseKeeping, RoomCharge, Invoice
- Mapped activity diagrams for: Room Booking, Check-in, Cancel Booking, Notifications
- Planned architecture: C# MVC as main app + Python microservice for AI pricing
- Chose Supabase as BaaS for PostgreSQL + Realtime

#### UI/UX Design
- Designed full prototype in Google Stitch (13 screens)
- Established design system: Navy `#031635` primary, Inter font, Material Symbols icons, Tailwind CSS

#### Development Environment
- Visual Studio 2022 Community with ASP.NET, Python, and C++ workloads
- ASP.NET Core MVC project: `StayWithMeh` (.NET 8.0 LTS)
- NuGet packages: `supabase-csharp` v0.16.2, `DotNetEnv` v3.2.0
- User Secrets configured for Supabase credentials
- GitHub repository created: `WeslyQuilendireno/StayWithMeh`

#### Database Setup (Supabase)
- Created Supabase project: HotelMS (Region: Northeast Asia - Tokyo)
- Tables created: `rooms`, `guests`, `bookings`, `invoices`
- Row Level Security enabled on all tables
- Realtime enabled on all tables
- Foreign key relationships configured

#### Guest Pages
- `Views/Shared/_Layout.cshtml` — fixed navbar, sliding underline, scroll shadow, footer
- `Views/Home/Index.cshtml` — hero search bar, room type chips, bento grid, newsletter
- `Views/Explore/Index.cshtml` — search bar, filter row, destination cards, available rooms
- `Views/Booking/MyBookings.cshtml` — booking cards with status tabs
- `Views/Room/Index.cshtml` — room grid with filter tabs and search
- `Views/Room/Details.cshtml` — room detail page with booking sidebar
- `Views/Wishlist/Index.cshtml` — saved rooms grid with sessionStorage
- `Views/Support/Index.cshtml` — FAQ accordion, topic dropdown, contact cards

---

## Week 2 — June 1–14, 2026 *(extended)*

### Sprint Goal
Complete all guest-facing UI, fix critical bugs, add runtime compilation, and prepare for Supabase wiring.

### Completed

#### Infrastructure
- Installed `Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation` v8.0.0
- Updated `Program.cs` to enable live `.cshtml` reloading without rebuild
- Excluded `RouteTests.cs` and `UnitTest1.cs` from main project (belong in test project)
- Fixed `StayWithMeh.Tests` NuGet references: `xunit` v2.9.3, `Microsoft.AspNetCore.Mvc.Testing` v8.0.0
- Confirmed Razor runtime compilation working — browser refresh reflects file changes instantly

#### Explore Page
- Rebuilt destination carousel from `overflow-x: auto` scroll to CSS `translateX` carousel
- Added all 8 global branches: New York, London, Tokyo, Dubai, Caracas, Addis Ababa, Melbourne, Singapore
- Added local city images: `caracasVEN.jpg`, `addisAbabaETH.jpg`, `melbourneAUS.jpg`, `MarinaBaySG.jpg`
- Fixed carousel scroll — `width: max-content` on track, `window.onload` timing, dynamic visible count
- Active branch filter — clicking a city card highlights it, dims others, fills destination input
- Arrow buttons with infinite index-wrap (next from Singapore → New York)
- Updated trust bar to reflect 8 branches

#### Room Pages
- `Room/Index.cshtml` — replaced "Book Now" navigation with inline booking modal popup
- Modal populated from card `data-*` attributes (no server round trip)
- Modal carousel: slide 1 = room's own image, slides 2–4 = `BookingSampleImage` assets
- Real-time price calculator in modal (base rate × nights + 12% tax + levy)
- "Details" button preserved for full `Room/Details` page navigation
- Escape key and backdrop click close the modal
- Replaced all Unsplash URLs with local `BookingSampleImage` assets in `Details.cshtml`

#### Support Page
- Fixed broken `DD_OPTIONS` array (missing closing quotes on 4 labels broke entire JS block)
- Dropdown now opens and selects correctly
- Removed emoji check/X marks from FAQ content
- Rewrote all FAQ descriptions to be formal and descriptive
- Added Room & Amenities and Booking Modifications as new FAQ entries (matching all 8 dropdown options)

#### RoomController
- Updated `Details()` action with static room catalogue (rooms 1–6)
- Passes `RoomType`, `Floor`, `RoomNumber`, `Price`, `BasePrice` via `ViewData`
- `Book()` POST action stubbed and ready for Supabase wiring

### Issues Resolved

| Issue | Cause | Fix |
|---|---|---|
| Carousel buttons doing nothing | `carouselTrack` and `btnNext` null at runtime | `width: max-content` on track + `window.onload` init |
| Browser serving stale views | Compiled `.dll` serving old Razor output | Razor runtime compilation + Clean/Rebuild |
| Build failed — 25+ errors | `RouteTests.cs` and `UnitTest1.cs` in wrong project | Excluded from main project |
| Support dropdown not opening | Unclosed string literals in `DD_OPTIONS` JS array | Fixed all 4 missing closing quotes |
| `RazorRuntimeCompilation` version conflict | NuGet defaulted to v10.0.9 (requires .NET 10) | Installed v8.0.0 explicitly |

---

## Week 3 — June 15–21, 2026

### Sprint Goal
Wire all guest-facing pages to real Supabase data and implement the full booking flow end-to-end.

### Completed

#### Models
- `Models/Room.cs` — mapped to `rooms` table, includes `PropertyId`, `BasePrice`, `Floor`
- `Models/Guest.cs` — mapped to `guests` table
- `Models/Booking.cs` — mapped to `bookings` table
- `Models/Invoice.cs` — mapped to `invoices` table
- `Models/Property.cs` — new model for the 8-branch `properties` table
- `Models/ExploreViewModel.cs` — bundles `Properties` and `Rooms` for the Explore page
- `Models/BookingRequest.cs` — DTO for the `/Room/Book` POST payload
- `Models/MyBookingsViewModel.cs` — bundles bookings with a room lookup dictionary for image/type display

#### Database
- Fixed table name mismatch: renamed `invoice` → `invoices`
- Added `base_price` and `property_id` columns to `rooms`
- Created `properties` table with FK constraint `fk_rooms_property`
- Added `GRANT` statements for `anon`/`authenticated` roles ahead of Supabase's October 2026 Data API enforcement
- Seeded 8 properties (all branches) and 6 rooms (all linked to New York, with real `base_price` values)
- Resolved duplicate-seed issue by wiping and reseeding both tables via combined `sql/seed_all.sql`
- Final verified state: 8 properties, 6 rooms

#### Supabase Connection
- `Program.cs` reads `Supabase:Url` and `Supabase:Key` from User Secrets
- `Supabase.Client` registered as a singleton via DI, initialized with `AutoConnectRealtime = true`
- Documented secrets split in `SETUP.md`: User Secrets for anon key (ASP.NET app), `pricing/.env` for service_role key (Python, deferred)

#### Pages — Live Data
- `Room/Index` — converted to `@model List<Room>`, renders all 6 rooms via `@foreach`, filter tabs aligned to real `room_type` values, booking modal uses real UUIDs
- `Room/Details` — converted to `@model Room`, all `ViewData` reads replaced with `@Model.*`, feature pills generated per room type, carousel slide 1 uses the room's real image
- `Explore/Index` — converted to `@model ExploreViewModel`, 8 branch cards render from `properties` with live room counts, "Available Rooms" renders from `rooms`, branch click filters rooms by `property_id`
- `Booking/MyBookings` — converted to `@model MyBookingsViewModel`, shows empty state when no bookings exist, renders real booking cards with room image/type lookup
- `Wishlist/Index` — confirmed working as-is; `sessionStorage` already keys off `data-room-id`, which now holds real Supabase UUIDs

#### Booking Flow
- `RoomController.Book()` POST action — validates input, recalculates total server-side (base price × nights + 12% tax + $5/night levy), looks up guest by email or creates a new `guests` row, inserts `Booking` (`status = "upcoming"`) and linked `Invoice`, returns JSON with `bookingId`, `total`, `nights`
- Added email field to both booking forms (`Room/Index` modal and `Room/Details`) — used for guest lookup-or-create, designed to be replaced by login state once auth exists
- `submitBooking()` and `submitModalBooking()` now POST to `/Room/Book` via `fetch()`
- `[FromBody]` added to `Book()` for correct JSON binding; `[ValidateAntiForgeryToken]` removed since it's incompatible with JSON `fetch()` POSTs and not meaningfully protective without auth — revisit once login exists

#### Folder Cleanup
- Moved and renamed `RoomDetails&Booking/DetailsAndBooking.cshtml` → `Room/Details.cshtml` to match MVC convention

### Design Decisions

- **Guest identity without auth:** booking forms collect name + email; the controller looks up an existing `guests` row by email or creates one.
- **Branch-aware room filtering:** Explore page filters rooms by `property_id` rather than by name matching, so additional branches can have rooms added later with zero further wiring.
- **Server-side price recalculation:** the booking total is never trusted from the client; `RoomController.Book()` recomputes it from `room.BasePrice` to prevent tampering.

---

## Week 4 — June 22–28, 2026 *(started early, June 16–19)*

### Sprint Goal
Implement Role-Based Access Control end-to-end: database infrastructure, JWT auth, four staff dashboards, and login/register pages tying it all together.

### Completed

#### RBAC Database Infrastructure
- `sql/rbac_migration.sql` — `hotel_role` enum (SuperAdmin, Manager, Receptionist, Housekeeper, Guest), `user_profiles` table linked to `auth.users` and `properties.id` (branch scoping), RLS policies (own-profile select, staff-select-all, insert-on-signup)
- `handle_new_user()` trigger — auto-creates a Guest-role profile on every Supabase Auth signup
- Fixed infinite RLS recursion on `user_profiles` (`staff_select_all_profiles` was querying its own table from inside its policy) via a `SECURITY DEFINER` helper function `get_my_role()` that bypasses RLS internally
- Refined `bookings` SELECT policy to check role/ownership instead of being fully open; added temporary `anon`-scoped policies on `bookings`/`rooms`/`guests`/`invoices`/`user_profiles` since no login session exists yet during dashboard development — flagged to drop once auth is live

#### Custom JWT Claims
- `sql/custom_claims_hook.sql` — `custom_access_token_hook(event jsonb)` Postgres function, injects `user_role` and `branch_id` into every JWT Supabase Auth issues, reading from `user_profiles`
- Activated in Supabase Dashboard → Authentication → Hooks → Customize Access Token (JWT) Claims
- Two bugs found and fixed post-activation:
  - `type "hotel_role" does not exist` — `supabase_auth_admin`'s default `search_path` doesn't include `public`; fixed with explicit `SET search_path = public` on the function
  - Claim name collision — Supabase's JWT already reserves a built-in `"role"` claim fixed to `"authenticated"`; our hook was colliding with it, so every login read back `"authenticated"` instead of the real hotel role. Renamed the custom claim to `"user_role"`.

#### Database Schema Additions for Dashboards
- `floor` column added to `rooms`; status vocabulary expanded from available/unavailable to available/occupied/dirty/in_progress/maintenance
- `staff_shifts` table — standalone (no FK to `user_profiles`, since real staff Auth accounts don't exist yet), seeded with 3 realistic shifts for the Manager dashboard
- `housekeeping_tasks` table — real task queue (room_id, task_type, priority, status, assigned_to), seeded with 2 realistic tasks
- SuperAdmin role-management policies — `superadmin_update_profiles` lets a SuperAdmin change any user's role

#### Four Staff Dashboards (all live-data, Tailwind, shared `_StaffLayout.cshtml`)
- **Receptionist** (`/Receptionist`) — Room Status Grid with floor filter tabs and status legend, room cards color-coded by status, click-to-update modal (`UpdateRoomStatus` POST), Occupancy Peak card, Upcoming Arrivals panel
- **Manager** (`/Manager`, `/Manager/Analytics`) — Staff Operations landing page (shift schedule, Floor 1 overview, Manager Toolkit, Recent Tickets clearly labeled as demo data), Analytics page (real occupancy snapshot, Revenue by Room Type, Staff Management table, honest placeholders for Guest Satisfaction/AI Pricing since no review system or pricing engine exists yet)
- **Housekeeping** (`/Housekeeping`) — stat cards (Total/Dirty/In Progress/Cleaned Today), Priority Tasks panel with working Start Now/Mark Complete actions, Room Inventory grid with Set In Progress/Mark Clean actions
- **SuperAdmin/Owner** (`/SuperAdmin`) — Role Management table (live dropdown → `UpdateRole` POST, SuperAdmin excluded from self-service promotion for safety), Privileged Accounts list, Financial Transactions log, honest placeholders for Net Profit Margin and Freeze All Outgoing (require cost-tracking/system-lock features that don't exist yet)
- `_StaffLayout.cshtml` made role-aware via `ViewData["StaffRoleContext"]` — each controller sets its own nav context; Housekeeping gets a distinct 3-item sidebar, SuperAdmin gets quick-links to all other dashboards

#### Login & Register (Account controller)
- `Models/LoginViewModel.cs`, `Models/RegisterViewModel.cs`
- `AccountController.Login()` — calls `_supabase.Auth.SignInWithPassword()`, decodes the returned JWT once with `JwtSecurityTokenHandler`, reads the `user_role` claim, builds a `ClaimsPrincipal`, and issues a standard ASP.NET Core auth cookie via `HttpContext.SignInAsync()` (chosen over re-validating the raw JWT on every request — see Design Decisions)
- `AccountController.Register()` — calls `_supabase.Auth.SignUp()`, relies on the `handle_new_user()` trigger for the initial Guest profile, then updates `full_name`
- `Program.cs` switched default auth scheme to Cookie Authentication; JWT Bearer (JWKS-based, no shared secret) kept registered alongside it for potential future API use
- **In progress:** role-based redirect after login (`RedirectByRole()`) currently sends users to the guest landing page instead of their dashboard — actively debugging which claim/value is actually present at decode time

#### Domain Gotcha
- Initial test accounts used a fictional `@staywithmeh.com` email domain, which silently broke Supabase's password-recovery/email-validation flow (`"Email address is invalid"`) and contributed to early `invalid_credentials` confusion. Recreated test accounts on a real domain (`manager.test@gmail.com`, `owner.test@gmail.com`) — noted here so the lesson isn't lost.

### Design Decisions

- **Cookie auth over raw JWT re-validation (Option A):** since this is a server-rendered MVC app and not a SPA/external API consumer, the Supabase JWT is decoded once at login time and its claims are baked into a standard ASP.NET Core auth cookie. `[Authorize(Roles="...")]` and `User.IsInRole(...)` work with zero extra plumbing. Trade-off: role changes don't take effect until the next login, since the cookie isn't re-checked against Supabase per request — acceptable for a prototype.
- **SuperAdmin excluded from the Role Management dropdown:** promoting an account to SuperAdmin stays a manual SQL action rather than a UI button, since it's too sensitive to expose without an audit trail.
- **Honest placeholders over fabricated data:** any dashboard metric without a real backing table (AI Pricing Insight, Guest Satisfaction, Net Profit Margin, Recent Tickets in early Manager build) is shown clearly disabled/labeled rather than faked, so the prototype never claims to know something it doesn't.

---

## Progress Tracker

### Pages
```
✅ Guest Landing Page (Home)
✅ Explore Destinations & Rooms (live Supabase data)
✅ My Bookings (live data + room image lookup + empty state)
✅ Room Browse (live data + booking modal)
✅ Room Details & Booking (live data + booking form)
✅ Saved Rooms Wishlist (UUID-compatible)
✅ Customer Support Center
✅ Receptionist Dashboard (room grid, status updates, arrivals)
✅ Manager Dashboard (Staff Operations + Analytics)
✅ Housekeeping Dashboard (tasks, room inventory)
✅ SuperAdmin/Owner Dashboard (role management, financial log)
✅ Login / Register pages (auth working, role redirect in progress)
```

### Backend
```
✅ Supabase project created
✅ Database tables (properties, rooms, guests, bookings, invoices,
   user_profiles, staff_shifts, housekeeping_tasks)
✅ Supabase C# package installed and wired in Program.cs
✅ C# Models for all tables above
✅ Live data fetching across every guest page and staff dashboard
✅ Booking POST to Supabase (guests, bookings, invoices)
✅ RBAC: hotel_role enum, user_profiles, auto-profile trigger
✅ Custom JWT claims hook (user_role, branch_id) — active and verified
✅ Cookie-based auth via AccountController
🔶 Role-based redirect after login — debugging in progress
⬜ [Authorize(Roles="...")] gating on staff controllers
⬜ Conditional nav in guest _Layout.cshtml based on role
⬜ Python AI pricing microservice
```

### Infrastructure
```
✅ Visual Studio 2022 configured
✅ GitHub repo created
✅ .gitignore protecting secrets
✅ User Secrets for API keys
✅ Razor Runtime Compilation (live reload)
⬜ Playwright E2E tests
⬜ Deployment setup (Render)
```

---

## Known Technical Debt

- **Permissive RLS policies** added temporarily on `bookings`, `rooms`, `guests`, `invoices`, `user_profiles` (`anon`-scoped, `USING (true)`) to support dashboard development before login existed. Now that auth works, these should be tightened to rely on `get_my_role()` / `auth.uid()` checks only.
- **No email format validation** on guest booking forms.
- **Hardcoded staff display names** (e.g. "Front Desk", "Alex Mercer") in dashboard controllers — needs to read from the logged-in user's `user_profiles` row once `[Authorize]` is wired up.
- **`staff_shifts` and `housekeeping_tasks`** use denormalized `staff_name`/`assigned_to` text fields instead of a FK to `user_profiles`, since real staff Auth accounts didn't exist at the time they were built. Worth migrating once staff accounts are routinely created via Register + SuperAdmin promotion.

---

*Last updated: June 19, 2026*
*Next update: End of Week 4*