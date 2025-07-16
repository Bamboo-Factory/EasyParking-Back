-- Script de datos de ejemplo para EasyParking
-- Ejecutar después de aplicar las migraciones

-- Insertar usuarios de ejemplo
INSERT INTO Users (FirstName, LastName, Email, PhoneNumber, LicensePlate, PasswordHash, Role, CreatedAt, IsActive)
VALUES 
    ('Juan', 'Pérez', 'juan.perez@email.com', '+51987654321', 'ABC123', 'hashed_password_1', 'Customer', GETUTCDATE(), 1),
    ('María', 'García', 'maria.garcia@email.com', '+51987654322', 'XYZ789', 'hashed_password_2', 'Customer', GETUTCDATE(), 1),
    ('Carlos', 'López', 'carlos.lopez@email.com', '+51987654323', 'DEF456', 'hashed_password_3', 'Admin', GETUTCDATE(), 1),
    ('Ana', 'Rodríguez', 'ana.rodriguez@email.com', '+51987654324', 'GHI789', 'hashed_password_4', 'ParkingOwner', GETUTCDATE(), 1),
    ('Luis', 'Martínez', 'luis.martinez@email.com', '+51987654325', 'JKL012', 'hashed_password_5', 'Customer', GETUTCDATE(), 1);

-- Insertar estacionamientos de ejemplo en Lima, Perú
INSERT INTO Parkings (Name, Address, City, PostalCode, Latitude, Longitude, TotalSpaces, AvailableSpaces, HourlyRate, DailyRate, Status, Description, ImageUrl, CreatedAt, IsActive)
VALUES 
    ('Estacionamiento Plaza San Martín', 'Plaza San Martín, Centro Histórico', 'Lima', '15001', -12.0464, -77.0428, 50, 45, 5.00, 50.00, 'Active', 'Estacionamiento en el corazón del centro histórico de Lima', 'https://example.com/parking1.jpg', GETUTCDATE(), 1),
    ('Parqueadero Universidad San Marcos', 'Av. Universitaria 1501, San Miguel', 'Lima', '15088', -12.0169, -77.0428, 30, 25, 3.00, 30.00, 'Active', 'Estacionamiento de la Universidad Nacional Mayor de San Marcos', 'https://example.com/parking2.jpg', GETUTCDATE(), 1),
    ('Estacionamiento Hospital Edgardo Rebagliati', 'Av. Rebagliati 490, Jesús María', 'Lima', '15072', -12.0764, -77.0428, 40, 35, 4.00, 40.00, 'Active', 'Estacionamiento del hospital nacional', 'https://example.com/parking3.jpg', GETUTCDATE(), 1),
    ('Parqueadero Aeropuerto Jorge Chávez', 'Av. Elmer Faucett s/n, Callao', 'Lima', '07031', -12.0219, -77.1143, 100, 80, 8.00, 80.00, 'Active', 'Estacionamiento del Aeropuerto Internacional Jorge Chávez', 'https://example.com/parking4.jpg', GETUTCDATE(), 1),
    ('Estacionamiento Mall Plaza San Miguel', 'Av. La Marina 2350, San Miguel', 'Lima', '15088', -12.0764, -77.0928, 60, 50, 6.00, 60.00, 'Active', 'Estacionamiento del centro comercial Plaza San Miguel', 'https://example.com/parking5.jpg', GETUTCDATE(), 1),
    ('Parqueadero Larcomar', 'Malecón de la Reserva 610, Miraflores', 'Lima', '15074', -12.1464, -77.0228, 80, 65, 7.00, 70.00, 'Active', 'Estacionamiento del centro comercial Larcomar', 'https://example.com/parking6.jpg', GETUTCDATE(), 1),
    ('Estacionamiento Jockey Plaza', 'Av. Javier Prado Este 4200, Surco', 'Lima', '15023', -12.1964, -76.9828, 120, 95, 6.50, 65.00, 'Active', 'Estacionamiento del centro comercial Jockey Plaza', 'https://example.com/parking7.jpg', GETUTCDATE(), 1),
    ('Parqueadero Plaza Mayor', 'Plaza Mayor de Lima, Centro Histórico', 'Lima', '15001', -12.0464, -77.0328, 40, 30, 4.50, 45.00, 'Active', 'Estacionamiento cerca de la Plaza Mayor de Lima', 'https://example.com/parking8.jpg', GETUTCDATE(), 1);

-- Insertar espacios de estacionamiento de ejemplo
INSERT INTO ParkingSpaces (SpaceNumber, ParkingId, Type, Status, SpecialRate, Notes, CreatedAt, IsActive)
VALUES 
    -- Estacionamiento 1 (Plaza San Martín)
    ('A1', 1, 'Standard', 'Available', NULL, NULL, GETUTCDATE(), 1),
    ('A2', 1, 'Standard', 'Available', NULL, NULL, GETUTCDATE(), 1),
    ('A3', 1, 'Standard', 'Available', NULL, NULL, GETUTCDATE(), 1),
    ('B1', 1, 'Disabled', 'Available', NULL, 'Espacio para discapacitados', GETUTCDATE(), 1),
    ('B2', 1, 'Electric', 'Available', 6.00, 'Cargador eléctrico disponible', GETUTCDATE(), 1),
    ('C1', 1, 'Large', 'Available', 7.00, 'Espacio para vehículos grandes', GETUTCDATE(), 1),
    
    -- Estacionamiento 2 (Universidad San Marcos)
    ('U1', 2, 'Standard', 'Available', NULL, NULL, GETUTCDATE(), 1),
    ('U2', 2, 'Standard', 'Available', NULL, NULL, GETUTCDATE(), 1),
    ('U3', 2, 'Motorcycle', 'Available', 1.50, 'Espacio para motocicletas', GETUTCDATE(), 1),
    ('U4', 2, 'Disabled', 'Available', NULL, 'Espacio para discapacitados', GETUTCDATE(), 1),
    
    -- Estacionamiento 3 (Hospital Rebagliati)
    ('H1', 3, 'Standard', 'Available', NULL, NULL, GETUTCDATE(), 1),
    ('H2', 3, 'Standard', 'Available', NULL, NULL, GETUTCDATE(), 1),
    ('H3', 3, 'Disabled', 'Available', NULL, 'Espacio para discapacitados', GETUTCDATE(), 1),
    ('H4', 3, 'Electric', 'Available', 5.00, 'Cargador eléctrico disponible', GETUTCDATE(), 1),
    
    -- Estacionamiento 4 (Aeropuerto Jorge Chávez)
    ('AP1', 4, 'Standard', 'Available', NULL, NULL, GETUTCDATE(), 1),
    ('AP2', 4, 'Standard', 'Available', NULL, NULL, GETUTCDATE(), 1),
    ('AP3', 4, 'Large', 'Available', 10.00, 'Espacio para vehículos grandes', GETUTCDATE(), 1),
    ('AP4', 4, 'Electric', 'Available', 10.00, 'Cargador eléctrico disponible', GETUTCDATE(), 1),
    
    -- Estacionamiento 5 (Plaza San Miguel)
    ('M1', 5, 'Standard', 'Available', NULL, NULL, GETUTCDATE(), 1),
    ('M2', 5, 'Standard', 'Available', NULL, NULL, GETUTCDATE(), 1),
    ('M3', 5, 'Disabled', 'Available', NULL, 'Espacio para discapacitados', GETUTCDATE(), 1),
    
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

-- Insertar reservas de ejemplo
INSERT INTO Reservations (UserId, ParkingId, ParkingSpaceId, StartTime, EndTime, TotalAmount, Status, PaymentStatus, Notes, CreatedAt, IsActive)
VALUES 
    (1, 1, 1, DATEADD(HOUR, 1, GETUTCDATE()), DATEADD(HOUR, 3, GETUTCDATE()), 10.00, 'Confirmed', 'Paid', 'Reserva para visitar el centro histórico', GETUTCDATE(), 1),
    (2, 2, 7, DATEADD(HOUR, 2, GETUTCDATE()), DATEADD(HOUR, 4, GETUTCDATE()), 6.00, 'Active', 'Paid', 'Clase en la universidad', GETUTCDATE(), 1),
    (3, 3, 11, DATEADD(HOUR, -1, GETUTCDATE()), DATEADD(HOUR, 1, GETUTCDATE()), 8.00, 'Completed', 'Paid', 'Visita al hospital', GETUTCDATE(), 1),
    (4, 4, 15, DATEADD(DAY, 1, GETUTCDATE()), DATEADD(DAY, 3, GETUTCDATE()), 160.00, 'Pending', 'Pending', 'Viaje de negocios', GETUTCDATE(), 1),
    (5, 5, 19, DATEADD(HOUR, 3, GETUTCDATE()), DATEADD(HOUR, 5, GETUTCDATE()), 12.00, 'Confirmed', 'Paid', 'Compras en el mall', GETUTCDATE(), 1);

-- Actualizar espacios disponibles en los estacionamientos
UPDATE Parkings SET AvailableSpaces = 44 WHERE Id = 1; -- 50 total - 6 ocupados
UPDATE Parkings SET AvailableSpaces = 26 WHERE Id = 2; -- 30 total - 4 ocupados
UPDATE Parkings SET AvailableSpaces = 36 WHERE Id = 3; -- 40 total - 4 ocupados
UPDATE Parkings SET AvailableSpaces = 79 WHERE Id = 4; -- 100 total - 21 ocupados
UPDATE Parkings SET AvailableSpaces = 49 WHERE Id = 5; -- 60 total - 11 ocupados
UPDATE Parkings SET AvailableSpaces = 64 WHERE Id = 6; -- 80 total - 16 ocupados
UPDATE Parkings SET AvailableSpaces = 94 WHERE Id = 7; -- 120 total - 26 ocupados
UPDATE Parkings SET AvailableSpaces = 29 WHERE Id = 8; -- 40 total - 11 ocupados 