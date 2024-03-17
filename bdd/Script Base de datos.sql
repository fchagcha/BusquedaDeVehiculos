CREATE TABLE OutboxMessage
(
    Id CHAR(36) PRIMARY KEY,
    IntegrationEventType TEXT,
    TypeRequest TEXT,
    JsonRequest TEXT,
    Status INT,
    CreatedAt DATETIME,
    ProcessedAt DATETIME
);

CREATE TABLE Localidades (
    Id CHAR(36) PRIMARY KEY,
    Nombre VARCHAR(255) NOT NULL,
    Mercado VARCHAR(255) NOT NULL,
    EstaBorrado BOOLEAN NOT NULL,
    CreadoPor VARCHAR(255) NOT NULL,
    FechaCreacion DATETIME NOT NULL,
    ModificadoPor VARCHAR(255),
    FechaUltimaModificacion DATETIME,
    Version BLOB
);

CREATE TABLE Vehiculos (
    Id CHAR(36) PRIMARY KEY,
    IdLocalidadActual CHAR(36) NOT NULL,
    Marca VARCHAR(255) NOT NULL,
    Modelo VARCHAR(255) NOT NULL,
    Placa VARCHAR(255) NOT NULL,
    TarifaDiaria DECIMAL(10, 2) NOT NULL,
    Tipo INT NOT NULL,
    Estado INT NOT NULL,
    EstaBorrado BOOLEAN NOT NULL,
    CreadoPor VARCHAR(255) NOT NULL,
    FechaCreacion DATETIME NOT NULL,
    ModificadoPor VARCHAR(255),
    FechaUltimaModificacion DATETIME,
    Version BLOB,
    FOREIGN KEY (IdLocalidadActual) REFERENCES Localidades(Id)
);

CREATE TABLE Reservas (
    Id CHAR(36) PRIMARY KEY,
    IdVehiculo CHAR(36) NOT NULL,
    IdLocalidadRecogida CHAR(36) NOT NULL,
    IdLocalidadDevolucion CHAR(36) NOT NULL,
    NombreConductor VARCHAR(100) NOT NULL,
    CorreoElectronicoConductor VARCHAR(50) NOT NULL,
    FechaRecogida DATETIME NOT NULL,
    FechaDevolucion DATETIME NOT NULL,
    TarifaTotal DECIMAL(10, 2) NOT NULL,
    Estado INT NOT NULL,
    EstaBorrado BOOLEAN NOT NULL,
    CreadoPor VARCHAR(255) NOT NULL,
    FechaCreacion DATETIME NOT NULL,
    ModificadoPor VARCHAR(255),
    FechaUltimaModificacion DATETIME,
    Version BLOB,
    FOREIGN KEY (IdVehiculo) REFERENCES Vehiculos(Id),
    FOREIGN KEY (IdLocalidadRecogida) REFERENCES Localidades(Id),
    FOREIGN KEY (IdLocalidadDevolucion) REFERENCES Localidades(Id)
);

INSERT INTO Localidades (Id, Nombre, Mercado, EstaBorrado, CreadoPor, FechaCreacion)
VALUES 
('3D8BFE57-6EDB-46A3-A3C4-9FBC9D6479A1', 'Localidad 1', 'Mercado 1', false, 'Admin', NOW()),
('A1D330AD-7F2F-4BBA-A61C-9826143A9A50', 'Localidad 2', 'Mercado 2', false, 'Admin', NOW()),
('B2D330AD-7F2F-4BBA-A61C-9826143A9A51', 'Localidad 3', 'Mercado 3', false, 'Admin', NOW()),
('C3D330AD-7F2F-4BBA-A61C-9826143A9A52', 'Localidad 4', 'Mercado 4', false, 'Admin', NOW()),
('D4D330AD-7F2F-4BBA-A61C-9826143A9A53', 'Localidad 5', 'Mercado 5', false, 'Admin', NOW());

INSERT INTO Vehiculos (Id, IdLocalidadActual, Marca, Modelo, Placa, TarifaDiaria, Tipo, Estado, EstaBorrado, CreadoPor, FechaCreacion)
VALUES 
('D4B477F0-6453-4237-84C2-061AC52ABD86', '3D8BFE57-6EDB-46A3-A3C4-9FBC9D6479A1', 'Marca 1', 'Modelo 1', 'Placa 1', 100.00, 1, 1, false, 'Admin', NOW()),
('D55DAE28-5C29-467D-BC3E-994B5EB1961C', 'A1D330AD-7F2F-4BBA-A61C-9826143A9A50', 'Marca 2', 'Modelo 2', 'Placa 2', 200.00, 2, 1, false, 'Admin', NOW()),
('7BC795FB-CE8D-4A96-A2F5-C6DFBA0376C3', 'B2D330AD-7F2F-4BBA-A61C-9826143A9A51', 'Marca 3', 'Modelo 3', 'Placa 3', 300.00, 3, 1, false, 'Admin', NOW()),
('90D2E6C1-A16B-478C-9161-C840371DDD35', 'C3D330AD-7F2F-4BBA-A61C-9826143A9A52', 'Marca 4', 'Modelo 4', 'Placa 4', 400.00, 4, 1, false, 'Admin', NOW()),
('47900BED-FC14-4F53-B7F2-F073FD8B3AEE', 'D4D330AD-7F2F-4BBA-A61C-9826143A9A53', 'Marca 5', 'Modelo 5', 'Placa 5', 500.00, 5, 1, false, 'Admin', NOW());

INSERT INTO Reservas (Id, IdVehiculo, IdLocalidadRecogida, IdLocalidadDevolucion, NombreConductor, CorreoElectronicoConductor, FechaRecogida, FechaDevolucion, TarifaTotal, Estado, EstaBorrado, CreadoPor, FechaCreacion)
VALUES 
('J0D330AD-7F2F-4BBA-A61C-9826143A9A59', 'D4B477F0-6453-4237-84C2-061AC52ABD86', '3D8BFE57-6EDB-46A3-A3C4-9FBC9D6479A1', 'A1D330AD-7F2F-4BBA-A61C-9826143A9A50', 'Conductor 1', 'conductor1@example.com', NOW(), DATE_ADD(NOW(), INTERVAL 7 DAY),700.00,1,false, 'Admin', NOW());
