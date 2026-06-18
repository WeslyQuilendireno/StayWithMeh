-- Lets a SuperAdmin view and update any user's role from the dashboard.
-- The existing staff_select_all_profiles policy already allows
-- SuperAdmin/Manager to SELECT all profiles via get_my_role().
-- This adds the matching UPDATE policy so role changes can actually be saved.
CREATE POLICY "superadmin_update_profiles"
ON public.user_profiles
FOR UPDATE
USING (public.get_my_role() = 'SuperAdmin')
WITH CHECK (public.get_my_role() = 'SuperAdmin');

-- Temporary: same pattern as bookings/rooms earlier — no login session exists
-- yet for testing this dashboard, so open SELECT/UPDATE to anon for now.
-- Drop both once login is wired up and the SuperAdmin policy above takes over.
CREATE POLICY "temp_anon_select_profiles"
ON public.user_profiles FOR SELECT
TO anon
USING (true);

CREATE POLICY "temp_anon_update_profiles"
ON public.user_profiles FOR UPDATE
TO anon
USING (true) WITH CHECK (true);

-- No automatic SuperAdmin promotion here — create a new test user via
-- Supabase Dashboard (Authentication > Users > Add user), e.g.
-- owner@staywithmeh.com, then run:
--
--   UPDATE public.user_profiles
--   SET role = 'SuperAdmin', full_name = 'Wesly Quilendireno'
--   WHERE email = 'owner@staywithmeh.com';
--
-- This keeps manager@staywithmeh.com intact as a separate Manager account.
