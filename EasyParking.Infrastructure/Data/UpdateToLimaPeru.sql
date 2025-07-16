-- Script para actualizar datos existentes a ubicaciones en Lima, Perú
-- Ejecutar después de aplicar las migraciones iniciales

-- Actualizar números de teléfono a formato peruano
UPDATE Users SET PhoneNumber = '+51987654321' WHERE Id = 1;
UPDATE Users SET PhoneNumber = '+51987654322' WHERE Id = 2;
UPDATE Users SET PhoneNumber = '+51987654323' WHERE Id = 3;
UPDATE Users SET PhoneNumber = '+51987654324' WHERE Id = 4;
UPDATE Users SET PhoneNumber = '+51987654325' WHERE Id = 5;

-- Actualizar estacionamientos existentes a ubicaciones en Lima
UPDATE Parkings SET 
    Name = 'Estacionamiento Plaza San Martín',
    Address = 'Plaza San Martín, Centro Histórico',
    City = 'Lima',
    PostalCode = '15001',
    Latitude = -12.0464,
    Longitude = -77.0428,
    HourlyRate = 5.00,
    DailyRate = 50.00,
    Description = 'Estacionamiento en el corazón del centro histórico de Lima'
WHERE Id = 1;

UPDATE Parkings SET 
    Name = 'Parqueadero Universidad San Marcos',
    Address = 'Av. Universitaria 1501, San Miguel',
    City = 'Lima',
    PostalCode = '15088',
    Latitude = -12.0169,
    Longitude = -77.0428,
    HourlyRate = 3.00,
    DailyRate = 30.00,
    Description = 'Estacionamiento de la Universidad Nacional Mayor de San Marcos'
WHERE Id = 2;

UPDATE Parkings SET 
    Name = 'Estacionamiento Hospital Edgardo Rebagliati',
    Address = 'Av. Rebagliati 490, Jesús María',
    City = 'Lima',
    PostalCode = '15072',
    Latitude = -12.0764,
    Longitude = -77.0428,
    HourlyRate = 4.00,
    DailyRate = 40.00,
    Description = 'Estacionamiento del hospital nacional'
WHERE Id = 3;

UPDATE Parkings SET 
    Name = 'Parqueadero Aeropuerto Jorge Chávez',
    Address = 'Av. Elmer Faucett s/n, Callao',
    City = 'Lima',
    PostalCode = '07031',
    Latitude = -12.0219,
    Longitude = -77.1143,
    HourlyRate = 8.00,
    DailyRate = 80.00,
    Description = 'Estacionamiento del Aeropuerto Internacional Jorge Chávez'
WHERE Id = 4;

UPDATE Parkings SET 
    Name = 'Estacionamiento Mall Plaza San Miguel',
    Address = 'Av. La Marina 2350, San Miguel',
    City = 'Lima',
    PostalCode = '15088',
    Latitude = -12.0764,
    Longitude = -77.0928,
    HourlyRate = 6.00,
    DailyRate = 60.00,
    Description = 'Estacionamiento del centro comercial Plaza San Miguel'
WHERE Id = 5;

-- Insertar nuevos estacionamientos en Lima
INSERT INTO Parkings (Name, Address, City, PostalCode, Latitude, Longitude, TotalSpaces, AvailableSpaces, HourlyRate, DailyRate, Status, Description, ImageUrl, CreatedAt, IsActive)
VALUES 
    ('Parqueadero Larcomar', 'Malecón de la Reserva 610, Miraflores', 'Lima', '15074', -12.1464, -77.0228, 80, 65, 7.00, 70.00, 'Active', 'Estacionamiento del centro comercial Larcomar', 'https://example.com/parking6.jpg', GETUTCDATE(), 1),
    ('Estacionamiento Jockey Plaza', 'Av. Javier Prado Este 4200, Surco', 'Lima', '15023', -12.1964, -76.9828, 120, 95, 6.50, 65.00, 'Active', 'Estacionamiento del centro comercial Jockey Plaza', 'https://example.com/parking7.jpg', GETUTCDATE(), 1),
    ('Parqueadero Plaza Mayor', 'Plaza Mayor de Lima, Centro Histórico', 'Lima', '15001', -12.0464, -77.0328, 40, 30, 4.50, 45.00, 'Active', 'Estacionamiento cerca de la Plaza Mayor de Lima', 'https://example.com/parking8.jpg', GETUTCDATE(), 1);

-- Insertar espacios para los nuevos estacionamientos
INSERT INTO ParkingSpaces (SpaceNumber, ParkingId, Type, Status, SpecialRate, Notes, CreatedAt, IsActive)
VALUES 
    -- Estacionamiento 6 (Larcomar)
    ('L1', 6, 'Standard', 'Available', NULL, NULL, GETUTCDATE(), 1),
    ('L2', 6, 'Standard', 'Available', NULL, NULL, GETUTCDATE(), 1),
    ('L3', 6, 'Electric', 'Available', 8.00, 'Cargador eléctrico disponible', GETUTCDATE(), 1),
    ('L4', 6, 'Large', 'Available', 8.50, 'Espacio para vehículos grandes', GETUTCDATE(), 1),
    
    -- Estacionamiento 7 (Jockey Plaza)
    ('J1', 7, 'Standard', 'Available', NULL, NULL, GETUTCDATE(), 1),
    ('J2', 7, 'Standard', 'Available', NULL, NULL, GETUTCDATE(), 1),
    ('J3', 7, 'Disabled', 'Available', NULL, 'Espacio para discapacitados', GETUTCDATE(), 1),
    ('J4', 7, 'Electric', 'Available', 7.50, 'Cargador eléctrico disponible', GETUTCDATE(), 1),
    
    -- Estacionamiento 8 (Plaza Mayor)
    ('PM1', 8, 'Standard', 'Available', NULL, NULL, GETUTCDATE(), 1),
    ('PM2', 8, 'Standard', 'Available', NULL, NULL, GETUTCDATE(), 1),
    ('PM3', 8, 'Disabled', 'Available', NULL, 'Espacio para discapacitados', GETUTCDATE(), 1);

-- Actualizar tarifas especiales en espacios existentes
UPDATE ParkingSpaces SET SpecialRate = 6.00 WHERE Id = 5; -- Electric en Plaza San Martín
UPDATE ParkingSpaces SET SpecialRate = 7.00 WHERE Id = 6; -- Large en Plaza San Martín
UPDATE ParkingSpaces SET SpecialRate = 1.50 WHERE Id = 9; -- Motorcycle en Universidad
UPDATE ParkingSpaces SET SpecialRate = 5.00 WHERE Id = 13; -- Electric en Hospital
UPDATE ParkingSpaces SET SpecialRate = 10.00 WHERE Id = 17; -- Large en Aeropuerto
UPDATE ParkingSpaces SET SpecialRate = 10.00 WHERE Id = 18; -- Electric en Aeropuerto

-- Actualizar reservas con nuevos montos
UPDATE Reservations SET 
    TotalAmount = 10.00,
    Notes = 'Reserva para visitar el centro histórico'
WHERE Id = 1;

UPDATE Reservations SET 
    TotalAmount = 6.00,
    Notes = 'Clase en la universidad'
WHERE Id = 2;

UPDATE Reservations SET 
    TotalAmount = 8.00,
    Notes = 'Visita al hospital'
WHERE Id = 3;

UPDATE Reservations SET 
    TotalAmount = 160.00,
    Notes = 'Viaje de negocios'
WHERE Id = 4;

UPDATE Reservations SET 
    TotalAmount = 12.00,
    Notes = 'Compras en el mall'
WHERE Id = 5; 