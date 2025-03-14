create database proyectoDBS
go 

use proyectoDBS
go

CREATE TABLE Roles (
    RolID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) UNIQUE NOT NULL
);

CREATE TABLE Usuarios (
    UsuarioID INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario NVARCHAR(50) UNIQUE NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    ContrasenaHash NVARCHAR(255) NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    RolID INT NOT NULL,
    FOREIGN KEY (RolID) REFERENCES Roles(RolID) ON DELETE CASCADE
);

-- Tabla de Categorías (Para clasificar los contenidos en Manga o Anime)
CREATE TABLE Categorias (
    CategoriaID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Contenidos (
    ContenidoID INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(255) NOT NULL,
    Descripcion TEXT,
    CategoriaID INT NOT NULL,
    FechaPublicacion DATE,
    FOREIGN KEY (CategoriaID) REFERENCES Categorias(CategoriaID) ON DELETE CASCADE
);

CREATE TABLE Episodios (
    EpisodioID INT IDENTITY(1,1) PRIMARY KEY,
    ContenidoID INT NOT NULL,
    NumeroEpisodio INT NOT NULL,
    Titulo NVARCHAR(255),
    RutaArchivo NVARCHAR(500) NOT NULL, -- Ruta del archivo del episodio
    FechaSubida DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ContenidoID) REFERENCES Contenidos(ContenidoID) ON DELETE CASCADE
);

CREATE TABLE Comentarios (
    ComentarioID INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioID INT NOT NULL,
    ContenidoID INT NOT NULL,
    Texto NVARCHAR(1000) NOT NULL,
    FechaComentario DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID) ON DELETE CASCADE,
    FOREIGN KEY (ContenidoID) REFERENCES Contenidos(ContenidoID) ON DELETE CASCADE
);

-- Índices para optimizar búsquedas
CREATE INDEX idx_Usuarios_Email ON Usuarios(Email);
CREATE INDEX idx_Contenidos_Titulo ON Contenidos(Titulo);
CREATE INDEX idx_Episodios_ContenidoID ON Episodios(ContenidoID);
CREATE INDEX idx_Comentarios_ContenidoID ON Comentarios(ContenidoID);

INSERT INTO Roles (Nombre) VALUES ('Usuario'), ('Empleado'), ('Administrador');
INSERT INTO Categorias (Nombre) VALUES ('Manga'), ('Anime');

SELECT * FROM Usuarios