CREATE DATABASE proyectoDBS
GO 

USE proyectoDBS
GO

-- Tabla de Roles
CREATE TABLE Roles (
    RolID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) UNIQUE NOT NULL
);

-- Tabla de Usuarios
CREATE TABLE Usuarios (
    UsuarioID INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario NVARCHAR(50) UNIQUE NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    ContrasenaHash NVARCHAR(255) NOT NULL,
	EsPremium BIT DEFAULT 0,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    RolID INT NOT NULL,
    FOREIGN KEY (RolID) REFERENCES Roles(RolID)
);

-- Tabla de Géneros
CREATE TABLE Generos (
    GeneroID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) UNIQUE NOT NULL
);

-- Tabla de Mangas
CREATE TABLE Mangas (
    MangaID INT PRIMARY KEY IDENTITY(1,1),
    Titulo NVARCHAR(100) NOT NULL,
    Autor NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(500),
    URLPortada NVARCHAR(255) NOT NULL,
    Estado NVARCHAR(20) DEFAULT 'En publicación',
    FechaPublicacion DATE,
    CarpetaDriveID NVARCHAR(100) NOT NULL 
);

-- Tabla de relación Manga-Género
CREATE TABLE MangaGeneros (
    MangaID INT NOT NULL,
    GeneroID INT NOT NULL,
    PRIMARY KEY (MangaID, GeneroID),
    FOREIGN KEY (MangaID) REFERENCES Mangas(MangaID) ON DELETE CASCADE,
    FOREIGN KEY (GeneroID) REFERENCES Generos(GeneroID) ON DELETE CASCADE
);

-- Tabla de Volúmenes
CREATE TABLE Volumenes (
    VolumenID INT PRIMARY KEY IDENTITY(1,1),
    MangaID INT NOT NULL,
    Numero INT NOT NULL,
    Titulo NVARCHAR(100),
    CantidadPaginas INT DEFAULT 0,
    CarpetaDriveID NVARCHAR(100) NOT NULL,
    FechaPublicacion DATE,
    FOREIGN KEY (MangaID) REFERENCES Mangas(MangaID) ON DELETE CASCADE
);

-- Tabla de Favoritos
CREATE TABLE Favoritos (
    FavoritoID INT PRIMARY KEY IDENTITY(1,1),
    UsuarioID INT NOT NULL,
    MangaID INT NOT NULL,
    FechaAgregado DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID) ON DELETE CASCADE,
    FOREIGN KEY (MangaID) REFERENCES Mangas(MangaID) ON DELETE CASCADE,
    UNIQUE (UsuarioID, MangaID)
);

INSERT INTO Roles (Nombre) VALUES ('Admin'), ('Usuario');

INSERT INTO Generos (Nombre) VALUES 
('Acción'), ('Aventura'), ('Comedia'), ('Drama'),
('Fantasía'), ('Horror'), ('Misterio'), ('Romance'),
('Ciencia Ficción'), ('Seinen'), ('Shonen');

INSERT INTO Usuarios (NombreUsuario, Email, ContrasenaHash, RolID) VALUES 
('admin', 'admin@mangadb.com', '$2a$10$xJwL5v5Jz7U6QbZ5X2n4Xe', 1),
('usuario1', 'usuario1@mangadb.com', '$2a$10$xJwL5v5Jz7U6QbZ5X2n4Xe', 2);

INSERT INTO Mangas (Titulo, Autor, Descripcion, URLPortada, Estado, FechaPublicacion, CarpetaDriveID) VALUES 
('One Piece', 'Eiichiro Oda', 'Aventuras piratas de Monkey D. Luffy', 'https://drive.google.com/1aB2cD3eF4g', 'En publicación', '1997-07-22', '5hJ6kL7mN8o'),
('Berserk', 'Kentaro Miura', 'La oscura historia de Guts', 'https://drive.google.com/9pQ0rS1tU2v', 'Pausado', '1989-08-25', '3wX4yZ5aB6c');

INSERT INTO MangaGeneros (MangaID, GeneroID) VALUES 
(1, 1), (1, 2), (1, 5), 
(2, 1), (2, 5), (2, 10); 

INSERT INTO Volumenes (MangaID, Numero, Titulo, CantidadPaginas, CarpetaDriveID, FechaPublicacion) VALUES 
(1, 1, 'Romance Dawn', 200, '7dE8fG9hI0j', '1997-12-24'),
(1, 2, 'Buggy el Payaso', 192, '1kL2mN3oP4q', '1998-04-03'),
(2, 1, 'La Espada Negra', 240, '5rS6tU7vW8x', '1990-11-22');

INSERT INTO Favoritos (UsuarioID, MangaID) VALUES 
(2, 1),
(2, 2);

SELECT * FROM Roles;
SELECT * FROM Usuarios;
SELECT * FROM Generos;
SELECT * FROM Mangas;
SELECT * FROM MangaGeneros;
SELECT * FROM Volumenes;
SELECT * FROM Favoritos;

