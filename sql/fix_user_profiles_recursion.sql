-- Fixes "infinite recursion detected in policy for relation user_profiles"
-- The original staff_select_all_profiles policy queried user_profiles
-- from within its own USING clause, triggering RLS recursively.
--
-- SECURITY DEFINER functions run with the privileges of the function owner,
-- bypassing RLS internally, so checking "what is my own role" no longer
-- re-triggers the policy that called it.

CREATE OR REPLACE FUNCTION public.get_my_role()
RETURNS hotel_role
LANGUAGE sql
SECURITY DEFINER
SET search_path = public
STABLE
AS $$
    SELECT role FROM public.user_profiles WHERE id = auth.uid();
$$;

DROP POLICY IF EXISTS "staff_select_all_profiles" ON public.user_profiles;

CREATE POLICY "staff_select_all_profiles"
ON public.user_profiles
FOR SELECT
USING (public.get_my_role() IN ('SuperAdmin', 'Manager'));

-- bookings policy had the same recursive shape — rewrite using the function too
DROP POLICY IF EXISTS "staff_or_owner_select_bookings" ON public.bookings;

CREATE POLICY "staff_or_owner_select_bookings"
ON public.bookings
FOR SELECT
USING (
    public.get_my_role() IN ('SuperAdmin', 'Manager', 'Receptionist')
    OR
    EXISTS (
        SELECT 1 FROM public.guests g
        WHERE g.id = bookings.guest_id AND g.email = (auth.jwt() ->> 'email')
    )
);
