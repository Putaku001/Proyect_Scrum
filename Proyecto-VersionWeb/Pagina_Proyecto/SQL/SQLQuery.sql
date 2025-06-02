create database proyectoDBS3
go

use proyectoDBS3
go


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
    Estado NVARCHAR(20) DEFAULT 'En publicacion',
    FechaPublicacion DATE,
    URLMangaDrive NVARCHAR(100) NOT NULL, --carpeta de volumes de manga en pdf ubicado en drive
	URLPortada NVARCHAR(255) NOT NULL -- portadas de los mangas jpg ubicados en drive
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

ALTER TABLE Mangas ADD GeneroID INT;

ALTER TABLE Mangas ADD CONSTRAINT FK_Mangas_Generos FOREIGN KEY (GeneroID) REFERENCES Generos(GeneroID);

Select * from Mangas

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


CREATE TABLE TitulosAlternativos (
    ID INT PRIMARY KEY IDENTITY,
    MangaID INT FOREIGN KEY REFERENCES Mangas(MangaID),
    TituloAlternativo NVARCHAR(255)
);

-- Koe no Katachi → Silent Voice
INSERT INTO TitulosAlternativos (MangaID, TituloAlternativo)
VALUES (1, 'a Silent Voice');

-- Kimi no Na wa → Your Name
INSERT INTO TitulosAlternativos (MangaID, TituloAlternativo)
VALUES (5, 'Your Name');

CREATE TABLE ProgresoLectura (
    ProgresoID INT PRIMARY KEY IDENTITY(1,1),
    UsuarioID INT NOT NULL,
    MangaID INT NOT NULL,
    PaginaActual INT NOT NULL DEFAULT 0,
    Status NVARCHAR(20) DEFAULT 'Leyendo', -- Ej: Leyendo, Pausado, Finalizado
    FechaUltimaLectura DATETIME DEFAULT GETDATE(),
    TiempoLecturaTotal INT DEFAULT 0, -- en minutos
    VecesLeido INT DEFAULT 1, -- cuántas veces ha iniciado desde la página 0
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID) ON DELETE CASCADE,
    FOREIGN KEY (MangaID) REFERENCES Mangas(MangaID) ON DELETE CASCADE,
    UNIQUE (UsuarioID, MangaID)
);


INSERT INTO Roles (Nombre) VALUES ('Usuario');

ALTER TABLE Mangas ADD URLPortadaWeb NVARCHAR(255);

UPDATE Mangas SET URLPortadaWeb = 'imgs/covers/koe no katachi.jpg' WHERE Titulo = 'Koe no Katachi';
UPDATE Mangas SET URLPortadaWeb = 'imgs/covers/akunohana.jpg' WHERE Titulo = 'Aku no Hana';
UPDATE Mangas SET URLPortadaWeb = 'imgs/covers/Kurosawa.jpg' WHERE Titulo = 'Onanie Master Kurosawa';
UPDATE Mangas SET URLPortadaWeb = 'imgs/covers/ruri-dragon-vol1.jpg' WHERE Titulo = 'Ruri Dragon';
UPDATE Mangas SET URLPortadaWeb = 'imgs/covers/yourname.jpg' WHERE Titulo = 'Kimi no Na wa';
UPDATE Mangas SET URLPortadaWeb = 'imgs/covers/KS.jpg' WHERE Titulo = 'Kaichou-kun no Shimobe';



select * from TitulosAlternativos
select * from Generos
select * from Mangas
SELECT * FROM Roles;
SELECT * FROM Usuarios;
SELECT * FROM Favoritos;
select * from ProgresoLectura