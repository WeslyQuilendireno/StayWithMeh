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
Set up the project foundation, design system, database, and implement all Guest-facing pages (Home, Explore, Bookings, Room, Saved, Support).

---

### Completed Tasks

#### System Design & Planning
- Defined system requirements and core actors (Guest, Receptionist, Manager, Housekeeper, System)
- Designed OOD class diagram with key classes: Hotel, Room, RoomBooking, RoomKey, HouseKeeping, RoomCharge, Invoice
- Mapped activity diagrams for: Room Booking, Check-in, Cancel Booking, Notifications
- Planned architecture: C# MVC as main app + Python microservice for AI pricing
- Chose Supabase as Backend-as-a-Service (BaaS) for PostgreSQL + Realtime

#### UI/UX Design
- Designed full prototype in **Google Stitch** (Hotel Management System)
- Completed designs for 13 screens:
  - Guest Landing Page
  - Explore Destinations & Rooms
  - My Bookings (x2 variants)
  - Room Details & Booking
  - Saved Rooms Wishlist
  - Bookings Dashboard
  - Rooms Management Dashboard
  - Manager Analytics Dashboard
  - Customer Support Center
- Established design system: Navy `#031635` primary, Inter font, Material Symbols icons, Tailwind CSS

#### Development Environment Setup
- Installed **Visual Studio 2022 Community**
- Configured workloads:
  - ✅ ASP.NET and web development
  - ✅ Python development
  - ✅ Desktop development with C++
- Created ASP.NET Core MVC project: `StayWithMeh` (.NET 8.0 LTS)
- Installed NuGet packages:
  - `supabase-csharp` v0.16.2
  - `DotNetEnv` v3.2.0
- Configured **User Secrets** for Supabase credentials (never committed to Git)
- Set up `.gitignore` to exclude `secrets.json` and sensitive files
- Created **GitHub repository**: `WeslyQuilendireno/StayWithMeh`
- Made initial commit and push

#### Database Setup (Supabase)
- Created Supabase project: **HotelMS** (Region: Northeast Asia - Tokyo)
- Created 4 database tables:

| Table | Columns | Relations |
|---|---|---|
| `rooms` | id, room_number, room_type, status, price_per_night, description, image_url, created_at | — |
| `guests` | id, full_name, email (unique), phone, created_at | — |
| `bookings` | id, guest_id, room_id, check_in, check_out, status, total_amount, created_at | → guests, rooms |
| `invoices` | id, booking_id, amount, payment_method, payment_status, created_at | → bookings |

- Enabled **Row Level Security (RLS)** on all tables
- Enabled **Realtime** on all tables
- Set up Foreign Key relationships: bookings → guests, bookings → rooms, invoices → bookings

#### Guest Pages Implementation
- Created `Views/Shared/_Layout.cshtml`:
  - Fixed top navigation bar (StayWithMeh logo, nav links, Add Booking button)
  - Active page highlighting via `ViewData["ActivePage"]`
  - Scroll effect on header (shadow + blur on scroll)
  - Full footer (4-column: Brand, Destinations, Company, Support)
- Created `Views/Home/Index.cshtml` — Guest Landing Page:
  - Hero section with full-screen hotel lobby image
  - Search bar (Where To, Check-in/Check-out, Guests & Rooms, Search button)
  - Bento grid layout for 4 room types (Standard, Deluxe, Family, Business)
  - Newsletter subscription section
  - Trust indicators (50,000+ bookings, Sustainable Luxury)
- Created `Views/Explore/Index.cshtml` — Explore Destinations & Rooms
- Created `Views/Booking/MyBookings.cshtml` — My Bookings page
- Created `Views/Room/Index.cshtml` — Room Details & Booking
- Created `Views/Wishlist/Index.cshtml` — Saved Rooms Wishlist
- Created `Views/Support/Index.cshtml` — Customer Support Center:
  - Hero search bar with concern category dropdown (8 categories)
  - FAQ accordion (Cancellations & Refunds, Check-in Policies, Payments & Billing)
  - Empty state for "Your Support Requests" (no tickets shown by default)
  - Live Chat and Email Support contact cards
- Added `Controllers/SupportController.cs` to handle `/Support` routing
- Added own custom images to `wwwroot/images/guest_landing_page_image/`:
  - `GuestLandingPageChandelier.png` — Hero background
  - `StandardRoom.png`, `DeluxeSuite.png`, `FamilySuite.png`, `BusinessSuite.png`

---

### Issues Encountered & Fixed

| Issue | Cause | Fix |
|---|---|---|
| NuGet package not found | Package source set to "Offline" | Changed source to `nuget.org` |
| `secrets.json` crash on run | Invalid JSON formatting | Fixed JSON structure |
| C++ templates showing instead of C# | Language filter in project search | Changed dropdown to "C#" |
| Extra folders created in `wwwroot` | Accidental folder creation | Deleted via File Explorer |
| Duplicate `src` on hero image | Old Google URL not removed | Deleted placeholder URL |
| 404 on `/Support` route | Missing SupportController | Added `SupportController.cs` |

---

### Progress Tracker

#### Pages
```
✅ Guest Landing Page
✅ Explore Destinations & Rooms
✅ My Bookings
✅ Room Details & Booking
✅ Saved Rooms Wishlist
✅ Customer Support Center (UI complete)
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
⬜ Controllers created
⬜ Supabase data fetching working
⬜ Python AI pricing microservice
```

#### Infrastructure
```
✅ Visual Studio 2022 configured
✅ GitHub repo created
✅ .gitignore protecting secrets
✅ User Secrets for API keys
⬜ Deployment setup (Render)
```

---

## Week 2 — June 1–7, 2026

### Sprint Goal
Make all guest-facing pages fully functional with real data from Supabase.

### Planned Tasks
- [ ] Create C# Models (`Room.cs`, `Guest.cs`, `Booking.cs`, `Invoice.cs`)
- [ ] Configure Supabase connection in `Program.cs`
- [ ] Wire Explore page to fetch live rooms data from Supabase
- [ ] Wire Room Details page to display selected room info
- [ ] Wire My Bookings page to fetch guest bookings
- [ ] Wire Saved/Wishlist page to persist saved rooms
- [ ] Implement search and filter on Explore page
- [ ] Begin authentication flow (Guest login/register)

---

### Notes & Decisions

- Chose **Tailwind CSS via CDN** for rapid prototyping — will consider build pipeline later
- Using **User Secrets** locally and **Environment Variables** for deployment
- Room images stored in `wwwroot/images/` — will evaluate Supabase Storage for production
- Python microservice planned for Week 4–5 after core booking flow is complete

---

*Last updated: June 1, 2026*
*Next update: End of Week 2*
