-- Add floor number to rooms for the floor filter tabs in the Receptionist dashboard
ALTER TABLE rooms ADD COLUMN floor integer;

-- Expand status values beyond available/unavailable to reflect real room lifecycle:
-- available → ready for new guest
-- occupied  → guest currently staying
-- dirty     → checked out, needs housekeeping
-- maintenance → blocked for repairs, not bookable
-- unavailable → admin-blocked (existing value, kept for compatibility)

-- Update the 6 seeded rooms with realistic floor values derived from room_number
-- (Room 1xx = Floor 1, Room 2xx = Floor 2, etc.)
UPDATE rooms SET floor = 1 WHERE room_number = '102';
UPDATE rooms SET floor = 2 WHERE room_number = '215';
UPDATE rooms SET floor = 6 WHERE room_number = '601';
UPDATE rooms SET floor = 3 WHERE room_number = '304';
UPDATE rooms SET floor = 5 WHERE room_number = '510';
UPDATE rooms SET floor = 4 WHERE room_number = '402';

-- Set one room to each interesting status so the dashboard grid
-- has something to display beyond just "available"
UPDATE rooms SET status = 'occupied'    WHERE room_number = '402';
UPDATE rooms SET status = 'maintenance' WHERE room_number = '601';
UPDATE rooms SET status = 'dirty'       WHERE room_number = '215';
-- 304, 510, 102 remain 'available'
