<?php
session_start();
require_once '../Config/db.php';

$generoSeleccionado = $_GET['genero'] ?? '';
$busqueda = $_GET['buscar'] ?? '';

// Redirección aleatoria
if (isset($_GET['sorprendeme'])) {
    $sqlAleatorio = "SELECT TOP 1 MangaID FROM Mangas ORDER BY NEWID()";
    $stmtAleatorio = sqlsrv_query($conn, $sqlAleatorio);
    
    if ($row = sqlsrv_fetch_array($stmtAleatorio, SQLSRV_FETCH_ASSOC)) {
        header("Location: detalle_manga.php?id=" . $row['MangaID']);
        exit();
    }
}

// Obtener géneros
$generos = [];
$stmtGeneros = sqlsrv_query($conn, "SELECT GeneroID, Nombre FROM Generos");
while ($row = sqlsrv_fetch_array($stmtGeneros, SQLSRV_FETCH_ASSOC)) {
    $generos[] = $row;
}

// Obtener mangas con filtros
$sql = "SELECT M.MangaID, M.Titulo, M.Autor, M.Descripcion, M.Estado, M.FechaPublicacion, 
               M.URLMangaDrive, M.URLPortada, M.URLPortadaWeb, G.Nombre AS Genero
        FROM Mangas M
        INNER JOIN Generos G ON M.GeneroID = G.GeneroID
        WHERE 1=1";
$params = [];

if (!empty($generoSeleccionado)) {
    $sql .= " AND M.GeneroID = ?";
    $params[] = $generoSeleccionado;
}

if (!empty($busqueda)) {
    // Búsqueda sin distinguir mayúsculas ni acentos
    $sql .= " AND M.Titulo COLLATE Latin1_General_CI_AI LIKE ?";
    $params[] = '%' . $busqueda . '%';
}

$stmt = sqlsrv_query($conn, $sql, $params);
$mangas = [];

while ($row = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC)) {
    // Elegir portada local si está seteada, de lo contrario usar la de Drive
    $portadaWeb = trim($row['URLPortadaWeb']);
    $row['PortadaElegida'] = (!empty($portadaWeb)) ? $portadaWeb : $row['URLPortada'];
    $mangas[] = $row;
}
?>

<!DOCTYPE html>
<html lang="es">
<head>
  <meta charset="UTF-8">
  <title>Catálogo - Manga Verse</title>
  <link rel="stylesheet" href="../assets/css/style.css">
  <style>
    html, body {
      min-height: 100%;
      height: 100%;
      margin: 0;
      padding: 0;
      box-sizing: border-box;
    }
    body {
      display: flex;
      flex-direction: column;
      min-height: 100vh;
      background: var(--bg-primary, #1c1c28);
      color: var(--text-primary, #f0f0f0);
      font-family: 'Roboto', Arial, sans-serif;
    }
    header {
      background: var(--bg-header, #23233a);
      position: sticky;
      top: 0;
      z-index: 20;
      width: 100%;
      box-shadow: 0 2px 6px rgba(30,30,50,0.18);
    }
    .header-container {
      max-width: 1200px;
      margin: 0 auto;
      display: flex;
      align-items: center;
      justify-content: space-between;
      gap: 20px;
      padding: 18px 24px;
      flex-wrap: wrap;
    }
    .logo {
      display: flex;
      align-items: center;
      gap: 12px;
      font-size: 1.3rem;
      font-weight: 700;
      color: var(--accent-color, #8a2be2);
    }
    .header-logo {
      width: 40px;
      height: 40px;
      border-radius: 9px;
      object-fit: cover;
    }
    nav ul {
      list-style: none;
      display: flex;
      gap: 28px;
      padding: 0;
      margin: 0;
    }
    nav a {
      color: var(--text-primary, #fff);
      text-decoration: none;
      font-weight: 500;
      padding: 7px 13px;
      border-radius: 16px;
      transition: background 0.2s;
    }
    nav a:hover {
      background: var(--accent-color, #8a2be2);
      color: #fff;
    }
    .theme-switcher button {
      background: none;
      border: none;
      cursor: pointer;
      font-size: 1.35rem;
      color: var(--text-primary, #fff);
      transition: transform 0.2s;
    }
    .theme-switcher button:active {
      transform: scale(1.2);
    }
    .profile {
      font-size: 1rem;
      color: var(--text-primary, #fff);
      margin-left: 8px;
      white-space: nowrap;
    }

    .catalog-container {
      max-width: 1200px;
      width: 100%;
      margin: 40px auto 0;
      padding: 0 18px;
      flex: 1 1 auto;
    }
    .catalog-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 30px;
      flex-wrap: wrap;
      gap: 18px;
    }
    .catalog-header form {
      display: flex;
      flex-wrap: wrap;
      gap: 10px;
      width: 100%;
      align-items: center;
      justify-content: flex-start;
    }
    .catalog-header input, .catalog-header select, .catalog-header button {
      padding: 10px;
      border-radius: 8px;
      border: 1px solid var(--input-border, #39396b);
      background: var(--input-bg, #232346);
      color: var(--text-primary, #fff);
      margin-right: 0;
      min-width: 130px;
      margin-bottom: 0;
      font-size: 1rem;
      transition: border .2s;
    }
    .catalog-header input:focus,
    .catalog-header select:focus {
      outline: none;
      border-color: var(--accent-color, #8a2be2);
    }
    .manga-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(240px, 1fr));
      gap: 25px;
      width: 100%;
    }
    .manga-card {
      background: var(--bg-card, #232346);
      box-shadow: 0 4px 10px rgba(0,0,0,0.29);
      border-radius: 14px;
      overflow: hidden;
      transition: transform 0.28s;
      display: flex;
      flex-direction: column;
      height: 100%;
    }
    .manga-card:hover {
      transform: scale(1.035);
      box-shadow: 0 6px 18px rgba(138,43,226,0.21);
    }
    .manga-card img {
      width: 100%;
      height: 320px;
      object-fit: cover;
      background: #151526;
      display: block;
    }
    .manga-card .info {
      padding: 18px 14px 12px 14px;
      flex: 1 1 auto;
      display: flex;
      flex-direction: column;
      gap: 7px;
    }
    .manga-card .info h3 {
      margin: 0 0 8px 0;
      color: var(--text-primary, #fff);
      font-size: 1.15rem;
      font-weight: bold;
      word-break: break-word;
    }
    .manga-card .info p {
      color: var(--text-secondary, #c6c6ef);
      font-size: 0.95rem;
      margin: 0 0 3px 0;
      word-break: break-word;
    }
    .manga-card .leer-btn {
      display: inline-block;
      background: linear-gradient(135deg, #1e90ff, #8a2be2);
      color: #fff;
      padding: 10px 18px;
      border-radius: 25px;
      text-decoration: none;
      font-weight: 700;
      font-size: 1rem;
      text-align: center;
      margin-top: auto;
      transition: background .18s;
    }
    .manga-card .leer-btn:hover {
      filter: brightness(1.13);
    }

    /* --- MEDIA QUERIES --- */
    @media (max-width: 1100px) {
      .header-container {
        padding: 16px 10px;
        gap: 12px;
      }
      .catalog-container {
        max-width: 1000px;
      }
      .manga-grid {
        gap: 18px;
        grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
      }
    }
    @media (max-width: 800px) {
      .header-container {
        flex-direction: column;
        align-items: flex-start;
        gap: 10px;
      }
      nav ul {
        gap: 18px;
      }
      .profile {
        font-size: 0.97rem;
        margin-left: 0;
      }
      .catalog-header {
        flex-direction: column;
        align-items: stretch;
        gap: 8px;
      }
      .catalog-header form {
        flex-wrap: wrap;
        gap: 8px;
      }
      .catalog-header input,
      .catalog-header select,
      .catalog-header button {
        font-size: 0.98rem;
        padding: 8px;
      }
      .manga-grid {
        gap: 15px;
        grid-template-columns: repeat(auto-fill, minmax(160px, 1fr));
      }
      .manga-card img {
        height: 210px;
      }
    }
    @media (max-width: 520px) {
      .header-logo { width: 32px; height: 32px; }
      .logo { font-size: 1.05rem; }
      nav ul { gap: 10px; }
      .catalog-container {
        margin: 25px auto 0;
        padding: 0 7px;
      }
      .catalog-header input,
      .catalog-header select,
      .catalog-header button {
        font-size: 0.85rem;
        padding: 7px;
        min-width: 95px;
      }
      .manga-grid {
        grid-template-columns: 1fr;
      }
      .manga-card img {
        height: 160px;
      }
      .manga-card .info h3 { font-size: 1rem; }
    }

    /* FOOTER PEGADO */
    .main-footer {
      flex-shrink: 0;
      text-align: center;
      padding: 32px 10px 22px 10px;
      background: var(--bg-tertiary, #151526);
      color: var(--text-secondary, #b7b7de);
      font-size: 0.95rem;
      box-shadow: 0 -4px 12px rgba(137, 129, 248, 0.12);
      border-top: 1px solid var(--input-border, #39396b);
      width: 100%;
      margin-top: auto;
    }
        .sorprendeme-btn {
      display: inline-block;
      background: linear-gradient(135deg, #ff6b6b, #ff8e8e);
      color: white;
      padding: 10px 20px;
      border-radius: 30px;
      text-decoration: none;
      font-weight: bold;
      transition: transform 0.3s, box-shadow 0.3s;
      border: none;
      cursor: pointer;
      box-shadow: 0 4px 8px rgba(0,0,0,0.2);
    }

    .sorprendeme-btn:hover {
      transform: scale(1.05);
      box-shadow: 0 6px 12px rgba(0,0,0,0.3);
    }
  </style>
</head>
<body>

<header>
  <div class="header-container">
    <div class="logo">
      <img src="../assets/imgs/Logito.png" alt="Logo Manga Verse" class="header-logo">
      <span>Manga Verse</span>
    </div>
    <nav>
      <ul>
        <li><a href="./Client/dashboard.php">Inicio</a></li>
        <li><a href="favoritos.php">Mi Lista</a></li>
        <li><a href="catalogo.php">Catálogo</a></li>
      </ul>
    </nav>

    <div class="theme-switcher">
      <button id="theme-toggle" aria-label="Cambiar tema">
        <span class="dark-icon">🌙</span>
        <span class="light-icon">☀️</span>
      </button>
    </div>  

    <div class="profile">
      <?php if (isset($_SESSION['nombre'])): ?>
        <span>Hola, <?php echo $_SESSION['nombre']; ?></span>
      <?php else: ?>
        <a href="../Public/login.html">Iniciar Sesión</a>
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
    <a href="catalogo.php?sorprendeme=1" class="sorprendeme-btn">🎲 Sorpréndeme</a>
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

<script src="../assets/js/theme-switcher.js"></script>
</body>
</html> 