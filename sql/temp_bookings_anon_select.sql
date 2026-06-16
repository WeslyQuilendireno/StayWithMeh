-- TEMPORARY: allows the Receptionist dashboard to read all bookings
-- without requiring a logged-in session. The role/email-based policy
-- (staff_or_owner_select_bookings) stays in place underneath this —
-- once login is wired up, drop this policy so only that one applies.

CREATE POLICY "temp_anon_select_bookings"
ON bookings FOR SELECT
TO anon
USING (true);
