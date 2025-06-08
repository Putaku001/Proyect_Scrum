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
    }
    .manga-card img {
      width: 100%;
      height: 300px;
      object-fit: cover;
    }
    .manga-card .info {
      padding: 15px;
    }
    .manga-card h3 {
      color: #00d4ff;
    }
    .manga-card .admin-buttons {
      margin-top: 10px;
      display: flex;
      justify-content: space-between;
      flex-wrap: wrap;
    }
    .admin-buttons a {
      background: #ff8c00;
      color: #fff;
      padding: 6px 12px;
      text-decoration: none;
      border-radius: 5px;
      font-size: 0.85rem;
    }
    .admin-buttons a.eliminar {
      background: red;
    }
    footer {
      margin-top: 60px;
      text-align: center;
      color: #aaa;
    }
  </style>
</head>
<body>

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
        <img src="<?= htmlspecialchars($manga['PortadaElegida']) ?>" alt="Portada de <?= htmlspecialchars($manga['Titulo']) ?>">
        <div class="info">
          <h3><?= $manga['Titulo'] ?></h3>
          <p><strong>Autor:</strong> <?= $manga['Autor'] ?></p>
          <p><strong>Género:</strong> <?= $manga['Genero'] ?></p>
          <p><strong>Estado:</strong> <?= $manga['Estado'] ?></p>
          <div class="admin-buttons">
            <a href="detalle_manga.php?id=<?= $manga['MangaID'] ?>">📖 Detalles</a>
            <a href="editar_manga.php?id=<?= $manga['MangaID'] ?>">✏️ Editar</a>
            <a href="eliminar_manga.php?id=<?= $manga['MangaID'] ?>" class="eliminar" onclick="return confirm('¿Eliminar este manga?')">🗑️ Eliminar</a>
          </div>
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
