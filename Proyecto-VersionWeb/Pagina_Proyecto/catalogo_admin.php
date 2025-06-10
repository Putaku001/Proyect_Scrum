<?php
session_start();
require_once 'db.php';

if (!isset($_SESSION['usuario_id']) || $_SESSION['rol'] != 2) {
    header("Location: login.html");
    exit();
}

$generoSeleccionado = $_GET['genero'] ?? '';
$busqueda = $_GET['buscar'] ?? '';

// Obtener géneros
$generos = [];
$stmtGeneros = sqlsrv_query($conn, "SELECT GeneroID, Nombre FROM Generos");
while ($row = sqlsrv_fetch_array($stmtGeneros, SQLSRV_FETCH_ASSOC)) {
    $generos[] = $row;
}

// Obtener mangas
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
    $portadaWeb = $row['URLPortadaWeb'];
    $row['PortadaElegida'] = (!empty($portadaWeb) && file_exists($portadaWeb)) ? $portadaWeb : $row['URLPortada'];
    $mangas[] = $row;
}
?>
<!DOCTYPE html>
<html lang="es">
<head>
  <meta charset="UTF-8">
  <title>Catálogo Admin - Manga Verse</title>
  <link rel="stylesheet" href="./css/style.css">
  <style>
    body {
      background: #1e1e2f;
      color: #f0f0f0;
      font-family: 'Segoe UI', sans-serif;
    }
    .catalog-container {
      max-width: 1200px;
      margin: 100px auto 50px;
      padding: 0 20px;
    }
    .catalog-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      flex-wrap: wrap;
      margin-bottom: 30px;
    }
    .catalog-header form {
      display: flex;
      gap: 10px;
    }
    input, select, button {
      padding: 10px;
      border-radius: 6px;
      border: 1px solid #555;
      background: #2c2f4a;
      color: #f0f0f0;
    }
    .admin-actions {
      text-align: right;
    }
    .admin-actions a {
      padding: 10px 20px;
      background: #00d4ff;
      color: black;
      font-weight: bold;
      text-decoration: none;
      border-radius: 6px;
    }
    .manga-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
      gap: 20px;
    }
    .manga-card {
      background: #2c2f4a;
      border-radius: 10px;
      overflow: hidden;
      box-shadow: 0 4px 8px rgba(0,0,0,0.4);
      position: relative;
      transition: transform 0.3s ease;
    }
    .manga-card:hover {
      transform: scale(1.02);
    }
    .img-wrapper {
      position: relative;
      width: 100%;
      height: 300px;
      overflow: hidden;
    }
    .img-wrapper a {
      display: block;
      width: 100%;
      height: 100%;
    }
    .img-wrapper img {
      width: 100%;
      height: 100%;
      object-fit: cover;
      display: block;
    }
    .img-wrapper:hover .card-buttons-top {
      opacity: 1;
      pointer-events: all;
    }
    .card-buttons-top {
      position: absolute;
      top: 10px;
      right: 10px;
      display: flex;
      flex-direction: column;
      gap: 10px;
      opacity: 0;
      pointer-events: none;
      transition: opacity 0.3s ease;
      z-index: 2;
    }
    .card-buttons-top a {
      width: 38px;
      height: 38px;
      border-radius: 50%;
      background: rgba(0,0,0,0.7);
      color: white;
      display: flex;
      justify-content: center;
      align-items: center;
      text-decoration: none;
      font-size: 18px;
      border: 1px solid white;
    }
    .card-buttons-top a:hover {
      background: #00d4ff;
      color: black;
    }
    .card-buttons-top .eliminar:hover {
      background: red;
      color: white;
    }
    .manga-card .info {
      padding: 16px 15px;
      background-color: #2c2f4a;
      box-sizing: border-box;
    }
    .manga-card h3 {
      color: #00d4ff;
      font-size: 1.15rem;
      margin: 0;
    }
    footer {
      margin-top: 60px;
      text-align: center;
      color: #aaa;
    }
    .btn-volver {
  position: fixed;
  top: 32px;
  left: 32px;
  background: #232544;
  color: #00eaff;
  border-radius: 11px;
  padding: 11px 24px 11px 18px;
  font-weight: 700;
  font-size: 1.16em;
  text-decoration: none;
  box-shadow: 0 4px 22px #0005;
  border: 2px solid #00eaff22;
  transition: background .15s, color .15s, border .14s, box-shadow .12s, transform .11s;
  z-index: 10;
  display: inline-block;
}
.btn-volver:hover, .btn-volver:focus {
  background: #00eaff;
  color: #232544;
  border: 2.5px solid #00eaff;
  box-shadow: 0 8px 24px #00eaff66;
  outline: none;
  transform: scale(1.04);
}
@media (max-width:600px){
  .btn-volver{top:10px;left:8px;font-size:1em;padding:9px 13px;}
}

  </style>
</head>
<body>
<a href="admin_dashboard.php" class="btn-volver" title="Volver al dashboard">
  <span style="font-size:1.6em;vertical-align:-2px;">←</span> Volver
</a>

<div class="catalog-container">
  <div class="catalog-header">
    <form method="GET" action="catalogo_admin.php">
      <input type="text" name="buscar" placeholder="Buscar manga..." value="<?php echo htmlspecialchars($busqueda); ?>">
      <select name="genero">
        <option value="">Todos los géneros</option>
        <?php foreach ($generos as $g): ?>
          <option value="<?= $g['GeneroID'] ?>" <?= $g['GeneroID'] == $generoSeleccionado ? 'selected' : '' ?>>
            <?= $g['Nombre'] ?>
          </option>
        <?php endforeach; ?>
      </select>
      <button type="submit">Filtrar</button>
    </form>

    <div class="admin-actions">
      <a href="subidaMangaForm.php">+ Subir nuevo manga</a>
    </div>
  </div>

  <div class="manga-grid">
    <?php foreach ($mangas as $manga): ?>
      <div class="manga-card">
        <div class="img-wrapper">
          <a href="detalle_manga_admin.php?id=<?= $manga['MangaID'] ?>">
            <img src="<?= htmlspecialchars($manga['PortadaElegida']) ?>" alt="Portada de <?= htmlspecialchars($manga['Titulo']) ?>">
          </a>
          <div class="card-buttons-top">
            <a href="editar_manga.php?id=<?= $manga['MangaID'] ?>" title="Editar">✏️</a>
            <a href="eliminar_manga.php?id=<?= $manga['MangaID'] ?>" class="eliminar" title="Eliminar"
               onclick="return confirm('¿Eliminar este manga?')">🗑️</a>
          </div>
        </div>
        <div class="info">
          <h3><?= $manga['Titulo'] ?></h3>
        </div>
      </div>
    <?php endforeach; ?>
  </div>
</div>

<footer>
  <p>&copy; 2025 Manga Verse — Panel Admin</p>
</footer>

</body>
</html>
