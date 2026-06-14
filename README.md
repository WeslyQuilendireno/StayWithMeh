# StayWithMeh — Weekly Sprint Log

**Project:** Hotel Chain Management System
**Developer:** Wesly Quilendireno
**Stack:** ASP.NET Core MVC · C# · Supabase · Tailwind CSS · Razor Views
**Repository:** https://github.com/WeslyQuilendireno/StayWithMeh

---

## Project Overview

A full-stack web-based Hotel Chain Management System supporting multiple user roles (Guest, Receptionist, Manager, Housekeeper). Built for educational/personal use with a focus on clean architecture, real-time features, and AI-powered room pricing. StayWithMeh operates across **8 global branches**: New York, London, Tokyo, Dubai, Caracas, Addis Ababa, Melbourne, and Singapore.

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
- `Models/Room.cs` — mapped to `rooms` table, includes `PropertyId` and `BasePrice`
- `Models/Guest.cs` — mapped to `guests` table
- `Models/Booking.cs` — mapped to `bookings` table
- `Models/Invoice.cs` — mapped to `invoices` table
- `Models/Property.cs` — new model for the 8-branch `properties` table
- `Models/ExploreViewModel.cs` — bundles `Properties` and `Rooms` for the Explore page
- `Models/BookingRequest.cs` — DTO for the `/Room/Book` POST payload

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
- `Booking/MyBookings` — converted to `@model List<Booking>`, shows empty state when no bookings exist, renders real booking cards (dates, nights, total, status) when present
- `Wishlist/Index` — confirmed working as-is; `sessionStorage` already keys off `data-room-id`, which now holds real Supabase UUIDs

#### Booking Flow
- `RoomController.Book()` POST action — validates input, recalculates total server-side (base price × nights + 12% tax + $5/night levy), looks up guest by email or creates a new `guests` row, inserts `Booking` (`status = "upcoming"`) and linked `Invoice`, returns JSON with `bookingId`, `total`, `nights`
- Added email field to both booking forms (`Room/Index` modal and `Room/Details`) — used for guest lookup-or-create, designed to be replaced by login state once auth exists
- `submitBooking()` and `submitModalBooking()` now POST to `/Room/Book` via `fetch()`, with loading state and server-calculated total in the success overlay
- Configured `AddAntiforgery` with `HeaderName = "RequestVerificationToken"` in `Program.cs` to support JSON POSTs from fetch

#### Folder Cleanup
- Moved and renamed `RoomDetails&Booking/DetailsAndBooking.cshtml` → `Room/Details.cshtml` to match MVC convention (`RoomController.Details()` now resolves the view automatically)

### Design Decisions

- **Guest identity without auth:** booking forms collect name + email; the controller looks up an existing `guests` row by email or creates one. This is intentionally structured so that when a login/signup modal is added later, it becomes the source of the email instead of a raw form field — no booking logic changes required.
- **Branch-aware room filtering:** Explore page filters rooms by `property_id` rather than by name matching, so additional branches can have rooms added later with zero further wiring.
- **Server-side price recalculation:** the booking total is never trusted from the client; `RoomController.Book()` recomputes it from `room.BasePrice` to prevent tampering.

---

## Progress Tracker

### Pages
```
✅ Guest Landing Page (Home)
✅ Explore Destinations & Rooms (live Supabase data)
✅ My Bookings (live data + empty state)
✅ Room Browse (live data + booking modal)
✅ Room Details & Booking (live data + booking form)
✅ Saved Rooms Wishlist (UUID-compatible)
✅ Customer Support Center
⬜ Receptionist Bookings Dashboard
⬜ Rooms Management Dashboard
⬜ Manager Analytics Dashboard
```

### Backend
```
✅ Supabase project created
✅ Database tables created (properties, rooms, guests, bookings, invoices)
✅ Supabase C# package installed and wired in Program.cs
✅ C# Models created (Room, Guest, Booking, Invoice, Property)
✅ Live data fetching across Explore, Room/Index, Room/Details, MyBookings
✅ Booking POST to Supabase (guests, bookings, invoices)
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

## Week 4 — June 22–28, 2026

### Sprint Goal
TBD — candidates: Guest Auth (soft-gate login modal), Receptionist/Manager dashboards, or Python pricing microservice.

### Planned Tasks
- [ ] Guest login/register modal triggered on Book Now click or after a delay
- [ ] Replace email-based guest lookup with authenticated session once login exists
- [ ] `MyBookings` — scope query to logged-in guest
- [ ] Begin Receptionist Bookings Dashboard or Python pricing microservice (TBD)

---

*Last updated: June 12, 2026*
*Next update: End of Week 4*