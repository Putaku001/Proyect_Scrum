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

    // Convierte la URL pública en ruta física antes de comprobar file_exists
    $pathFisico = $_SERVER['DOCUMENT_ROOT'] . $portadaWeb;

    // Usa portada local si existe; de lo contrario cae a la de Drive
    $row['PortadaElegida'] = (!empty($portadaWeb) && file_exists($pathFisico))
                             ? $portadaWeb
                             : $row['URLPortada'];

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
    :root {
  --bg-primary: #181928;
  --bg-secondary: #232346;
  --bg-tertiary: #101020;
  --bg-card: #21213a;
  --input-bg: #23233b;
  --input-border: #36368a;
  --accent-color: #7c4cff;
  --button-primary: linear-gradient(90deg, #7c4cff 40%, #5e96fc 100%);
  --button-text-color: #fff;
  --text-primary: #fafaff;
  --text-secondary: #adb0c8;
  --shadow-strong: 0 8px 32px 0 rgba(39, 23, 107, 0.18);
  --shadow-card: 0 2px 18px 0 rgba(22,18,63,0.08);
}

body {
  min-height: 100vh;
  background: var(--bg-primary);
  color: var(--text-primary);
  font-family: 'Roboto', Arial, sans-serif;
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

/* Sticky header look */
.catalog-header {
  background: rgba(28, 28, 48, 0.90);
  backdrop-filter: blur(4px);
  border-radius: 14px;
  box-shadow: var(--shadow-strong);
  position: sticky;
  top: 0;
  z-index: 5;
  padding: 18px 26px;
  margin-bottom: 32px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-wrap: wrap;
  gap: 18px;
  border: 1.5px solid var(--input-border);
  animation: fadein 0.6s;
}

/* Catálogo container */
.catalog-container {
  max-width: 1220px;
  margin: 100px auto 54px;
  padding: 0 22px 36px 22px;
  background: transparent;
  border-radius: 18px;
}

.catalog-header form {
  display: flex;
  gap: 14px;
  flex-wrap: wrap;
  flex-grow: 1;
  align-items: center;
}

.catalog-header input,
.catalog-header select {
  background: var(--input-bg);
  color: var(--text-primary);
  padding: 13px 16px;
  border-radius: 9px;
  border: 1.5px solid var(--input-border);
  font-size: 1rem;
  min-width: 210px;
  flex-grow: 1;
  box-shadow: 0 1.5px 7px 0 rgba(55,70,125,0.05);
  transition: border 0.2s, box-shadow 0.3s;
}

.catalog-header input:focus,
.catalog-header select:focus {
  outline: none;
  border-color: var(--accent-color);
  box-shadow: 0 0 0 2px rgba(124,76,255,0.13);
}

.catalog-header button {
  padding: 13px 30px;
  border-radius: 9px;
  border: none;
  background: var(--button-primary);
  color: var(--button-text-color);
  font-weight: bold;
  font-size: 1.06rem;
  cursor: pointer;
  letter-spacing: 0.05em;
  box-shadow: 0 4px 14px 0 rgba(124,76,255,0.10);
  transition: background 0.2s, transform 0.2s, box-shadow 0.2s;
}
.catalog-header button:hover {
  background: linear-gradient(90deg, #9d4dff 30%, #5ec8fc 100%);
  transform: translateY(-1.5px) scale(1.04);
  box-shadow: 0 8px 24px 0 rgba(124,76,255,0.12);
}

/* Botón volver */
.btn-volver {
  position: fixed;
  top: 30px;
  left: 30px;
  background: rgba(28,28,48,0.78);
  color: var(--accent-color);
  border-radius: 13px;
  padding: 12px 27px 12px 20px;
  font-weight: 700;
  font-size: 1.13rem;
  text-decoration: none;
  box-shadow: 0 8px 32px rgba(124, 76, 255, 0.12);
  border: 2px solid var(--accent-color);
  transition: all 0.2s;
  z-index: 20;
  display: flex;
  align-items: center;
  gap: 9px;
  backdrop-filter: blur(2.5px);
}
.btn-volver:hover,
.btn-volver:focus {
  background: var(--accent-color);
  color: var(--button-text-color);
  transform: translateY(-2px) scale(1.04);
  outline: none;
}

/* Acciones admin */
.admin-actions a {
  padding: 13px 32px;
  background: var(--button-primary);
  color: #fff;
  font-weight: bold;
  text-decoration: none;
  border-radius: 9px;
  font-size: 1.08rem;
  letter-spacing: 0.04em;
  box-shadow: 0 4px 14px 0 rgba(124,76,255,0.10);
  display: inline-flex;
  align-items: center;
  gap: 9px;
  transition: background 0.15s, box-shadow 0.18s, transform 0.18s;
}
.admin-actions a:hover {
  background: linear-gradient(90deg, #a67cfc 0%, #6ae3ff 100%);
  transform: translateY(-2px) scale(1.04);
  box-shadow: 0 10px 26px 0 rgba(124,76,255,0.17);
}

.manga-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(265px, 1fr));
  gap: 30px;
  margin-top: 16px;
  animation: fadein 1.2s;
}

/* CARD */
.manga-card {
  background: var(--bg-card);
  border-radius: 18px;
  overflow: hidden;
  box-shadow: var(--shadow-card);
  position: relative;
  border: 1.5px solid var(--input-border);
  display: flex;
  flex-direction: column;
  transition: transform 0.22s, box-shadow 0.22s;
  animation: floatin 0.8s;
}
.manga-card:hover {
  transform: translateY(-7px) scale(1.03);
  box-shadow: 0 12px 36px rgba(124, 76, 255, 0.16);
  border-color: var(--accent-color);
}

.img-wrapper {
  width: 100%;
  height: 320px;
  overflow: hidden;
  border-bottom: 2.5px solid var(--input-border);
  position: relative;
  background: #23233b;
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
  transition: transform 0.25s;
  display: block;
  border-radius: 0;
}
.manga-card:hover .img-wrapper img {
  transform: scale(1.07) rotate(-0.6deg);
}

.card-buttons-top {
  position: absolute;
  top: 14px;
  right: 14px;
  display: flex;
  flex-direction: column;
  gap: 11px;
  opacity: 0;
  pointer-events: none;
  transition: opacity 0.22s;
  z-index: 2;
}
.manga-card:hover .card-buttons-top {
  opacity: 1;
  pointer-events: all;
}

.card-buttons-top a {
  width: 44px;
  height: 44px;
  border-radius: 50%;
  background: rgba(29, 20, 59, 0.88);
  color: #fff;
  display: flex;
  justify-content: center;
  align-items: center;
  font-size: 1.23em;
  border: 2px solid var(--accent-color);
  transition: background 0.18s, border 0.18s, transform 0.18s;
}
.card-buttons-top a:hover {
  background: var(--accent-color);
  color: white;
  transform: scale(1.16);
}
.card-buttons-top .eliminar:hover {
  background: #ff4c4c;
  border-color: #ff4c4c;
}

.manga-card .info {
  padding: 21px 17px 19px 17px;
  background: transparent;
  display: flex;
  flex-direction: column;
  gap: 6px;
  min-height: 70px;
}

.manga-card h3 {
  color: var(--accent-color);
  font-size: 1.15rem;
  margin: 0;
  line-height: 1.2;
  letter-spacing: 0.01em;
  font-weight: 700;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

footer {
  position: fixed;
  bottom: 0; left: 0; right: 0;
  padding: 20px;
  text-align: center;
  color: var(--text-secondary);
  background: var(--bg-primary);
  font-size: 1.07rem;
  box-shadow: 0 -2px 16px rgba(39, 23, 107, 0.10);
  z-index: 2;
  border-top: 1.5px solid var(--input-border);
}

@media (max-width: 1120px) {
  .catalog-container { padding: 0 7px; }
}
@media (max-width: 900px) {
  .manga-grid { grid-template-columns: repeat(auto-fill, minmax(190px, 1fr)); }
  .img-wrapper { height: 210px; }
}
@media (max-width: 768px) {
  .catalog-header { padding: 10px 9px; }
  .admin-actions a { font-size: 0.97rem; padding: 11px 20px; }
  .img-wrapper { height: 160px; }
  .btn-volver { font-size: 0.97rem; top: 9px; left: 9px; padding: 8px 16px 8px 12px; }
  .catalog-header input, .catalog-header select, .catalog-header button { font-size: 0.92rem; }
}
@media (max-width: 480px) {
  .manga-grid { grid-template-columns: 1fr; gap: 15px; }
  .catalog-header { flex-direction: column; gap: 8px; }
  .catalog-header form { flex-direction: column; gap: 8px; }
  .img-wrapper { height: 90vw; max-height: 310px; }
  .btn-volver { font-size: 0.85rem; padding: 7px 9px; }
}
@media (max-width: 360px) {
  .manga-card .info { padding: 9px 4px; }
  .img-wrapper { height: 150px; }
  .card-buttons-top a { width: 32px; height: 32px; font-size: 1em; }
}

@keyframes fadein {
  0% { opacity: 0; transform: translateY(12px);}
  100% { opacity: 1; transform: none;}
}
@keyframes floatin {
  0% { opacity: 0.7; transform: scale(0.98);}
  100% { opacity: 1; transform: none;}
}

footer {
      flex-shrink: 0;
      text-align: center;
      padding: 32px 10px 22px 10px;
      background: var(--bg-tertiary, #151526);
      color: var(--text-secondary, #b7b7de);
      font-size: 0.96rem;
      box-shadow: 0 -4px 10px rgba(137, 129, 248, 0.08);
      border-top: 1px solid var(--input-border, #39396b);
      width: 100%;
      margin-top: auto;
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
          <a href="Admin/eliminar_manga.php?id=<?= $manga['MangaID'] ?>"
   class="eliminar" title="Eliminar"
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
