-- rooms.status is a plain text column (no enum/check constraint exists),
-- so "in_progress" can be used immediately as a value without any DDL change.
-- Noting it here for clarity since it's a new value in the status vocabulary.

-- Real task queue for housekeeping — what needs doing, where, and how urgently.
-- assigned_to is denormalized (plain text name) for the same reason as
-- staff_shifts: no real housekeeper auth accounts exist yet to foreign-key against.
CREATE TABLE public.housekeeping_tasks (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    room_id UUID REFERENCES public.rooms(id),
    task_type TEXT NOT NULL,        -- checkout_clean, stay_over, deep_clean, maintenance
    priority TEXT NOT NULL DEFAULT 'standard', -- urgent, standard
    description TEXT,
    due_at TIMESTAMP WITH TIME ZONE,
    status TEXT NOT NULL DEFAULT 'pending', -- pending, in_progress, completed
    assigned_to TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT TIMEZONE('utc'::text, NOW())
);

ALTER TABLE public.housekeeping_tasks ENABLE ROW LEVEL SECURITY;

CREATE POLICY "anon_select_housekeeping_tasks"
ON public.housekeeping_tasks FOR SELECT
TO anon, authenticated
USING (true);

CREATE POLICY "anon_insert_housekeeping_tasks"
ON public.housekeeping_tasks FOR INSERT
TO anon, authenticated
WITH CHECK (true);

CREATE POLICY "anon_update_housekeeping_tasks"
ON public.housekeeping_tasks FOR UPDATE
TO anon, authenticated
USING (true) WITH CHECK (true);

-- Seed a few realistic tasks matching the prototype, tied to real room ids.
INSERT INTO public.housekeeping_tasks (room_id, task_type, priority, description, due_at, assigned_to)
SELECT id, 'deep_clean', 'urgent', 'Deep clean required. Priority checkout for VIP guest.',
       (CURRENT_DATE + TIME '10:30')::timestamptz, 'Elena Rodriguez'
FROM public.rooms WHERE room_number = '402'
UNION ALL
SELECT id, 'stay_over', 'standard', 'Stay-over service. Refill minibar and fresh towels.',
       (CURRENT_DATE + TIME '11:45')::timestamptz, 'Elena Rodriguez'
FROM public.rooms WHERE room_number = '510';
