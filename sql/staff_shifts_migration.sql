-- Tracks who's working today and when, so the Manager dashboard
-- can show a real shift schedule instead of static placeholder names.
--
-- No FK to user_profiles for now — that table requires a real Supabase
-- Auth account per row, and staff accounts don't exist yet. This table
-- is denormalized on purpose; once login is built and real staff sign up,
-- this can be migrated to reference user_profiles.id properly.
CREATE TABLE public.staff_shifts (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    staff_name TEXT NOT NULL,
    department TEXT NOT NULL, -- Front Desk, Housekeeping, Maintenance, etc.
    shift_date DATE NOT NULL DEFAULT CURRENT_DATE,
    start_time TIME NOT NULL,
    end_time TIME NOT NULL,
    status TEXT NOT NULL DEFAULT 'scheduled', -- active, pending, completed
    created_at TIMESTAMP WITH TIME ZONE DEFAULT TIMEZONE('utc'::text, NOW())
);

ALTER TABLE public.staff_shifts ENABLE ROW LEVEL SECURITY;

CREATE POLICY "anon_select_staff_shifts"
ON public.staff_shifts FOR SELECT
TO anon, authenticated
USING (true);

CREATE POLICY "anon_insert_staff_shifts"
ON public.staff_shifts FOR INSERT
TO anon, authenticated
WITH CHECK (true);

INSERT INTO public.staff_shifts (staff_name, department, start_time, end_time, status)
VALUES
    ('Jordan Smith',    'Front Desk',   '08:00', '16:00', 'active'),
    ('Elena Rodriguez', 'Housekeeping', '09:00', '17:00', 'active'),
    ('Marcus Chen',     'Maintenance',  '15:00', '23:00', 'pending');
