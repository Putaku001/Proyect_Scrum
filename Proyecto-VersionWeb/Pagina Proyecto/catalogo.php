<?php
session_start();
require_once 'db.php';

$generoSeleccionado = $_GET['genero'] ?? '';
$busqueda = $_GET['buscar'] ?? '';

// Obtener géneros
$generos = [];
$stmtGeneros = sqlsrv_query($conn, "SELECT GeneroID, Nombre FROM Generos");
while ($row = sqlsrv_fetch_array($stmtGeneros, SQLSRV_FETCH_ASSOC)) {
    $generos[] = $row;
}

// Obtener mangas con filtros, incluyendo URLPortadaWeb
$sql = "SELECT M.MangaID, M.Titulo, M.Autor, M.Descripcion, M.Estado, M.FechaPublicacion, 
               M.URLMangaDrive, M.URLPortada, M.URLPortadaWeb, G.Nombre AS Genero
        FROM Mangas M
        INNER JOIN Generos G ON M.GeneroID = G.GeneroID
        WHERE 1=1";
$params = [];

if ($generoSeleccionado != '') {
    $sql .= " AND M.GeneroID = ?";
    $params[] = $generoSeleccionado;
}
if ($busqueda != '') {
    $sql .= " AND M.Titulo LIKE ?";
    $params[] = '%' . $busqueda . '%';
}

$stmt = sqlsrv_query($conn, $sql, $params);
$mangas = [];
while ($row = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC)) {
    // Elegir portada: local si existe, si no la de Drive
    $portadaWeb = $row['URLPortadaWeb'];
    if (!empty($portadaWeb) && file_exists($portadaWeb)) {
        $row['PortadaElegida'] = $portadaWeb;
    } else {
        $row['PortadaElegida'] = $row['URLPortada'];
    }
    $mangas[] = $row;
}
?>


<!DOCTYPE html>
<html lang="es">
<head>
  <meta charset="UTF-8">
  <title>Catálogo - Manga Verse</title>
  <link rel="stylesheet" href="./css/style.css">
  <style>
    .catalog-container {
      max-width: 1200px;
      margin: 100px auto 50px;
      padding: 0 20px;
    }
    .catalog-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 30px;
      flex-wrap: wrap;
    }
    .catalog-header input, .catalog-header select, .catalog-header button {
      padding: 10px;
      border-radius: 8px;
      border: none;
      margin-right: 10px;
    }
    .manga-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(240px, 1fr));
      gap: 25px;
    }
    .manga-card {
      background: #2a2a2a;
      border-radius: 12px;
      overflow: hidden;
      box-shadow: 0 4px 8px rgba(0,0,0,0.4);
      transition: transform 0.3s;
    }
    .manga-card:hover {
      transform: scale(1.03);
    }
    .manga-card img {
      width: 100%;
      height: 320px;
      object-fit: cover;
    }
    .manga-card .info {
      padding: 15px;
    }
    .manga-card .info h3 {
      margin-bottom: 10px;
      color: #fff;
    }
    .manga-card .info p {
      color: #ccc;
      font-size: 0.9rem;
      margin-bottom: 10px;
    }
    .manga-card .info .leer-btn {
      display: inline-block;
      background: linear-gradient(135deg, #1e90ff, #8a2be2);
      color: #fff;
      padding: 10px 20px;
      border-radius: 30px;
      text-decoration: none;
      font-weight: bold;
    }
    footer {
      margin-top: 80px;
    }
  </style>
</head>
<body>

<header>
  <div class="header-container">
    <div class="logo">
      <img src="./imgs/Logito.png" alt="Logo Manga Verse" class="header-logo">
      <span>Manga Verse</span>
    </div>
    <nav>
      <ul>
        <li><a href="dashboard.php">Inicio</a></li>
        <li><a href="#">Mi Lista</a></li>
        <li><a href="catalogo.php">Catálogo</a></li>
      </ul>
    </nav>
    <div class="profile">
      <?php if (isset($_SESSION['nombre'])): ?>
        <span>Hola, <?php echo $_SESSION['nombre']; ?></span>
      <?php else: ?>
        <a href="login.html">Iniciar Sesión</a>
      <?php endif; ?>
    </div>
  </div>
</header>


<div class="catalog-container">
  <div class="catalog-header">
    <form method="GET" action="catalogo.php">
      <input type="text" name="buscar" placeholder="Buscar manga..." value="<?php echo htmlspecialchars($busqueda); ?>">
      <select name="genero">
        <option value="">Todos los géneros</option>
        <?php foreach ($generos as $g): ?>
          <option value="<?php echo $g['GeneroID']; ?>" <?php if ($g['GeneroID'] == $generoSeleccionado) echo 'selected'; ?>>
            <?php echo $g['Nombre']; ?>
          </option>
        <?php endforeach; ?>
      </select>
      <button type="submit">Filtrar</button>
    </form>
  </div>

  <div class="manga-grid">
    <?php foreach ($mangas as $manga): ?>
      <div class="manga-card">
        <a href="detalle_manga.php?id=<?php echo $manga['MangaID']; ?>">
        <img src="<?php echo htmlspecialchars($manga['PortadaElegida']); ?>" alt="Portada de <?php echo htmlspecialchars($manga['Titulo']); ?>">
        </a>
        <div class="info">
          <h3><?php echo $manga['Titulo']; ?></h3>
          <p><strong>Autor:</strong> <?php echo $manga['Autor']; ?></p>
          <p><strong>Género:</strong> <?php echo $manga['Genero']; ?></p>
          <p><strong>Estado:</strong> <?php echo $manga['Estado']; ?></p>
          <a href="detalle_manga.php?id=<?php echo $manga['MangaID']; ?>" class="leer-btn">📖 Ver Detalles</a>
        </div>
      </div>
    <?php endforeach; ?>
  </div>
</div>

<footer>
  <p>&copy; 2025 Manga Verse. Todos los derechos reservados.</p>
</footer>

</body>
</html> 