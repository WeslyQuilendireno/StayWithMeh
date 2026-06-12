-- Seed properties (8 branches)
INSERT INTO properties (name, city, country, flag_emoji, address, image_url) VALUES
('StayWithMeh New York',     'New York',    'United States',        '🇺🇸', '123 5th Avenue, New York, NY 10160',           '/images/explore_page/newyorkUS.jpg'),
('StayWithMeh London',       'London',      'United Kingdom',       '🇬🇧', '45 Tower Bridge Road, London SE1 2UP',         '/images/explore_page/londonUK.jpg'),
('StayWithMeh Tokyo',         'Tokyo',       'Japan',                 '🇯🇵', '2-1 Marunouchi, Chiyoda City, Tokyo 100-0005', '/images/explore_page/tokyoJP.jpg'),
('StayWithMeh Dubai',         'Dubai',       'United Arab Emirates',  '🇦🇪', 'Sheikh Zayed Road, Dubai',                      '/images/explore_page/dubaiUAE.jpg'),
('StayWithMeh Caracas',       'Caracas',     'Venezuela',             '🇻🇪', 'Av. Francisco de Miranda, Caracas 1060',       '/images/explore_page/caracasVEN.jpg'),
('StayWithMeh Addis Ababa',   'Addis Ababa', 'Ethiopia',              '🇪🇹', 'Bole Road, Addis Ababa',                       '/images/explore_page/addisAbabaETH.jpg'),
('StayWithMeh Melbourne',     'Melbourne',   'Australia',             '🇦🇺', '1 Flinders Street, Melbourne VIC 3000',        '/images/explore_page/melbourneAUS.jpg'),
('StayWithMeh Singapore',     'Singapore',   'Singapore',             '🇸🇬', '10 Bayfront Avenue, Singapore 018956',         '/images/explore_page/MarinaBaySG.jpg');

-- Seed rooms (6 rooms, all linked to New York)
INSERT INTO rooms (property_id, room_number, room_type, status, price_per_night, base_price, description, image_url)
SELECT
    p.id,
    v.room_number,
    v.room_type,
    v.status,
    v.price_per_night,
    v.base_price,
    v.description,
    v.image_url
FROM (VALUES
    ('402', 'Business Suite',  'available',   450.00, 400, 'Spacious corner suite with floor-to-ceiling windows overlooking the financial district.', '/images/rooms/Rooms_Dashboard/SkylineExecutiveSuiteRoom.png'),
    ('215', 'Standard',        'available',   210.00, 190, 'A refined space designed for the modern professional with an ergonomic workspace.',         '/images/rooms/Rooms_Dashboard/SuperiorTwinRoom.png'),
    ('601', 'Deluxe Suite',    'unavailable', 325.00, 290, 'Ocean-view suite with king bed, balcony, and premium amenities.',                           '/images/rooms/Rooms_Dashboard/DeluxeOceanViewRoom.png'),
    ('304', 'Standard',        'available',   180.00, 160, 'Comfortable king room with smart TV and complimentary WiFi.',                               '/images/rooms/Rooms_Dashboard/StandardKingRoom.png'),
    ('510', 'Family Suite',    'available',   380.00, 340, 'Dual-level loft with separate sleeping areas and child-friendly amenities.',                '/images/rooms/Rooms_Dashboard/FamilyConnectionSuiteRoom.png'),
    ('102', 'Business Suite',  'available',   150.00, 130, 'Compact executive studio with work desk and pantry.',                                       '/images/rooms/Rooms_Dashboard/ExecutiveStudioRoom.png')
) AS v(room_number, room_type, status, price_per_night, base_price, description, image_url)
CROSS JOIN properties p
WHERE p.city = 'New York';

-- Verify
SELECT (SELECT COUNT(*) FROM properties) AS property_count,
       (SELECT COUNT(*) FROM rooms)      AS room_count;
