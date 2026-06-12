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
