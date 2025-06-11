<?php
session_start();
require_once '../Config/db.php';

if (!isset($_SESSION['usuario_id']) || $_SESSION['rol'] != 2) {
    header("Location: ../Public/login.html");
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
  <link rel="stylesheet" href="../assets/css/style.css">
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
      flex-wrap: wrap;
      margin-bottom: 30px;
      gap: 20px;
    }

    .catalog-header form {
      display: flex;
      gap: 12px;
      flex-wrap: wrap;
      flex-grow: 1;
    }

    .catalog-header input,
    .catalog-header select {
      padding: 12px 15px;
      border-radius: 8px;
      border: 1px solid var(--input-border);
      background: var(--input-bg);
      color: var(--text-primary);
      font-size: 1rem;
      min-width: 200px;
      flex-grow: 1;
      transition: border-color 0.3s ease;
    }

    .catalog-header input:focus,
    .catalog-header select:focus {
      outline: none;
      border-color: var(--accent-color);
      box-shadow: 0 0 0 2px rgba(138, 43, 226, 0.2);
    }

    .catalog-header button {
      padding: 12px 24px;
      border-radius: 8px;
      border: none;
      background: var(--button-primary);
      color: white;
      font-weight: bold;
      cursor: pointer;
      transition: all 0.3s ease;
      flex-shrink: 0;
    }

    .catalog-header button:hover {
      transform: translateY(-2px);
      box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
    }

    .admin-actions a {
      padding: 12px 24px;
      background: var(--button-primary);
      color: white;
      font-weight: bold;
      text-decoration: none;
      border-radius: 8px;
      transition: all 0.3s ease;
      display: inline-flex;
      align-items: center;
      gap: 8px;
      white-space: nowrap;
    }

    .admin-actions a:hover {
      transform: translateY(-2px);
      box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
    }

    .manga-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
      gap: 25px;
    }

    .manga-card {
      background: var(--bg-card);
      border-radius: 12px;
      overflow: hidden;
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
      position: relative;
      transition: all 0.3s ease;
      border: 1px solid var(--input-border);
    }

    .manga-card:hover {
      transform: translateY(-5px);
      box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2);
    }

    .img-wrapper {
      position: relative;
      width: 100%;
      height: 320px;
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
      transition: transform 0.3s ease;
    }

    .manga-card:hover .img-wrapper img {
      transform: scale(1.05);
    }

    .card-buttons-top {
      position: absolute;
      top: 12px;
      right: 12px;
      display: flex;
      flex-direction: column;
      gap: 10px;
      opacity: 0;
      pointer-events: none;
      transition: opacity 0.3s ease;
      z-index: 2;
    }

    .manga-card:hover .card-buttons-top {
      opacity: 1;
      pointer-events: all;
    }

    .card-buttons-top a {
      width: 40px;
      height: 40px;
      border-radius: 50%;
      background: rgba(0, 0, 0, 0.7);
      color: white;
      display: flex;
      justify-content: center;
      align-items: center;
      text-decoration: none;
      font-size: 18px;
      border: 1px solid var(--accent-color);
      transition: all 0.3s ease;
    }

    .card-buttons-top a:hover {
      background: var(--accent-color);
      color: white;
      transform: scale(1.1);
    }

    .card-buttons-top .eliminar:hover {
      background: #ff4c4c;
      border-color: #ff4c4c;
    }

    .manga-card .info {
      padding: 18px;
      background: var(--bg-card);
    }

    .manga-card h3 {
      color: var(--accent-color);
      font-size: 1.2rem;
      margin: 0;
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
    }

    .btn-volver {
      position: fixed;
      top: 32px;
      left: 32px;
      background: var(--bg-card);
      color: var(--accent-color);
      border-radius: 12px;
      padding: 12px 24px 12px 18px;
      font-weight: 700;
      font-size: 1.1rem;
      text-decoration: none;
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
      border: 2px solid var(--accent-color);
      transition: all 0.3s ease;
      z-index: 10;
      display: inline-flex;
      align-items: center;
      gap: 8px;
    }

    .btn-volver:hover,
    .btn-volver:focus {
      background: var(--accent-color);
      color: var(--button-text-color);
      box-shadow: 0 8px 24px rgba(138, 43, 226, 0.3);
      transform: translateY(-2px);
      outline: none;
    }

   body {
      min-height: 100vh;
      margin: 0;
      padding: 0;
      box-sizing: border-box;
      font-family: 'Roboto', Arial, sans-serif;
      background: var(--bg-primary);
      color: var(--text-primary);
    }



footer {
  position: fixed;
  bottom: 0;
  left: 0;
  right: 0;
  padding: 20px;
  text-align: center;
  color: var(--text-secondary);
  background-color: var(--bg-primary);
}




    .theme-switcher {
      position: fixed;
      top: 30px;
      right: 30px;
      z-index: 10;
    }

    @media (max-width: 1024px) {
      .catalog-container {
        margin-top: 90px;
        padding: 0 15px;
      }

      .manga-grid {
        grid-template-columns: repeat(auto-fill, minmax(220px, 1fr));
        gap: 20px;
      }

      .img-wrapper {
        height: 300px;
      }

      .catalog-header form {
        flex-direction: row;
      }

      .admin-actions a {
        padding: 10px 20px;
        font-size: 0.9rem;
      }
    }

    @media (max-width: 768px) {
      .catalog-container {
        margin-top: 80px;
      }

      .manga-grid {
        grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
        gap: 15px;
      }

      .img-wrapper {
        height: 250px;
      }

      .catalog-header input,
      .catalog-header select,
      .catalog-header button {
        padding: 10px 12px;
        font-size: 0.9rem;
      }

      .manga-card .info {
        padding: 12px;
      }

      .manga-card h3 {
        font-size: 1.1rem;
      }

      .btn-volver {
        font-size: 0.9rem;
        padding: 8px 12px;
      }
    }

    @media (max-width: 576px) {
      .catalog-container {
        margin-top: 70px;
      }

      .catalog-header {
        gap: 12px;
      }

      .catalog-header form {
        flex-direction: column;
      }

      .manga-grid {
        grid-template-columns: repeat(auto-fill, minmax(160px, 1fr));
      }

      .img-wrapper {
        height: 220px;
      }

      .admin-actions a {
        width: 100%;
        justify-content: center;
        text-align: center;
      }

      .card-buttons-top a {
        width: 36px;
        height: 36px;
        font-size: 16px;
      }

      .btn-volver {
        top: 10px;
        left: 10px;
        padding: 6px 10px;
      }
    }

    @media (max-width: 480px) {
      .catalog-container {
        margin-top: 60px;
      }

      .manga-grid {
        grid-template-columns: 1fr;
        max-width: 350px;
        margin: 0 auto;
      }

      .img-wrapper {
        height: 400px;
      }

      .catalog-header input,
      .catalog-header select {
        min-width: 0;
      }

      .theme-switcher {
        top: 10px;
        right: 10px;
      }

      .btn-volver {
        font-size: 0.8rem;
      }
    }

    @media (max-width: 400px) {
      .img-wrapper {
        height: 350px;
      }

      .manga-card h3 {
        font-size: 1rem;
      }

      .card-buttons-top {
        top: 8px;
        right: 8px;
        gap: 6px;
      }

      .card-buttons-top a {
        width: 32px;
        height: 32px;
        font-size: 14px;
      }
    }
  </style>
</head>
<body>
<a href="./Admin/admin_dashboard.php" class="btn-volver" title="Volver al dashboard">
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
      <a href="./Admin/subidaMangaForm.php">+ Subir nuevo manga</a>
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
            <a href="./Admin/editar_manga.php?id=<?= $manga['MangaID'] ?>" title="Editar">✏️</a>
            <a href="./Admin/eliminar_manga.php?id=<?= $manga['MangaID'] ?>" class="eliminar" title="Eliminar"
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
