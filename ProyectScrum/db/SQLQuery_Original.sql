CREATE DATABASE proyectoDBSS
GO 

USE proyectoDBSS
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

-- Tabla de G�neros
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
    Estado NVARCHAR(20) DEFAULT 'En publicación',
    FechaPublicacion DATE,
    URLMangaDrive NVARCHAR(100) NOT NULL,
    URLPortada NVARCHAR(255) NOT NULL,
    GeneroID INT,
    FOREIGN KEY (GeneroID) REFERENCES Generos(GeneroID)
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
('Romance'),
('Drama'),
('Psicológico'),
('Sobrenatural'),
('Vida Escolar'),
('Fantástico');

DELETE FROM Mangas;
DBCC CHECKIDENT ('Mangas', RESEED, 0);

INSERT INTO Mangas (Titulo, Autor, Descripcion, Estado, FechaPublicacion, URLMangaDrive, URLPortada, GeneroID)
VALUES 
('Koe no Katachi', 'Yoshitoki Ōima',
    'Shōko Nishimiya, una estudiante de primaria sorda, empieza a sufrir bullying. Años después, Ishida busca redimirse.',
    'Finalizado', '2013-08-06',
    'https://drive.google.com/drive/folders/1izcBRPWyw9t5SfTm44F-PJZGe3qVLQJY?usp=sharing',
    'https://drive.google.com/uc?export=view&id=1_8egGWW_-pXajVA99w5fr7loFjeP3Nw8',
    2),

('Aku no Hana', 'Shūzō Oshimi',
    'Kasuga ama la literatura y roba el uniforme de la chica que le gusta, lo que desencadena una espiral psicológica.',
    'Finalizado', '2014-09-09',
    'https://drive.google.com/drive/folders/1CcjRJzp7IKHqKM0KJDts3cPWkRWpeZF2?usp=sharing',
    'https://drive.google.com/uc?export=view&id=1kklUx7glv8kKIitozA9Fs_fY0dOCHzDw',
    3),

('Onanie Master Kurosawa', 'Ise Katsura',
    'Un solitario adolescente enfrenta consecuencias inesperadas por sus actos secretos en la escuela.',
    'Finalizado', '2008-01-01',
    'https://drive.google.com/drive/folders/17IjQGamseRmbG0yXtSlGSVty8Iku2GKe?usp=sharing',
    'https://drive.google.com/uc?export=view&id=1Ju5-yJBRKZHHL6IUurs9pmLZJmgdyCsn',
    3),

('Ruri Dragon', 'Masaoki Shindo',
    'Ruri descubre que es mitad dragón y vive aventuras sobrenaturales en la escuela.',
    'En publicación', '2022-06-13',
    'https://drive.google.com/drive/folders/1GUkMoBVIAkcrEGcTFqhJWucdXgIp4cQZ?usp=sharing',
    'https://drive.google.com/uc?export=view&id=1ujfDFBovbRAMe9Pj5O8pbTIFGr_nxGRO',
    4),

('Kimi no Na wa', 'Makoto Shinkai',
    'Dos adolescentes intercambian cuerpos en sueños, buscando encontrarse y comprender su conexión.',
    'Finalizado', '2016-08-26',
    'https://drive.google.com/drive/folders/1cMWvfzju9I92nXn9K8ZE2f79xD8nrEF0?usp=sharing',
    'https://drive.google.com/uc?export=view&id=16YvC2eePRj1VwHwmr2xC87R9_2Nk-lXw',
    1),

('Kaichou-kun no Shimobe', 'Fujikawa Yura',
    'Una chica termina siendo asistente del presidente del consejo estudiantil, un chico con una actitud difícil.',
    'Finalizado', '2013-11-01',
    'https://drive.google.com/drive/folders/1grC9VA25fjf-xjzERZAn6J10sDnjx024?usp=sharing',
    'https://drive.google.com/uc?export=view&id=1TOsTCZOXUN08qAdAX0L_knTzqAGvC7u_',
    5);

	INSERT INTO Favoritos (UsuarioID, MangaID) VALUES 
	(2, 1),
	(2, 2);

SELECT * FROM Roles;
SELECT * FROM Usuarios;
SELECT * FROM Generos;
SELECT * FROM Mangas;
SELECT * FROM Favoritos;

