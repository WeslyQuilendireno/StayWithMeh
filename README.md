# StayWithMeh — Weekly Sprint Log

**Project:** SmartStay Hotel Management System  
**Developer:** Wesly Quilendireno  
**Stack:** ASP.NET Core MVC · C# · Supabase · Tailwind CSS · Python  
**Repository:** https://github.com/WeslyQuilendireno/StayWithMeh

---

## Project Overview

A full-stack web-based Hotel Management System supporting multiple user roles (Guest, Receptionist, Manager, Housekeeper). Built for educational/personal use with a focus on clean architecture, real-time features, and AI-powered room pricing.

---

## Week 1 — May 24–31, 2026

### Sprint Goal
Set up the project foundation, design system, database, and implement the Guest Landing Page.

---

### Completed Tasks

#### System Design & Planning
- Defined system requirements and core actors (Guest, Receptionist, Manager, Housekeeper, System)
- Designed OOD class diagram with key classes: Hotel, Room, RoomBooking, RoomKey, HouseKeeping, RoomCharge, Invoice
- Mapped activity diagrams for: Room Booking, Check-in, Cancel Booking, Notifications
- Planned architecture: C# MVC as main app + Python microservice for AI pricing
- Chose Supabase as Backend-as-a-Service (BaaS) for PostgreSQL + Realtime

#### UI/UX Design
- Designed full prototype in **Google Stitch** (SmartStay Hotel Management System)
- Completed designs for 13 screens:
  - Guest Landing Page
  - Explore Destinations & Rooms
  - My Bookings (x2 variants)
  - Room Details & Booking
  - Saved Rooms Wishlist
  - Bookings Dashboard
  - Rooms Dashboard (Kanban + Sidebar variants)
  - Rooms Management Dashboard
  - Manager Analytics Dashboard
  - Customer Support Center
- Established design system: Navy `#031635` primary, Inter font, Material Symbols icons, Tailwind CSS

#### Development Environment Setup
- Installed **Visual Studio 2022 Community**
- Configured workloads: ASP.NET and web development · Python development · Desktop development with C++
- Created ASP.NET Core MVC project: `StayWithMeh` (.NET 8.0 LTS)
- Installed NuGet packages: `supabase-csharp` v0.16.2 · `DotNetEnv` v3.2.0
- Configured **User Secrets** for Supabase credentials (never committed to Git)
- Set up `.gitignore` to exclude `secrets.json` and sensitive files
- Created **private GitHub repository**: `WeslyQuilendireno/StayWithMeh`
- Made initial commit and push

#### Database Setup (Supabase)
- Created Supabase project: **HotelMS** (Region: Northeast Asia — Tokyo)
- Created 4 database tables with RLS + Realtime enabled on all:

| Table | Columns | Relations |
|---|---|---|
| `rooms` | id, room_number, room_type, status, price_per_night, description, image_url, created_at | — |
| `guests` | id, full_name, email (unique), phone, created_at | — |
| `bookings` | id, guest_id, room_id, check_in, check_out, status, total_amount, created_at | → guests, rooms |
| `invoices` | id, booking_id, amount, payment_method, payment_status, created_at | → bookings |

#### Guest Landing Page Implementation
- Created `Views/Shared/_Layout.cshtml` — fixed navbar, active page highlighting, scroll shadow, full footer
- Created `Views/Home/Index.cshtml` — hero section, search bar, bento grid (4 room types), newsletter, trust indicators
- Added custom images to `wwwroot/images/guest_landing_page_image/`: Chandelier, StandardRoom, DeluxeSuite, FamilySuite, BusinessSuite

---

### Progress Tracker — End of Week 1

#### Pages
```
✅ Guest Landing Page
⬜ Explore Destinations & Rooms
⬜ My Bookings
⬜ Room Details & Booking
⬜ Saved Rooms Wishlist
⬜ Customer Support Center
⬜ Receptionist/Bookings Dashboard
⬜ Rooms Dashboard (Kanban)
⬜ Rooms Dashboard (Sidebar)
⬜ Rooms Management
⬜ Manager Analytics Dashboard
```

#### Backend
```
✅ Supabase project created
✅ Database tables created (rooms, guests, bookings, invoices)
✅ Supabase C# package installed
⬜ C# Models created
⬜ Program.cs Supabase connection
⬜ Supabase data fetching working
⬜ Python AI pricing microservice
```

#### Infrastructure
```
✅ Visual Studio 2022 configured
✅ GitHub private repo created
✅ .gitignore protecting secrets
✅ User Secrets for API keys
⬜ Deployment setup
```

---

### Notes & Decisions
- Chose **Tailwind CSS via CDN** for rapid prototyping — will consider build pipeline later
- Using **User Secrets** locally and **Environment Variables** for deployment
- Room images stored in `wwwroot/images/` — will evaluate Supabase Storage for production
- Python microservice planned for Week 4–5 after core booking flow is complete

---

## Week 2 — June 1–7, 2026

### Sprint Goal
Make all guest-facing pages fully functional, connect navigation across the entire app, implement an interactive search experience on the landing page, and set up the testing infrastructure.

---

### Completed Tasks

#### Navigation & Layout
- Updated `Views/Shared/_Layout.cshtml`:
  - Connected all 6 nav links: Home → Explore → Bookings → Rooms → Saved → Support
  - Implemented **sliding underline animation** that transitions smoothly between active nav items
  - Fixed z-index layering — header now always renders above page dropdowns
  - "Add Booking" button connected to My Bookings page
  - Removed `overflow-hidden` from hero section to allow search dropdowns to escape clipping

#### Landing Page — Interactive Search Bar
- Rebuilt `Views/Home/Index.cshtml` search bar with full interactivity:
  - **Where To?** — destination dropdown with country flags (🇺🇸 New York · 🇬🇧 London · 🇯🇵 Tokyo · 🇦🇪 Dubai)
  - **Check-in / Check-out** — dual-month calendar date picker with range highlighting and night counter
  - **Guests & Rooms** — adult/children/rooms counters with smart room type suggestions based on party size
  - **Room Type filter chips** — All / Standard / Deluxe Suite / Family Suite / Business Suite
  - Search button builds URL params and redirects to `/Explore`
  - Panels auto-advance (destination → dates → guests) for a smooth booking flow
  - All dropdowns use `fixed` positioning to prevent z-index clipping

#### Explore Page
- Built `Views/Explore/Index.cshtml`:
  - Search bar with destination, date, guest inputs carrying over from landing page params
  - **Popular Destinations** — 4 rectangle cards using local images (newyorkUS.jpg, londonUK.jpg, tokyoJP.jpg, dubaiUAE.jpg)
  - Horizontal scroll with left/right arrow navigation
  - **Available Rooms** grid — 3 cards (Skyline Executive Suite · Urban Deluxe King · Grand Family Loft) with local images
  - Status badges — green Available (pulse animation) / red Fully Booked
  - Grid/List toggle, price range slider, amenity checkboxes, sort dropdown
  - Flash Offer card (Business Trip Save 25%) + Operational Standards grid
  - Trust bar (50,000+ travelers, 98% satisfaction)
- Created `ExploreController.cs`
- Added images to `wwwroot/images/explore_page/`

#### My Bookings Page
- Built `Views/Booking/MyBookings.cshtml`:
  - 3 booking cards with real room photos:
    - **UPCOMING** — Skyline Executive Suite · Booking #SWM-20240524 · Check-in Online + Cancel Booking
    - **ACTIVE** — Urban Deluxe King · Booking #SWM-20240520 · Currently staying indicator
    - **COMPLETED** — Grand Family Loft · Booking #SWM-20240510 · Book Again + Download Invoice
  - Tab filter (All / Upcoming / Active / Completed / Cancelled) with client-side JavaScript filtering
  - Booking detail grid (dates, duration, guests, room number) per card
  - Pagination (static)
- Created `BookingController.cs`
- Added booking room images to `wwwroot/images/bookings/`

#### Browse Rooms Page
- Built `Views/Room/Index.cshtml`:
  - 6 room cards with local images: Skyline Executive Suite · Superior Twin Room · Deluxe Ocean View · Standard King Room · Family Connection Suite · Executive Studio
  - Tab filter — All Rooms / Available / Suites / Standard / Family
  - Real-time search input filtering by room name
  - **Heart save button** — saves room to `sessionStorage` for Wishlist page
  - Each card has `data-room-id`, `data-price`, and `.room-floor` class for JavaScript data extraction
  - Availability badges — green Available / red Unavailable + Join Waitlist
  - AI Priced badge on select rooms
- Created `RoomController.cs`
- Added 6 room images to `wwwroot/images/rooms/Rooms_Dashboard/`

#### Saved Rooms / Wishlist Page
- Built `Views/Wishlist/Index.cshtml`:
  - **Empty state** by default — inbox icon, description, "Explore Rooms" button redirecting to `/Explore`
  - When rooms saved from Browse Rooms: cards render dynamically from `sessionStorage`
  - Remove heart on saved card removes room from `sessionStorage` instantly
  - Item count updates (0 Items / 1 Item / N Items)
  - Share Collection + Add Booking header actions appear when items exist
- Created `WishlistController.cs`

#### Customer Support Page
- Built `Views/Support/Index.cshtml`:
  - Hero section with search bar and **concern category dropdown** (portal pattern — appended to `document.body` to prevent z-index clipping from any ancestor)
  - 8 concern categories: All Topics · Cancellations & Refunds · Check-in Policies · Payments & Billing · Room & Amenities · Wi-Fi & Technical · Booking Modifications · Other
  - Quick pill shortcuts (Cancel Booking · Check-in Time · Refund Status · Wi-Fi Issues · Payment Issues)
  - **FAQ accordion** — 4 topics with rich content:
    - Cancellations & Refunds — full refund/non-refundable grid cards
    - Check-in Policies — 3-column time cards (check-in/check-out/online) + requirements
    - Payments & Billing — payment method cards + security deposit info
    - Wi-Fi & Technical — connection instructions
  - **Empty support requests** state (no tickets submitted)
  - Right sidebar: Live Chat card · Email Support card · CustomerSupport.png banner · Direct Contact (phone/email/hours)
- Created `SupportController.cs`
- Added `CustomerSupport.png` to `wwwroot/images/customer_support_center/`

#### Testing Infrastructure
- Created `StayWithMeh.Tests` xUnit test project
- Installed packages: `Microsoft.NET.Test.Sdk 18.6.0` · `xunit 2.9.3` · `xunit.runner.visualstudio` · `Microsoft.AspNetCore.Mvc.Testing 8.0.0`
- Project references main `StayWithMeh.csproj` (one-way only, no circular dependency)
- `RouteTests.cs` planned: verifies all 6 routes return HTTP 200

---

### Progress Tracker — End of Week 2

#### Pages
```
✅ Guest Landing Page        (interactive search bar)
✅ Explore Destinations      (local images, room grid)
✅ My Bookings               (3 cards, status filter)
⬜ Room Details & Booking    ← next priority
✅ Saved Rooms Wishlist      (sessionStorage)
✅ Customer Support          (FAQ accordion, dropdown)
✅ Browse Rooms              (filter, search, save)
⬜ Receptionist/Bookings Dashboard
⬜ Rooms Dashboard (Kanban)
⬜ Rooms Dashboard (Sidebar)
⬜ Rooms Management
⬜ Manager Analytics Dashboard
```

#### Backend
```
✅ Supabase project created
✅ Database tables created (rooms, guests, bookings, invoices)
✅ Supabase C# package installed
⬜ C# Models created (Room.cs, Guest.cs, Booking.cs, Invoice.cs)
⬜ Program.cs Supabase connection
⬜ Supabase data fetching working
⬜ Python AI pricing microservice
```

#### Infrastructure
```
✅ Visual Studio 2022 configured
✅ GitHub private repo created
✅ .gitignore protecting secrets
✅ User Secrets for API keys
✅ xUnit test project created
⬜ Route tests passing
⬜ Deployment setup
```

---

### Next Sprint Goals (Week 3)
- [ ] Create C# Models (`Room.cs`, `Guest.cs`, `Booking.cs`, `Invoice.cs`) with Supabase `[Table]` attributes
- [ ] Update `Program.cs` with Supabase `Client` dependency injection
- [ ] Seed Supabase `rooms` table with sample data
- [ ] Build **Room Details & Booking** page — images, description, booking form
- [ ] Replace static room cards with real Supabase queries
- [ ] Replace `sessionStorage` wishlist with Supabase `favourites` table
- [ ] Complete `RouteTests.cs` — verify all 6 routes return HTTP 200

---

### Notes & Decisions
- `overflow-hidden` removed from hero section — only applied to the background image div, allowing search dropdowns to escape
- Portal pattern (`document.body.appendChild`) used for Support page dropdown to guarantee it renders above all page content
- `sessionStorage` used for wishlist temporarily — replaced with Supabase after authentication is implemented
- Tailwind config remains embedded in `_Layout.cshtml` — no build step required for this phase
- Test packages (`xunit`, `Microsoft.NET.Test.Sdk`) belong only in `StayWithMeh.Tests.csproj`, never in the main project

---

*Last updated: June 7, 2026*  
*Next update: End of Week 3*
