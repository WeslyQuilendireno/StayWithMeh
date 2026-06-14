CREATE TYPE hotel_role AS ENUM ('SuperAdmin', 'Manager', 'Receptionist', 'Housekeeper', 'Guest');

CREATE TABLE public.user_profiles (
    id UUID REFERENCES auth.users NOT NULL PRIMARY KEY,
    email TEXT NOT NULL,
    full_name TEXT,
    role hotel_role NOT NULL DEFAULT 'Guest',
    branch_id UUID REFERENCES public.properties(id),
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT TIMEZONE('utc'::text, NOW())
);

ALTER TABLE public.user_profiles ENABLE ROW LEVEL SECURITY;

-- Anyone authenticated can read their own profile (needed for role checks in the app)
CREATE POLICY "users_select_own_profile"
ON public.user_profiles
FOR SELECT
USING (auth.uid() = id);

-- Staff roles can view all profiles (needed for admin/manager screens later)
CREATE POLICY "staff_select_all_profiles"
ON public.user_profiles
FOR SELECT
USING (
    (SELECT role FROM public.user_profiles WHERE id = auth.uid()) IN ('SuperAdmin', 'Manager')
);

-- A new row is inserted automatically on signup via trigger below;
-- this policy lets that trigger's insert succeed.
CREATE POLICY "allow_profile_insert_on_signup"
ON public.user_profiles
FOR INSERT
WITH CHECK (auth.uid() = id);

-- Auto-create a user_profiles row whenever someone signs up via Supabase Auth
CREATE OR REPLACE FUNCTION public.handle_new_user()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO public.user_profiles (id, email, role)
    VALUES (NEW.id, NEW.email, 'Guest');
    RETURN NEW;
END;
$$ LANGUAGE plpgsql SECURITY DEFINER;

CREATE TRIGGER on_auth_user_created
    AFTER INSERT ON auth.users
    FOR EACH ROW EXECUTE FUNCTION public.handle_new_user();

-- Refine bookings RLS now that user_profiles exists:
-- staff (SuperAdmin/Manager/Receptionist) see all bookings,
-- guests see only their own (matched by guest_id -> guests.id,
-- which is separate from auth.users.id until guests are linked to accounts)
DROP POLICY IF EXISTS "anon_select_bookings" ON public.bookings;

CREATE POLICY "staff_or_owner_select_bookings"
ON public.bookings
FOR SELECT
USING (
    (SELECT role FROM public.user_profiles WHERE id = auth.uid()) IN ('SuperAdmin', 'Manager', 'Receptionist')
    OR
    EXISTS (
        SELECT 1 FROM public.guests g
        WHERE g.id = bookings.guest_id AND g.email = (auth.jwt() ->> 'email')
    )
);

GRANT SELECT, INSERT, UPDATE ON public.user_profiles TO anon, authenticated;
