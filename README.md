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

### Planned Tasks

#### Models
- [ ] `Models/Room.cs` — map to `rooms` Supabase table
- [ ] `Models/Guest.cs` — map to `guests` table
- [ ] `Models/Booking.cs` — map to `bookings` table
- [ ] `Models/Invoice.cs` — map to `invoices` table

#### Supabase Connection
- [ ] Configure Supabase client in `Program.cs` using User Secrets
- [ ] Create `Services/SupabaseService.cs` as a singleton wrapper

#### Pages — Live Data
- [ ] `Explore/Index` — fetch rooms from Supabase, replace static cards
- [ ] `Room/Index` — fetch rooms filtered by status
- [ ] `Room/Details` — display selected room's real data
- [ ] `Booking/MyBookings` — fetch guest bookings from Supabase
- [ ] `Wishlist/Index` — persist saved rooms to Supabase instead of sessionStorage

#### Booking Flow
- [ ] Wire `RoomController.Book()` POST to insert into `bookings` table
- [ ] Generate invoice record on booking confirmation
- [ ] Add `PropertyId` to room catalogue linking rooms to branches

#### Authentication (stretch goal)
- [ ] Guest login / register flow using Supabase Auth

---

## Progress Tracker

### Pages
```
✅ Guest Landing Page (Home)
✅ Explore Destinations & Rooms
✅ My Bookings
✅ Room Browse (with booking modal)
✅ Room Details & Booking
✅ Saved Rooms Wishlist
✅ Customer Support Center
⬜ Receptionist Bookings Dashboard
⬜ Rooms Management Dashboard
⬜ Manager Analytics Dashboard
```

### Backend
```
✅ Supabase project created
✅ Database tables created (rooms, guests, bookings, invoices)
✅ Supabase C# package installed
✅ RoomController with static catalogue (rooms 1–6)
⬜ C# Models created
⬜ Supabase connection in Program.cs
⬜ Live data fetching
⬜ Booking POST to Supabase
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

*Last updated: June 11, 2026*
*Next update: End of Week 3*